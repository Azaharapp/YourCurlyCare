import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { UsuarioLogin, UsuarioRegistro } from '../models/usuariosi';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http: HttpClient = inject(HttpClient);
  private router = inject(Router);

  private apiUrl: string = "http://localhost:8000/api/usuarios";            
  //private apiUrl: string = "http://127.0.0.1:5216/api/usuarios"

  public usuarioActual = signal<string | null>(localStorage.getItem('usuarioNombre') || sessionStorage.getItem('usuarioNombre')); public userRol = signal<string | null>(localStorage.getItem('rol') || sessionStorage.getItem('rol'));
  public userId = signal<string | null>(localStorage.getItem('usuarioId') || sessionStorage.getItem('usuarioId'));

  guardarSesion(res: any, recordar: boolean): void {
    const storage = recordar ? localStorage : sessionStorage;
    const storage2 = recordar ? sessionStorage : localStorage;

    storage2.clear();

    storage.setItem('token', res.token);
    storage.setItem('usuarioNombre', res.username);
    storage.setItem('usuarioId', res.userId.toString());
    storage.setItem('rol', res.rol);

    this.usuarioActual.set(res.username);
    this.userRol.set(res.rol);
    this.userId.set(res.userId.toString());
  }

  cerrarSesion(): void {
    this.usuarioActual.set(null);
    this.userRol.set(null);
    this.userId.set(null);

    localStorage.clear();
    sessionStorage.clear();

    this.router.navigate(['/login']);
  }

  isAdmin(): boolean {
    return this.userRol()?.toLowerCase() === 'admin';
  }

  isLogguedIn(): boolean {
    const token = localStorage.getItem('token') || sessionStorage.getItem('token');
    const id = localStorage.getItem('usuarioId') || sessionStorage.getItem('usuarioId');

    return token !== null && id !== null;
  }

  registrar(datosUsuario: UsuarioRegistro): Observable<any> {
    return this.http.post(this.apiUrl, datosUsuario);
  }

  login(usuarioExistente: UsuarioLogin): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, usuarioExistente, {
      withCredentials: true
    });
  }

  recuperarPassword(email: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/olvidar-pass`, { email });
  }

  resetearPassword(datos: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/reset-pass`, datos);
  }

  solicitarEliminacion(): Observable<any> {
    const token = localStorage.getItem('token') || sessionStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.post(`${this.apiUrl}/solicitar-eliminacion`, {}, {
      headers: headers,
      withCredentials: true
    });
  }

  confirmarEliminacion(codigo: string): Observable<any> {
    const token = localStorage.getItem('token') || sessionStorage.getItem('token');

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.http.post(`${this.apiUrl}/confirmar-eliminacion`, JSON.stringify(codigo), {
      headers: headers,
      withCredentials: true
    });
  }
}
