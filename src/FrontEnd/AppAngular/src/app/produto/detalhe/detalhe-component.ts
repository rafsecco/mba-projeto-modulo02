import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { switchMap } from 'rxjs/operators';

import { Produto } from '../models/produto';
import { ProdutoService } from '../services/produto-service';
import { Vendedor } from '../../vendedor/models/vendedor';
import { VendedorService } from '../../vendedor/services/vendedor-service';

@Component({
  selector: 'app-detalhe',
  imports: [CommonModule, RouterLink],
  templateUrl: './detalhe-component.html',
  styleUrl: './detalhe-component.css',
  providers: [],
})
export class DetalheComponent implements OnInit {
  //imagens: string = environment.imagensUrl;
  public produto: Produto | undefined;
  public vendedor: Vendedor | undefined;
  public id: string = '';

  constructor(
    private route: ActivatedRoute,
    private produtoService: ProdutoService,
    private vendedorService: VendedorService
  ) {
    this.id = route.snapshot.paramMap.get('id') || '';
  }

  ngOnInit(): void {
    this.route.paramMap
      .pipe(
        switchMap((params) => {
          this.id = params.get('id') || '';
          return this.produtoService.obterProdutoPorId(this.id);
        }),
        switchMap((produto) => {
          this.produto = produto;
          return this.vendedorService.obterVendedorPorId(produto.vendedorId);
        })
      )
      .subscribe({
        next: (vendedor) => {
          this.vendedor = vendedor;
        },
        error: (err) => {
          console.error('Erro ao obter produtos', err);
        },
      });
  }
}
