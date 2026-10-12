import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { PedidosListComponent } from './pedidos/pedidos-list.component';

@Component({
  selector: 'app-root',
  imports: [MatToolbarModule, MatIconModule, PedidosListComponent],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {}
