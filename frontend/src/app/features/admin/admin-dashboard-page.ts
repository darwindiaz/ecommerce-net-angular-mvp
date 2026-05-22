import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { OrdersApiService } from '../../core/services/orders-api.service';
import { ProductsApiService } from '../../core/services/products-api.service';
import { ORDER_STATUSES, Order, OrderStatus } from '../../shared/models/order.models';
import {
  PRODUCT_COLORS,
  PRODUCT_SIZES,
  Product,
  ProductColor,
  ProductRequest,
  ProductSize,
} from '../../shared/models/product.models';

@Component({
  selector: 'app-admin-dashboard-page',
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <section class="page-header">
      <h1>Administración</h1>
      <button type="button" (click)="resetProductForm()">Nuevo producto</button>
    </section>

    <section class="admin-grid">
      <form [formGroup]="productForm" (ngSubmit)="saveProduct()" class="form-grid">
        <h2>{{ editingProductId() ? 'Editar producto' : 'Crear producto' }}</h2>
        <input placeholder="Código" formControlName="code" />
        <input placeholder="Imagen URL" formControlName="imageUrl" />
        <input placeholder="Nombre" formControlName="name" />
        <textarea placeholder="Descripción" formControlName="description"></textarea>
        <select formControlName="size">
          @for (size of sizes; track size) {
            <option [value]="size">{{ size }}</option>
          }
        </select>
        <select formControlName="color">
          @for (color of colors; track color) {
            <option [value]="color">{{ color }}</option>
          }
        </select>
        <input type="number" placeholder="Precio" formControlName="price" />
        <input type="number" placeholder="Stock" formControlName="stock" />
        <button type="submit">Guardar</button>
      </form>

      <section class="table-list">
        <h2>Productos</h2>
        @for (product of products(); track product.id) {
          <article class="table-row">
            <img [src]="product.imageUrl" [alt]="product.name" />
            <div>
              <strong>{{ product.name }}</strong>
              <p>{{ product.code }} · {{ product.price | currency: 'COP' : 'symbol-narrow' : '1.0-0' }}</p>
            </div>
            <button type="button" (click)="editProduct(product)">Editar</button>
            <button type="button" class="ghost" (click)="deleteProduct(product.id)">Eliminar</button>
          </article>
        }
      </section>
    </section>

    <section class="table-list">
      <h2>Gestión de órdenes</h2>
      @for (order of orders(); track order.id) {
        <article class="table-row order-admin">
          <div>
            <strong>{{ order.id }}</strong>
            <p>{{ order.total | currency: 'COP' : 'symbol-narrow' : '1.0-0' }}</p>
          </div>
          <select #status [value]="order.status">
            @for (state of statuses; track state) {
              <option [value]="state">{{ state }}</option>
            }
          </select>
          <button type="button" (click)="updateOrderStatus(order.id, status.value)">Actualizar</button>
          <button type="button" class="ghost" (click)="deleteOrder(order.id)">Eliminar</button>
        </article>
      }
    </section>
  `,
})
export class AdminDashboardPage {
  private readonly productsApi = inject(ProductsApiService);
  private readonly ordersApi = inject(OrdersApiService);
  private readonly formBuilder = inject(FormBuilder);

  readonly sizes = PRODUCT_SIZES;
  readonly colors = PRODUCT_COLORS;
  readonly statuses = ORDER_STATUSES;
  readonly products = signal<Product[]>([]);
  readonly orders = signal<Order[]>([]);
  readonly editingProductId = signal<string | null>(null);

  readonly productForm = this.formBuilder.nonNullable.group({
    code: ['', [Validators.required]],
    imageUrl: ['', [Validators.required]],
    name: ['', [Validators.required]],
    description: ['', [Validators.required]],
    size: [7, [Validators.required]],
    color: ['White', [Validators.required]],
    price: [0, [Validators.required, Validators.min(1)]],
    stock: [0, [Validators.required, Validators.min(0)]],
  });

  constructor() {
    this.loadProducts();
    this.loadOrders();
  }

  saveProduct(): void {
    if (this.productForm.invalid) {
      this.productForm.markAllAsTouched();
      return;
    }

    const request = this.toProductRequest();
    const editingId = this.editingProductId();
    const action = editingId
      ? this.productsApi.update(editingId, request)
      : this.productsApi.create(request);

    action.subscribe(() => {
      this.resetProductForm();
      this.loadProducts();
    });
  }

  editProduct(product: Product): void {
    this.editingProductId.set(product.id);
    this.productForm.setValue({
      code: product.code,
      imageUrl: product.imageUrl,
      name: product.name,
      description: product.description,
      size: product.size,
      color: product.color,
      price: product.price,
      stock: product.stock,
    });
  }

  deleteProduct(id: string): void {
    this.productsApi.delete(id).subscribe(() => this.loadProducts());
  }

  resetProductForm(): void {
    this.editingProductId.set(null);
    this.productForm.reset({
      code: '',
      imageUrl: '',
      name: '',
      description: '',
      size: 7,
      color: 'White',
      price: 0,
      stock: 0,
    });
  }

  updateOrderStatus(id: string, status: string): void {
    this.ordersApi.updateStatus(id, { status: status as OrderStatus }).subscribe(() => this.loadOrders());
  }

  deleteOrder(id: string): void {
    this.ordersApi.delete(id).subscribe(() => this.loadOrders());
  }

  private loadProducts(): void {
    this.productsApi.list().subscribe((products) => this.products.set(products));
  }

  private loadOrders(): void {
    this.ordersApi.list().subscribe((orders) => this.orders.set(orders));
  }

  private toProductRequest(): ProductRequest {
    const value = this.productForm.getRawValue();

    return {
      code: value.code,
      imageUrl: value.imageUrl,
      name: value.name,
      description: value.description,
      size: Number(value.size) as ProductSize,
      color: value.color as ProductColor,
      price: Number(value.price),
      stock: Number(value.stock),
    };
  }
}
