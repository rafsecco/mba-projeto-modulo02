import { Produto } from "../../produto/models/produto";

export interface Favorito {
  clienteId: string;
  produtoId: string;
  produto: Produto;
}
