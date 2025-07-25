import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

import { Produto } from '../../produto/models/produto';
import { ProdutoService } from '../../produto/services/produto-service';

@Component({
  standalone: true,
  selector: 'app-home',
  imports: [CommonModule, RouterLink],
  templateUrl: './home-component.html',
  providers: [ProdutoService]
})
export class HomeComponent implements OnInit {

  public produtos: Produto[] = [];

  constructor(private produtoService: ProdutoService) { }

  ngOnInit(): void {
    this.produtoService.obterProdutos().subscribe({
      next: (produtos) => {
        this.produtos = produtos;
      },
      error: (err) => {
        console.error('Erro ao obter produtos', err);
      }
    });
  }

}
