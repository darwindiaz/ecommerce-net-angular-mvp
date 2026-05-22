export type ProductSize = 7 | 8 | 9 | 10;
export type ProductColor = 'White' | 'Black' | 'Gray';

export const PRODUCT_SIZES: readonly ProductSize[] = [7, 8, 9, 10];
export const PRODUCT_COLORS: readonly ProductColor[] = ['White', 'Black', 'Gray'];

export interface Product {
  id: string;
  code: string;
  imageUrl: string;
  name: string;
  description: string;
  size: ProductSize;
  color: ProductColor;
  price: number;
  stock: number;
  isAvailable: boolean;
}

export interface ProductFilters {
  name?: string;
  description?: string;
  code?: string;
  size?: ProductSize;
  color?: ProductColor;
}

export interface ProductRequest {
  code: string;
  imageUrl: string;
  name: string;
  description: string;
  size: ProductSize;
  color: ProductColor;
  price: number;
  stock: number;
}
