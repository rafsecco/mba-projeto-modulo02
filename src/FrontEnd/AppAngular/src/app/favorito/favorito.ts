import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

import { Produto } from '../produto/models/produto';
import { Favorito } from './models/favorito';
import { FavoritoService } from './services/favorito-service';

@Component({
  selector: 'app-favorito',
  imports: [CommonModule, RouterLink],
  templateUrl: './favorito.html',
  styleUrl: './favorito.css',
  providers: [FavoritoService],
})
export class FavoritoComponent implements OnInit {
  public favoritos: Favorito[] = [];

  constructor(private favoriteService: FavoritoService) {}

  removeFavorito(currentProduto: Produto) {
    this.favoriteService.removerFavorito(currentProduto.id).subscribe({
      next: () => {
        this.favoritos = this.favoritos.filter(
          (f) => f.produtoId !== currentProduto.id
        );
        console.log('Favorito removido');
      },
      error: (err) => console.error('Erro ao remover favorito:', err),
    });
  }

  ngOnInit(): void {
    this.favoriteService.obterFavoritos().subscribe({
      next: (favoritos) => {
        this.favoritos = favoritos;
      },
      error: (err) => {
        console.error('Erro ao obter favoritos', err);
      },
    });
  }
}
