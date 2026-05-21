import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CartApiService } from '../../core/services/cart-api.service';
import { OrdersApiService } from '../../core/services/orders-api.service';
import { CartStore } from '../../core/store/cart.store';

@Component({
  selector: 'app-cart-page',
  imports: [CommonModule],
  template: `
    <section class="page-header">
      <h1>Carrito</h1>
      <strong>Total: {{ cartStore.total() | currency: 'COP' : 'symbol-narrow' : '1.0-0' }}</strong>
    </section>

    <section class="table-list">
      @for (item of cartStore.items(); track item.id) {
        <article class="table-row">
          <img [src]="item.imageUrl" [alt]="item.productName" />
          <div>
            <strong>{{ item.productName }}</strong>
            <p>{{ item.productCode }}</p>
          </div>
          <span>{{ item.unitPrice | currency: 'COP' : 'symbol-narrow' : '1.0-0' }}</span>
          <input #qty type="number" min="1" [value]="item.quantity" />
          <span>{{ item.subtotal | currency: 'COP' : 'symbol-narrow' : '1.0-0' }}</span>
          <button type="button" (click)="update(item.id, qty.value)">Actualizar</button>
          <button type="button" class="ghost" (click)="remove(item.id)">Eliminar</button>
        </article>
      } @empty {
        <p>Tu carrito está vacío.</p>
      }
    </section>

    <button type="button" [disabled]="cartStore.items().length === 0" (click)="checkout()">
      Finalizar compra
    </button>
  `,
})
export class CartPage {
  private readonly cartApi = inject(CartApiService);
  private readonly ordersApi = inject(OrdersApiService);
  private readonly router = inject(Router);
  readonly cartStore = inject(CartStore);

  constructor() {
    this.cartStore.load();
  }

  update(cartItemId: string, quantityValue: string): void {
    const quantity = Math.max(1, Number(quantityValue) || 1);
    this.cartApi.updateItem({ cartItemId, quantity }).subscribe((cart) => this.cartStore.set(cart));
  }

  remove(cartItemId: string): void {
    this.cartApi.deleteItem(cartItemId).subscribe(() => this.cartStore.load());
  }

  checkout(): void {
    this.ordersApi.checkout().subscribe(() => {
      this.cartStore.load();
      void this.router.navigateByUrl('/orders');
    });
  }
}
