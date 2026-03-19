import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { UsuarioLogin, UsuarioRegistro } from '../models/usuariosi';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = "http://localhost:5216/api/usuarios"

  usuarioActual = signal<string | null>(localStorage.getItem('usuarioNombre'));

  guardarUsuarioActivo(nombre: string) {
    this.usuarioActual.set(nombre);
    localStorage.setItem('usuarioNombre', nombre); 
  }

  cerrarSesion() {
    this.usuarioActual.set(null);
    localStorage.removeItem('usuarioNombre');
    localStorage.removeItem('token');
  }
  registrar(datosUsuario: UsuarioRegistro): Observable<any> {
    return this.http.post(this.apiUrl, datosUsuario);
  }

  login(usuarioExistente: UsuarioLogin): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, usuarioExistente);
  }
}
