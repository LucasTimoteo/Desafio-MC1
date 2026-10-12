import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  ListarPedidosParams,
  Pedido,
  PedidoRequest,
  Produto,
  ResultadoPaginado,
  StatusPedido,
} from '../models/pedido.models';

@Injectable({ providedIn: 'root' })
export class PedidoService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiBaseUrl;

  listarPedidos(params: ListarPedidosParams): Observable<ResultadoPaginado<Pedido>> {
    let httpParams = new HttpParams()
      .set('pagina', params.pagina)
      .set('tamanhoPagina', params.tamanhoPagina);

    if (params.texto?.trim()) {
      httpParams = httpParams.set('texto', params.texto.trim());
    }
    if (params.status) {
      httpParams = httpParams.set('status', params.status);
    }

    return this.http.get<ResultadoPaginado<Pedido>>(`${this.baseUrl}/pedidos`, { params: httpParams });
  }

  obterPedido(id: string): Observable<Pedido> {
    return this.http.get<Pedido>(`${this.baseUrl}/pedidos/${id}`);
  }

  criarPedido(request: PedidoRequest): Observable<Pedido> {
    return this.http.post<Pedido>(`${this.baseUrl}/pedidos`, request);
  }

  atualizarPedido(id: string, request: PedidoRequest): Observable<Pedido> {
    return this.http.put<Pedido>(`${this.baseUrl}/pedidos/${id}`, request);
  }

  atualizarStatus(id: string, status: StatusPedido): Observable<Pedido> {
    return this.http.patch<Pedido>(`${this.baseUrl}/pedidos/${id}/status`, { status });
  }

  excluirPedido(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/pedidos/${id}`);
  }

  listarProdutos(): Observable<Produto[]> {
    return this.http.get<Produto[]>(`${this.baseUrl}/produtos`);
  }
}
