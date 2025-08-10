import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

import { Produto } from '../../produto/models/produto';
import { Favorito } from '../../favorito/models/favorito'; 
import { ProdutoService } from '../../produto/services/produto-service';
import { FavoritoService } from '../../favorito/services/favorito-service';
import { CategoriaService } from '../../categoria/services/categoria.service';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-home',
  imports: [CommonModule, RouterLink,FormsModule],
  templateUrl: './home-component.html',
  providers: [ProdutoService],
})
export class HomeComponent implements OnInit {
  public produtos: Produto[] = [];
  public favoritos: Favorito[] = [];
  isFavorited = false;
  public categorias: any[] = [];
  public categoriaSelecionada: string = '';

  constructor(
    private produtoService: ProdutoService,
    private favoritoService: FavoritoService,
    private categoriaService: CategoriaService
  ) {}

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
    this.produtoService.obterProdutos().subscribe({
      next: (produtos) => {
        this.produtos = produtos;
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

    this.categoriaService.obterCategorias().subscribe({
    next: (categorias) => this.categorias = categorias,
    error: (err) => console.error('Erro ao carregar categorias', err),
    });
  }

  filtrarProdutos() 
  {
    this.produtos = [];
    if (!this.categoriaSelecionada) {       
      this.produtoService.obterProdutos().subscribe({
        next: (produtos) => this.produtos = produtos.filter(p => p.ativo),
        error: (err) => console.error('Erro ao carregar todos os produtos', err),
      });
    } else {       
      this.produtoService.obterProdutosPorCategoria(this.categoriaSelecionada).subscribe({
        next: (produtos) => this.produtos = produtos.filter(p => p.ativo),
        error: (err) => console.error('Erro ao filtrar produtos por categoria', err),
      });
    }
  }

  trackById(index: number, item: Produto) {
  return item.id;
  }
}
