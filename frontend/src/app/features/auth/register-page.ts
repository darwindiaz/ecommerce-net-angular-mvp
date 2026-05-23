import { Component, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthApiService } from '../../core/services/auth-api.service';
import { AuthStore } from '../../core/store/auth.store';

@Component({
  selector: 'app-register-page',
  imports: [ReactiveFormsModule, RouterLink],
  template: `
    <section class="auth-panel wide">
      <h1>Registro</h1>
      <form [formGroup]="form" (ngSubmit)="submit()" class="form-grid two-columns">
        <label>Nombres <input formControlName="names" /></label>
        <label>Apellidos <input formControlName="lastNames" /></label>
        <label>Fecha nacimiento <input type="date" formControlName="birthDate" /></label>
        <label>Edad <input type="number" formControlName="age" readonly /></label>
        <label>País <input formControlName="country" /></label>
        <label>Departamento <input formControlName="department" /></label>
        <label>Ciudad <input formControlName="city" /></label>
        <label>Celular <input formControlName="phone" /></label>
        <label class="full">Dirección <input formControlName="address" /></label>
        <label>Email <input type="email" formControlName="email" /></label>
        <label>Contraseña <input type="password" formControlName="password" /></label>
        @if (error()) {
          <p class="error full">{{ error() }}</p>
        }
        <button type="submit">Crear cuenta</button>
      </form>
      <a routerLink="/login">Ya tengo cuenta</a>
    </section>
  `,
})
export class RegisterPage {
  private readonly authApi = inject(AuthApiService);
  private readonly authStore = inject(AuthStore);
  private readonly formBuilder = inject(FormBuilder);
  private readonly router = inject(Router);

  readonly error = signal('');
  readonly form = this.formBuilder.nonNullable.group({
    names: ['', [Validators.required]],
    lastNames: ['', [Validators.required]],
    age: [18, [Validators.required, Validators.min(1)]],
    birthDate: ['', [Validators.required]],
    country: ['', [Validators.required]],
    department: ['', [Validators.required]],
    city: ['', [Validators.required]],
    phone: ['', [Validators.required]],
    address: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
  });

  constructor() {
    this.form.controls.birthDate.valueChanges.pipe(takeUntilDestroyed()).subscribe((birthDate) => {
      this.form.controls.age.setValue(this.calculateAge(birthDate), { emitEvent: false });
    });
  }

  submit(): void {
    this.form.controls.age.setValue(this.calculateAge(this.form.controls.birthDate.value), {
      emitEvent: false,
    });

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.error.set('');
    this.authApi.register(this.form.getRawValue()).subscribe({
      next: (session) => {
        this.authStore.setSession(session);
        void this.router.navigateByUrl('/products');
      },
      error: () => this.error.set('No fue posible registrar el usuario.'),
    });
  }

  private calculateAge(birthDateValue: string): number {
    if (!birthDateValue) {
      return 0;
    }

    const birthDate = new Date(`${birthDateValue}T00:00:00`);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const hasBirthdayPassed =
      today.getMonth() > birthDate.getMonth() ||
      (today.getMonth() === birthDate.getMonth() && today.getDate() >= birthDate.getDate());

    if (!hasBirthdayPassed) {
      age -= 1;
    }

    return Math.max(0, age);
  }
}
