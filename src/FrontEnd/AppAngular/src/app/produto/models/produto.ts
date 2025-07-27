import { Categoria } from "../../categoria/models/categoria";

export interface Produto {
  vendedorId: string;
  categoriaId: string;
  id: string;
  nome: string;
  descricao: string;
  preco: number;
  estoque: number;
  imagem: string;
  ativo: boolean;
  categoria: Categoria
}
