import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Favorito } from '../models/favorito';
import { BaseService } from '../../services/base.service';

@Injectable({
  providedIn: 'root',
})
export class FavoritoService extends BaseService {
  constructor(private http: HttpClient) {
    super();
  }

  protected url: string = `${this.UrlServiceV1}Clientes`;

  obterFavoritos(): Observable<Favorito[]> {
    return this.http.get<Favorito[]>(
      this.url + '/Favoritos',
      super.ObterHeaderAuthorization()
    );
  }

  adicionarFavorito(produtoId: string): Observable<any> {
    let result = this.http.post(
      `${this.url}/${produtoId}/Favoritos`,
      {},
      this.ObterHeaderAuthorization()
    );
    return result;
  }

  removerFavorito(produtoId: string): Observable<any> {
    let result = this.http.delete(
      `${this.url}/${produtoId}/Favoritos`,
      this.ObterHeaderAuthorization()
    );
    return result;
  }
}
