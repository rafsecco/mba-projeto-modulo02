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

  registrarusuario(usuario: Usuario): Observable<Usuario> {
    return this.http.post<Usuario>(
      `${this.UrlServiceV1}clientes`,
      usuario,
      this.ObterHeaderJson()
    ).pipe(
      map(this.extractData),
      catchError(this.serviceError)
    );
  }

  login(usuario: Usuario): Observable<any> {
    return this.http.post<any>(
      `${this.UrlServiceV1}users/login`,
      usuario,
      this.ObterHeaderJson()
    ).pipe(
      map(this.extractData),
      catchError(this.serviceError)
    );
  }

  override getCliente(): Observable<any> {
    return super.getCliente();
  }
}
