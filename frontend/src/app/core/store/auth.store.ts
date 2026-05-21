import { computed, Injectable, signal } from '@angular/core';
import { AuthResponse } from '../../shared/models/auth.models';

const storageKey = 'ecommerce.auth';

@Injectable({ providedIn: 'root' })
export class AuthStore {
  private readonly session = signal<AuthResponse | null>(this.loadSession());

  readonly user = this.session.asReadonly();
  readonly token = computed(() => this.session()?.token ?? null);
  readonly isAuthenticated = computed(() => Boolean(this.session()?.token));
  readonly isAdmin = computed(() => this.session()?.role === 'Admin');

  setSession(session: AuthResponse): void {
    this.session.set(session);
    localStorage.setItem(storageKey, JSON.stringify(session));
  }

  clear(): void {
    this.session.set(null);
    localStorage.removeItem(storageKey);
  }

  private loadSession(): AuthResponse | null {
    const rawSession = localStorage.getItem(storageKey);

    if (!rawSession) {
      return null;
    }

    try {
      return JSON.parse(rawSession) as AuthResponse;
    } catch {
      localStorage.removeItem(storageKey);
      return null;
    }
  }
}
