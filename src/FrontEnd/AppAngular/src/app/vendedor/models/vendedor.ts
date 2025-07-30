import { Produto } from "../../produto/models/produto";

export interface Vendedor {
  id: string;
  userId: string;
  ativo: boolean;
  produtos: Produto[];
}
