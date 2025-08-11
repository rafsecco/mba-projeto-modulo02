import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink, Router } from '@angular/router';
import { switchMap } from 'rxjs/operators';

import { Produto } from '../models/produto';
import { ProdutoService } from '../services/produto-service';
import { FavoritoService } from '../../favorito/services/favorito-service';
import { Vendedor } from '../../vendedor/models/vendedor';
import { VendedorService } from '../../vendedor/services/vendedor-service';
import { Favorito } from '../../favorito/models/favorito';
import { LocalStorageUtils } from '../../utils/localstorage';

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
  public favoritos: Favorito[] = [];
  public id: string = '';
  logado = false;
  private localStorageUtils = new LocalStorageUtils();

  constructor(
    private route: ActivatedRoute,
    private produtoService: ProdutoService,
    private vendedorService: VendedorService,
    private favoritoService: FavoritoService,
    private router: Router
  ) {
    this.id = route.snapshot.paramMap.get('id') || '';
  }

  redirecionarLogin() {
    this.router.navigate(['/conta/login']);
  }

  alternaFavorito(currentProduto: Produto) {
    let isfavorito = this.verificaFavorito(currentProduto.id);

    if (isfavorito) {
      this.favoritoService.removerFavorito(currentProduto.id).subscribe({
        next: () => {
          this.favoritos = this.favoritos.filter(
            (f) => f.produtoId !== currentProduto.id
          );
          console.log('Favorito removido');
        },
        error: (err) => console.error('Erro ao remover favorito:', err),
      });
      return;
    }

    this.favoritoService.adicionarFavorito(currentProduto.id).subscribe({
      next: (novoFavorito) => {
        this.favoritos.push(novoFavorito);
        console.log('Favorito adicionado');
      },
      error: (err) => console.error('Erro ao adicionar favorito:', err),
    });
  }

  verificaFavorito(currentProdutoId: string) {
    let isfavorito = this.favoritos.some(
      (f) => f.produtoId == currentProdutoId
    );
    return isfavorito;
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

    this.favoritoService.obterFavoritos().subscribe({
      next: (favoritos) => {
        this.favoritos = favoritos;
      },
      error: (err) => {
        console.error('Erro ao obter favoritos', err);
      },
    });

    let token = this.localStorageUtils.obterTokenUsuario();
    this.logado = token != undefined;
  }
}
