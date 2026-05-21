import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { OrdersApiService } from '../../core/services/orders-api.service';
import { Order, OrderStatus } from '../../shared/models/order.models';

@Component({
  selector: 'app-orders-page',
  imports: [CommonModule],
  template: `
    <section class="page-header">
      <h1>Órdenes</h1>
      <select #status (change)="load(status.value)">
        <option value="">Todos</option>
        @for (state of statuses; track state) {
          <option [value]="state">{{ state }}</option>
        }
      </select>
    </section>

    <section class="table-list">
      @for (order of orders(); track order.id) {
        <article class="order-card">
          <div class="page-header compact">
            <strong>{{ order.id }}</strong>
            <span>{{ order.status }}</span>
          </div>
          <p>{{ order.createdAt | date: 'medium' }} · {{ order.total | currency: 'COP' : 'symbol-narrow' : '1.0-0' }}</p>
          @for (item of order.items; track item.id) {
            <p>{{ item.productName }} x {{ item.quantity }} · {{ item.subtotal | currency: 'COP' : 'symbol-narrow' : '1.0-0' }}</p>
          }
        </article>
      } @empty {
        <p>No hay órdenes registradas.</p>
      }
    </section>
  `,
})
export class OrdersPage {
  private readonly ordersApi = inject(OrdersApiService);
  readonly statuses: OrderStatus[] = ['InProcess', 'Paid', 'Shipped', 'Delivered'];
  readonly orders = signal<Order[]>([]);

  constructor() {
    this.load('');
  }

  load(status: string): void {
    this.ordersApi.list((status || undefined) as OrderStatus | undefined).subscribe((orders) => {
      this.orders.set(orders);
    });
  }
}
