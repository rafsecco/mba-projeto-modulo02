import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import { LocalStorageUtils } from '../utils/localstorage';
import { environment } from '../environments/environment';

export abstract class BaseService {
  public LocalStorage = new LocalStorageUtils();
  protected UrlServiceV1: string = environment.apiUrlv1;

  protected ObterHeaderJson() {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
  }

  protected ObterHeaderAuthorization(): { headers: HttpHeaders } {
    const token = this.LocalStorage.obterTokenUsuario() || '';
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      }),
    };
  }

  protected extractData(response: any) {
    return response.data || {};
  }

  protected serviceError(error: any): Observable<never> {
    let customError: string[] = [];

    if (error instanceof HttpErrorResponse) {
      if (error.statusText === 'Unknown Error') {
        customError.push('Ocorreu um erro desconhecido');
        error.error.errors = customError;
      }
    }

    console.error(error);
    return throwError(() => error);
  }
}
