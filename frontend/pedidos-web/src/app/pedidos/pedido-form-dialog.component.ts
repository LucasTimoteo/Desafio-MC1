import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Pedido, PedidoRequest, Produto } from '../models/pedido.models';
import { PedidoService } from '../services/pedido.service';
import { extrairMensagensErro } from '../shared/erros';

export interface PedidoFormData {
  pedido?: Pedido;
  produtos: Produto[];
}

@Component({
  selector: 'app-pedido-form-dialog',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './pedido-form-dialog.component.html',
  styleUrl: './pedido-form-dialog.component.css',
})
export class PedidoFormDialogComponent {
  private readonly fb = inject(FormBuilder);
  private readonly service = inject(PedidoService);
  private readonly dialogRef = inject(MatDialogRef<PedidoFormDialogComponent>);
  readonly data = inject<PedidoFormData>(MAT_DIALOG_DATA);

  readonly produtos = this.data.produtos;
  readonly editando = !!this.data.pedido;
  readonly salvando = signal(false);
  readonly erros = signal<string[]>([]);

  readonly form = this.fb.group({
    nomeCliente: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(120)]],
    descricao: ['', [Validators.required, Validators.maxLength(500)]],
    itens: this.fb.array([] as ReturnType<PedidoFormDialogComponent['criarItemGroup']>[]),
  });

  readonly totalPreview = signal(0);

  constructor() {
    const pedido = this.data.pedido;
    if (pedido) {
      this.form.patchValue({ nomeCliente: pedido.nomeCliente, descricao: pedido.descricao });
      for (const item of pedido.itens) {
        this.itens.push(this.criarItemGroup(item.produtoId, item.quantidade));
      }
    } else {
      this.adicionarItem();
    }

    this.form.valueChanges.subscribe(() => this.recalcularTotal());
    this.recalcularTotal();
  }

  get itens(): FormArray {
    return this.form.get('itens') as FormArray;
  }

  criarItemGroup(produtoId = '', quantidade = 1) {
    return this.fb.group({
      produtoId: [produtoId, Validators.required],
      quantidade: [quantidade, [Validators.required, Validators.min(1), Validators.max(1000)]],
    });
  }

  adicionarItem(): void {
    this.itens.push(this.criarItemGroup());
  }

  removerItem(indice: number): void {
    this.itens.removeAt(indice);
  }

  private recalcularTotal(): void {
    let total = 0;
    for (const controle of this.itens.controls) {
      const produtoId = controle.get('produtoId')?.value as string;
      const quantidade = Number(controle.get('quantidade')?.value) || 0;
      const produto = this.produtos.find((p) => p.id === produtoId);
      if (produto) {
        total += produto.preco * quantidade;
      }
    }
    this.totalPreview.set(total);
  }

  salvar(): void {
    this.erros.set([]);

    if (this.itens.length === 0) {
      this.erros.set(['O pedido deve conter ao menos um item.']);
      return;
    }

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const valor = this.form.getRawValue();
    const request: PedidoRequest = {
      nomeCliente: valor.nomeCliente!.trim(),
      descricao: valor.descricao!.trim(),
      itens: this.itens.controls.map((c) => ({
        produtoId: c.get('produtoId')!.value as string,
        quantidade: Number(c.get('quantidade')!.value),
      })),
    };

    this.salvando.set(true);
    const requisicao = this.editando
      ? this.service.atualizarPedido(this.data.pedido!.id, request)
      : this.service.criarPedido(request);

    requisicao.subscribe({
      next: () => this.dialogRef.close(true),
      error: (erro) => {
        this.erros.set(extrairMensagensErro(erro));
        this.salvando.set(false);
      },
    });
  }
}
