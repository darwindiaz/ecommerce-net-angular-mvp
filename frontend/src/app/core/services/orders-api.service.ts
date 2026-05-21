import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Order, OrderStatus, UpdateOrderStatusRequest } from '../../shared/models/order.models';
import { API_BASE_URL } from './api.config';

@Injectable({ providedIn: 'root' })
export class OrdersApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  checkout(): Observable<Order> {
    return this.http.post<Order>(`${this.apiBaseUrl}/orders`, {});
  }

  list(status?: OrderStatus): Observable<Order[]> {
    const params = status ? new HttpParams().set('status', status) : undefined;
    return this.http.get<Order[]>(`${this.apiBaseUrl}/orders`, { params });
  }

  updateStatus(id: string, request: UpdateOrderStatusRequest): Observable<Order> {
    return this.http.put<Order>(`${this.apiBaseUrl}/orders/${id}/status`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}/orders/${id}`);
  }
}
