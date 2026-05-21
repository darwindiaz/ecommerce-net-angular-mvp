import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CartApiService } from '../../core/services/cart-api.service';
import { ProductsApiService } from '../../core/services/products-api.service';
import { AuthStore } from '../../core/store/auth.store';
import { CartStore } from '../../core/store/cart.store';
import { Product } from '../../shared/models/product.models';

@Component({
  selector: 'app-product-detail-page',
  imports: [CommonModule],
  template: `
    @if (product(); as product) {
      <section class="detail-layout">
        <img [src]="product.imageUrl" [alt]="product.name" />
        <div>
          <h1>{{ product.name }}</h1>
          <p>{{ product.description }}</p>
          <dl>
            <dt>Código</dt><dd>{{ product.code }}</dd>
            <dt>Talla</dt><dd>{{ product.size }}</dd>
            <dt>Color</dt><dd>{{ product.color }}</dd>
            <dt>Stock</dt><dd>{{ product.stock }}</dd>
            <dt>Precio</dt><dd>{{ product.price | currency: 'COP' : 'symbol-narrow' : '1.0-0' }}</dd>
          </dl>
          <div class="row-actions">
            <input #qty type="number" min="1" [max]="product.stock" value="1" />
            <button type="button" [disabled]="!product.isAvailable" (click)="addToCart(product, qty.value)">
              Agregar al carrito
            </button>
          </div>
        </div>
      </section>
    }
  `,
})
export class ProductDetailPage {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly authStore = inject(AuthStore);
  private readonly productsApi = inject(ProductsApiService);
  private readonly cartApi = inject(CartApiService);
  private readonly cartStore = inject(CartStore);

  readonly product = signal<Product | null>(null);

  constructor() {
    const productId = this.route.snapshot.paramMap.get('id');
    if (productId) {
      this.productsApi.getById(productId).subscribe((product) => this.product.set(product));
    }
  }

  addToCart(product: Product, quantityValue: string): void {
    if (!this.authStore.isAuthenticated()) {
      void this.router.navigateByUrl('/login');
      return;
    }

    const quantity = Math.max(1, Number(quantityValue) || 1);
    this.cartApi.addItem({ productId: product.id, quantity }).subscribe((cart) => this.cartStore.set(cart));
  }
}
