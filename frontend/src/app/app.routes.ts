import { Routes } from '@angular/router';
import { adminGuard } from './core/guards/admin.guard';
import { authGuard } from './core/guards/auth.guard';
import { AdminDashboardPage } from './features/admin/admin-dashboard-page';
import { LoginPage } from './features/auth/login-page';
import { RegisterPage } from './features/auth/register-page';
import { CartPage } from './features/cart/cart-page';
import { OrdersPage } from './features/orders/orders-page';
import { ProductDetailPage } from './features/products/product-detail-page';
import { ProductsPage } from './features/products/products-page';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'products' },
  { path: 'login', component: LoginPage },
  { path: 'register', component: RegisterPage },
  { path: 'products', component: ProductsPage },
  { path: 'products/:id', component: ProductDetailPage },
  { path: 'cart', component: CartPage, canActivate: [authGuard] },
  { path: 'orders', component: OrdersPage, canActivate: [authGuard] },
  { path: 'admin', component: AdminDashboardPage, canActivate: [authGuard, adminGuard] },
  { path: '**', redirectTo: 'products' },
];
