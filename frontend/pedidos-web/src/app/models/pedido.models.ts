export type StatusPedido = 'Aberto' | 'Pago' | 'Cancelado';

export const STATUS_PEDIDO: readonly StatusPedido[] = ['Aberto', 'Pago', 'Cancelado'] as const;

export interface Produto {
  id: string;
  nome: string;
  preco: number;
}

export interface PedidoItem {
  id: string;
  produtoId: string;
  produtoNome: string;
  quantidade: number;
  precoUnitario: number;
  subtotal: number;
}

export interface Pedido {
  id: string;
  nomeCliente: string;
  descricao: string;
  valorTotal: number;
  status: StatusPedido;
  dataCriacao: string;
  createdAt: string;
  updatedAt: string;
  itens: PedidoItem[];
}

export interface PedidoItemRequest {
  produtoId: string;
  quantidade: number;
}

export interface PedidoRequest {
  nomeCliente: string;
  descricao: string;
  itens: PedidoItemRequest[];
}

export interface ResultadoPaginado<T> {
  itens: T[];
  total: number;
  pagina: number;
  tamanhoPagina: number;
  totalPaginas: number;
}

export interface ListarPedidosParams {
  texto?: string;
  status?: StatusPedido | '';
  pagina: number;
  tamanhoPagina: number;
}
