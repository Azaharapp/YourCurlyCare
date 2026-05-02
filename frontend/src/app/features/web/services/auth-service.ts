import { HttpClient } from '@angular/common/http';
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

  public usuarioActual = signal<string | null>(localStorage.getItem('usuarioNombre') || sessionStorage.getItem('usuarioNombre'));

  guardarSesion(res: any, recordar: boolean): void {
    const storage = recordar ? localStorage : sessionStorage;

      storage.setItem('token', res.token);
      storage.setItem('usuarioNombre', res.username);
      storage.setItem('usuarioId', res.userId.toString());

      this.usuarioActual.set(res.username);
  }

  cerrarSesion(): void {
    this.usuarioActual.set(null);

    localStorage.removeItem('usuarioNombre');
    localStorage.removeItem('token');
    localStorage.removeItem('usuarioId');

    this.router.navigate(['/login']);
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
    }).pipe(
      tap((res: any) => this.guardarSesion(res, usuarioExistente.recordar))
    );
  }

  recuperarPassword(email: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/olvidar-pass`, { email });
  }

  resetearPassword(datos: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/reset-pass`, datos);
  }
}
