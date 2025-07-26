import { Component, OnInit } from '@angular/core';
import { Produto } from '../models/produto';
import { ProdutoService } from '../services/produto-service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-detalhe',
  imports: [],
  templateUrl: './detalhe-component.html',
  styleUrl: './detalhe-component.css',
  providers: [],
})
export class DetalheComponent implements OnInit {

  //imagens: string = environment.imagensUrl;
  public produto: Produto | undefined;
  public id: string = '';

  constructor(
    private route: ActivatedRoute,
    private produtoService: ProdutoService) {
      this.id = route.snapshot.paramMap.get('id') || '';
    }

  ngOnInit(): void {
    this.produtoService.obterProdutoPorId(this.id).subscribe({
      next: (objProduto) => {
        this.produto = objProduto;
      },
      error: (err) => {
        console.error('Erro ao obter produtos', err);
      }
    });
  }

}
