import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-validacion',
  imports: [FormsModule],
  templateUrl: './validacion.html',
  styleUrl: './validacion.css',
})
export class Validacion {
  public codigo: string = '';
  public mensajeError: string = '';

  private http: HttpClient = inject(HttpClient);
  private router: Router = inject(Router);

  enviarCodigo() {
    const body = { codigo: this.codigo };

    //this.http.post('http://127.0.0.1:5216/api/Usuarios/confirmar', body, {
    this.http.post('http://localhost:8000/api/Usuarios/confirmar', body, {
      headers: { 'Content-Type': 'application/json' }
    }).subscribe({
      next: () => {
        alert("¡Cuenta activada! Ya puedes iniciar sesión.");
        this.router.navigate(['/login']);
      },
      error: () => {
        this.mensajeError = "Código incorrecto. Inténtalo de nuevo.";
      }
    });
  }
}
