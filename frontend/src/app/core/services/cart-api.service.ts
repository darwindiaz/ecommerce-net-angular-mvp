import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddCartItemRequest, Cart, UpdateCartItemRequest } from '../../shared/models/cart.models';
import { API_BASE_URL } from './api.config';

@Injectable({ providedIn: 'root' })
export class CartApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  get(): Observable<Cart> {
    return this.http.get<Cart>(`${this.apiBaseUrl}/cart`);
  }

  addItem(request: AddCartItemRequest): Observable<Cart> {
    return this.http.post<Cart>(`${this.apiBaseUrl}/cart/items`, request);
  }

  updateItem(request: UpdateCartItemRequest): Observable<Cart> {
    return this.http.put<Cart>(`${this.apiBaseUrl}/cart/items`, request);
  }

  deleteItem(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}/cart/items/${id}`);
  }
}
