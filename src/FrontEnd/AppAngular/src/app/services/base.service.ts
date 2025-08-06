import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { throwError, Observable } from "rxjs";
import { LocalStorageUtils } from "../utils/localstorage";
import { environment } from "../environments/environment";

export abstract class BaseService {
  public LocalStorage = new LocalStorageUtils();
  protected UrlServiceV1: string = environment.apiUrlv1;

  constructor(protected http: HttpClient) {}

  protected ObterHeaderJson() {
    return {
      headers: new HttpHeaders({
        "Content-Type": "application/json",
      }),
    };
  }

  protected ObterHeaderAuthorization(): { headers: HttpHeaders } {
    const token = this.LocalStorage.obterTokenUsuario() || "";
    return {
      headers: new HttpHeaders({
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      }),
    };
  }

  protected extractData(response: any) {
    return response.data || {};
  }

  protected serviceError(response: Response | any) {
    let customError: string[] = [];

    if (response instanceof HttpErrorResponse) {
      if (response.statusText === "Unknown Error") {
        customError.push("Ocorreu um erro desconhecido");
        response.error.errors = customError;
      }
    }

    console.error(response);
    return throwError(response);
  }

  public getCliente(): Observable<any> {
    return this.http.get<any>(
      `{this.UrlServiceV1}/clientes`,
      this.ObterHeaderAuthorization()
    );
  }
}
