export type UserRole = 'Admin' | 'Customer';

export interface AuthResponse {
  userId: string;
  email: string;
  role: UserRole;
  token: string;
  expiresAt: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  names: string;
  lastNames: string;
  age: number;
  birthDate: string;
  country: string;
  department: string;
  city: string;
  phone: string;
  address: string;
  email: string;
  password: string;
}
