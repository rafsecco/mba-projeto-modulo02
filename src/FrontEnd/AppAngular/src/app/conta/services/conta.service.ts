import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Usuario } from "../models/usuario";
import { Observable } from "rxjs";
import { map, catchError } from "rxjs/operators";
import { BaseService } from "../../services/base.service";

@Injectable({
  providedIn: 'root'
})
export class ContaService extends BaseService {

  constructor(protected override http: HttpClient) {
    super(http);
  }

  registrarusuario(usuario: Usuario): Observable<{ accessToken: string }> {
    return this.http.post<{ accessToken: string }>(
      `${this.UrlServiceV1}Clientes`,
      usuario,
      this.ObterHeaderJson()
    ).pipe(
      map(this.extractData),
      catchError(this.serviceError)
    );
  }

login(usuario: Usuario): Observable<{ token: string }> {
  return this.http
    .post<{ token: string }>(
      `${this.UrlServiceV1}Users/login`,
      usuario,
      this.ObterHeaderJson()
    )
    .pipe(
      catchError((error) => this.serviceError(error))  
    );
}


  override getCliente(): Observable<any> {
    return super.getCliente();
  }
}
