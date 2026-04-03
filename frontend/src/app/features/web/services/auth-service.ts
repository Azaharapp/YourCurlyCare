import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { UsuarioLogin, UsuarioRegistro } from '../models/usuariosi';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http: HttpClient = inject(HttpClient);
private apiUrl: string = "http://localhost:8000/api/usuarios";            //"http://localhost:5216/api/usuarios"
 // private apiUrl: string = "http://localhost:5216/api/usuarios"

  public usuarioActual = signal<string | null>(localStorage.getItem('usuarioNombre'));

  guardarUsuarioActivo(nombre: string) :void{
    this.usuarioActual.set(nombre);
    localStorage.setItem('usuarioNombre', nombre); 
  }

  cerrarSesion():void {
    this.usuarioActual.set(null);
    localStorage.removeItem('usuarioNombre');
    localStorage.removeItem('token');
    localStorage.removeItem('usuarioId');
  }

  registrar(datosUsuario: UsuarioRegistro): Observable<any> {
    return this.http.post(this.apiUrl, datosUsuario);
  }

  login(usuarioExistente: UsuarioLogin): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, usuarioExistente);
  }

  isLogguedIn(): boolean {
    const token = localStorage.getItem('token');

   // return token !== null && token !== '' ? true : false;
   return token !== null && token !== '' && token !== 'null';
  }


}
