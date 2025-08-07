import { Injectable } from "@angular/core";
import { BaseService } from "../../services/base.service";
import { HttpClient } from "@angular/common/http";
import { catchError, map, Observable } from "rxjs";
import { Categoria } from "../models/categoria";

@Injectable({
  providedIn: 'root'
})
export class CategoriaService extends BaseService {
    constructor(protected http: HttpClient){
        super();
    }

   obterCategorias(): Observable<Categoria[]> {
  return this.http
    .get<Categoria[]>(`${this.UrlServiceV1}Categorias`)
    .pipe(
      map(this.extractData),
      catchError(this.serviceError)
    );
  }
}