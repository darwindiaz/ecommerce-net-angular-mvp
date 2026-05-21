import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthApiService } from '../../core/services/auth-api.service';
import { AuthStore } from '../../core/store/auth.store';

@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule, RouterLink],
  template: `
    <section class="auth-panel">
      <h1>Iniciar sesión</h1>
      <form [formGroup]="form" (ngSubmit)="submit()" class="form-grid">
        <label>
          Email
          <input type="email" formControlName="email" />
        </label>
        <label>
          Contraseña
          <input type="password" formControlName="password" />
        </label>
        @if (error()) {
          <p class="error">{{ error() }}</p>
        }
        <button type="submit">Entrar</button>
      </form>
      <a routerLink="/register">Crear cuenta</a>
    </section>
  `,
})
export class LoginPage {
  private readonly authApi = inject(AuthApiService);
  private readonly authStore = inject(AuthStore);
  private readonly formBuilder = inject(FormBuilder);
  private readonly router = inject(Router);

  readonly error = signal('');
  readonly form = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.error.set('');
    this.authApi.login(this.form.getRawValue()).subscribe({
      next: (session) => {
        this.authStore.setSession(session);
        void this.router.navigateByUrl('/products');
      },
      error: () => this.error.set('Credenciales inválidas.'),
    });
  }
}
