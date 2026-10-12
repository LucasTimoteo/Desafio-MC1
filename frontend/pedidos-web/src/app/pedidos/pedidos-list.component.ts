import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit, inject, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Subscription, debounceTime, interval } from 'rxjs';
import { Pedido, Produto, STATUS_PEDIDO, StatusPedido } from '../models/pedido.models';
import { PedidoService } from '../services/pedido.service';
import { ConfirmDialogComponent } from '../shared/confirm-dialog.component';
import { extrairMensagensErro } from '../shared/erros';
import { PedidoFormDialogComponent } from './pedido-form-dialog.component';

const INTERVALO_POLLING_MS = 10_000;

@Component({
  selector: 'app-pedidos-list',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatTooltipModule,
    MatProgressBarModule,
  ],
  templateUrl: './pedidos-list.component.html',
  styleUrl: './pedidos-list.component.css',
})
export class PedidosListComponent implements OnInit, OnDestroy {
  private readonly service = inject(PedidoService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  readonly colunas = ['nomeCliente', 'descricao', 'valorTotal', 'status', 'dataCriacao', 'acoes'];
  readonly statusDisponiveis = STATUS_PEDIDO;

  readonly pedidos = signal<Pedido[]>([]);
  readonly total = signal(0);
  readonly carregando = signal(false);
  readonly erro = signal<string | null>(null);
  readonly ultimaAtualizacao = signal<Date | null>(null);

  readonly pagina = signal(0);
  readonly tamanhoPagina = signal(10);
  readonly tamanhosPagina = [5, 10, 25, 50];

  readonly filtroControl = new FormControl<string>('', { nonNullable: true });
  readonly statusControl = new FormControl<StatusPedido | ''>('', { nonNullable: true });

  private produtos: Produto[] = [];
  private dialogAberto = false;
  private readonly assinaturas = new Subscription();

  ngOnInit(): void {
    this.carregarProdutos();
    this.carregar();

    this.assinaturas.add(
      this.filtroControl.valueChanges.pipe(debounceTime(400)).subscribe(() => {
        this.pagina.set(0);
        this.carregar();
      }),
    );

    this.assinaturas.add(
      this.statusControl.valueChanges.subscribe(() => {
        this.pagina.set(0);
        this.carregar();
      }),
    );

    this.assinaturas.add(
      interval(INTERVALO_POLLING_MS).subscribe(() => {
        if (!this.dialogAberto) {
          this.carregar(true);
        }
      }),
    );
  }

  ngOnDestroy(): void {
    this.assinaturas.unsubscribe();
  }

  carregar(silencioso = false): void {
    if (!silencioso) {
      this.carregando.set(true);
    }

    this.service
      .listarPedidos({
        texto: this.filtroControl.value,
        status: this.statusControl.value,
        pagina: this.pagina() + 1,
        tamanhoPagina: this.tamanhoPagina(),
      })
      .subscribe({
        next: (resultado) => {
          this.pedidos.set(resultado.itens);
          this.total.set(resultado.total);
          this.erro.set(null);
          this.carregando.set(false);
          this.ultimaAtualizacao.set(new Date());
        },
        error: (erro) => {
          this.erro.set(extrairMensagensErro(erro).join(' '));
          this.carregando.set(false);
        },
      });
  }

  onPagina(evento: PageEvent): void {
    this.pagina.set(evento.pageIndex);
    this.tamanhoPagina.set(evento.pageSize);
    this.carregar();
  }

  novoPedido(): void {
    this.abrirFormulario();
  }

  editar(pedido: Pedido): void {
    this.abrirFormulario(pedido);
  }

  excluir(pedido: Pedido): void {
    this.dialogAberto = true;
    const ref = this.dialog.open(ConfirmDialogComponent, {
      data: {
        titulo: 'Excluir pedido',
        mensagem: `Deseja excluir o pedido de ${pedido.nomeCliente}?`,
        confirmarTexto: 'Excluir',
      },
    });

    ref.afterClosed().subscribe((confirmado: boolean) => {
      this.dialogAberto = false;
      if (!confirmado) {
        return;
      }
      this.service.excluirPedido(pedido.id).subscribe({
        next: () => {
          this.notificar('Pedido excluído.');
          this.carregar();
        },
        error: (erro) => this.notificar(extrairMensagensErro(erro).join(' ')),
      });
    });
  }

  mudarStatus(pedido: Pedido, status: StatusPedido): void {
    if (pedido.status === status) {
      return;
    }
    this.service.atualizarStatus(pedido.id, status).subscribe({
      next: () => {
        this.notificar(`Status alterado para ${status}.`);
        this.carregar();
      },
      error: (erro) => this.notificar(extrairMensagensErro(erro).join(' ')),
    });
  }

  private abrirFormulario(pedido?: Pedido): void {
    this.dialogAberto = true;
    const ref = this.dialog.open(PedidoFormDialogComponent, {
      width: '560px',
      data: { pedido, produtos: this.produtos },
    });

    ref.afterClosed().subscribe((salvo: boolean) => {
      this.dialogAberto = false;
      if (salvo) {
        this.notificar(pedido ? 'Pedido atualizado.' : 'Pedido criado.');
        this.carregar();
      }
    });
  }

  private carregarProdutos(): void {
    this.service.listarProdutos().subscribe({
      next: (produtos) => (this.produtos = produtos),
      error: () => this.notificar('Não foi possível carregar o catálogo de produtos.'),
    });
  }

  private notificar(mensagem: string): void {
    this.snackBar.open(mensagem, 'Fechar', { duration: 4000 });
  }
}
