import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { inject } from '@angular/core';
import { AuthStore } from './core/store/auth.store';
import { CartStore } from './core/store/cart.store';

@Component({
  selector: 'app-root',
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  readonly authStore = inject(AuthStore);
  readonly cartStore = inject(CartStore);
  private readonly router = inject(Router);

  logout(): void {
    this.authStore.clear();
    this.cartStore.clear();
    void this.router.navigateByUrl('/login');
  }
}
