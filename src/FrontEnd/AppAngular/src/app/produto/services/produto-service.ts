import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Produto } from '../models/produto';

@Injectable({
  providedIn: 'root'
})
export class ProdutoService {

  constructor(private http: HttpClient) { }

  protected UrlServiceV1: string = "https://localhost:7011/Produtos/";

  obterProdutos() : Observable<Produto[]> {
    return this.http
      .get<Produto[]>(this.UrlServiceV1 + "produtosValidos");
  }

  obterProdutoPorId(id:string) : Observable<Produto> {
    return this.http
      .get<Produto>(this.UrlServiceV1 + `${id}`);
  }

  obterProdutosPorCategoria(categoriaId: string): Observable<Produto[]> {
    return this.http
      .get<Produto[]>(`${this.UrlServiceV1}${categoriaId}/categoriaId`);
  }

}
