import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CartApiService } from '../../core/services/cart-api.service';
import { ProductsApiService } from '../../core/services/products-api.service';
import { AuthStore } from '../../core/store/auth.store';
import { CartStore } from '../../core/store/cart.store';
import {
  PRODUCT_COLORS,
  PRODUCT_SIZES,
  Product,
  ProductColor,
  ProductFilters,
  ProductSize,
} from '../../shared/models/product.models';

@Component({
  selector: 'app-products-page',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <section class="page-header">
      <h1>Catálogo</h1>
      <form [formGroup]="filtersForm" (ngSubmit)="load()" class="toolbar">
        <input placeholder="Nombre" formControlName="name" />
        <input placeholder="Descripción" formControlName="description" />
        <input placeholder="Código" formControlName="code" />
        <select formControlName="size">
          <option value="">Talla</option>
          @for (size of sizes; track size) {
            <option [value]="size">{{ size }}</option>
          }
        </select>
        <select formControlName="color">
          <option value="">Color</option>
          @for (color of colors; track color) {
            <option [value]="color">{{ color }}</option>
          }
        </select>
        <button type="submit">Filtrar</button>
      </form>
    </section>

    @if (error()) {
      <p class="error">{{ error() }}</p>
    }

    <section class="product-grid">
      @for (product of products(); track product.id) {
        <article class="product-card">
          <img [src]="product.imageUrl" [alt]="product.name" />
          <div>
            <a [routerLink]="['/products', product.id]">{{ product.name }}</a>
            <p>{{ product.code }} · Talla {{ product.size }} · {{ product.color }}</p>
            <strong>{{ product.price | currency: 'COP' : 'symbol-narrow' : '1.0-0' }}</strong>
            <span [class.available]="product.isAvailable">
              {{ product.isAvailable ? 'Disponible' : 'Agotado' }}
            </span>
          </div>
          <div class="row-actions">
            <input #qty type="number" min="1" [max]="product.stock" value="1" />
            <button type="button" [disabled]="!product.isAvailable" (click)="addToCart(product, qty.value)">
              Agregar
            </button>
          </div>
        </article>
      } @empty {
        <p>No hay productos con esos filtros.</p>
      }
    </section>
  `,
})
export class ProductsPage {
  private readonly productsApi = inject(ProductsApiService);
  private readonly cartApi = inject(CartApiService);
  private readonly cartStore = inject(CartStore);
  private readonly authStore = inject(AuthStore);
  private readonly formBuilder = inject(FormBuilder);
  private readonly router = inject(Router);

  readonly sizes = PRODUCT_SIZES;
  readonly colors = PRODUCT_COLORS;
  readonly products = signal<Product[]>([]);
  readonly error = signal('');
  readonly filtersForm = this.formBuilder.nonNullable.group({
    name: [''],
    description: [''],
    code: [''],
    size: [''],
    color: [''],
  });

  constructor() {
    this.load();
  }

  load(): void {
    this.error.set('');
    const raw = this.filtersForm.getRawValue();
    const filters: ProductFilters = {
      name: raw.name,
      description: raw.description,
      code: raw.code,
      size: raw.size ? (Number(raw.size) as ProductSize) : undefined,
      color: raw.color ? (raw.color as ProductColor) : undefined,
    };

    this.productsApi.list(filters).subscribe({
      next: (products) => this.products.set(products),
      error: () => {
        this.products.set([]);
        this.error.set('No fue posible cargar los productos.');
      },
    });
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
