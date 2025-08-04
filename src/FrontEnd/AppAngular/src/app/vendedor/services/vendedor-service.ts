import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Vendedor } from '../models/vendedor';

@Injectable({
  providedIn: 'root'
})
export class VendedorService {

  constructor(private http: HttpClient) { }

  protected UrlServiceV1: string = "https://localhost:7011/";

  obterVendedorPorId(id:string) : Observable<Vendedor> {
    console.log('Obtendo vendedor com ID:', id);
    return this.http
      .get<Vendedor>(this.UrlServiceV1 + `Vendedores/${id}`);
  }
}
