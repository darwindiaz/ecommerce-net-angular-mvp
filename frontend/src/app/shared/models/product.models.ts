export type ProductSize = 7 | 8 | 9 | 10;
export type ProductSizeName = 'Seven' | 'Eight' | 'Nine' | 'Ten';
export type ProductColor = 'White' | 'Black' | 'Gray';

export const PRODUCT_SIZES: readonly ProductSize[] = [7, 8, 9, 10];
export const PRODUCT_COLORS: readonly ProductColor[] = ['White', 'Black', 'Gray'];

const PRODUCT_COLOR_LABELS: Record<ProductColor, string> = {
  White: 'Blanco',
  Black: 'Negro',
  Gray: 'Gris',
};

const PRODUCT_SIZE_LABELS: Record<string, string> = {
  7: '7',
  8: '8',
  9: '9',
  10: '10',
  Seven: '7',
  Eight: '8',
  Nine: '9',
  Ten: '10',
};

const PRODUCT_SIZE_VALUES: Record<string, ProductSize> = {
  7: 7,
  8: 8,
  9: 9,
  10: 10,
  Seven: 7,
  Eight: 8,
  Nine: 9,
  Ten: 10,
};

export function formatProductSize(size: ProductSize | ProductSizeName): string {
  return PRODUCT_SIZE_LABELS[String(size)] ?? String(size);
}

export function toProductSizeValue(size: ProductSize | ProductSizeName): ProductSize {
  return PRODUCT_SIZE_VALUES[String(size)] ?? 7;
}

export function formatProductColor(color: ProductColor): string {
  return PRODUCT_COLOR_LABELS[color] ?? color;
}

export interface Product {
  id: string;
  code: string;
  imageUrl: string;
  name: string;
  description: string;
  size: ProductSize | ProductSizeName;
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
