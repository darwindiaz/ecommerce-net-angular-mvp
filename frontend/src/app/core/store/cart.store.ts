import { computed, inject, Injectable, signal } from '@angular/core';
import { Cart } from '../../shared/models/cart.models';
import { CartApiService } from '../services/cart-api.service';

@Injectable({ providedIn: 'root' })
export class CartStore {
  private readonly cartApi = inject(CartApiService);
  private readonly cart = signal<Cart | null>(null);

  readonly current = this.cart.asReadonly();
  readonly items = computed(() => this.cart()?.items ?? []);
  readonly total = computed(() => this.cart()?.total ?? 0);
  readonly count = computed(() => this.items().reduce((sum, item) => sum + item.quantity, 0));

  load(): void {
    this.cartApi.get().subscribe((cart) => this.cart.set(cart));
  }

  set(cart: Cart): void {
    this.cart.set(cart);
  }

  clear(): void {
    this.cart.set(null);
  }
}
