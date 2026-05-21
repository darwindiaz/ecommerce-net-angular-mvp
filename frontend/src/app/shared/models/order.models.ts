export type OrderStatus = 'InProcess' | 'Paid' | 'Shipped' | 'Delivered';

export interface OrderItem {
  id: string;
  productId: string;
  productCode: string;
  productName: string;
  unitPrice: number;
  quantity: number;
  subtotal: number;
}

export interface Order {
  id: string;
  userId: string;
  total: number;
  status: OrderStatus;
  createdAt: string;
  items: OrderItem[];
}

export interface UpdateOrderStatusRequest {
  status: OrderStatus;
}
