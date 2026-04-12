import { Component, inject, ChangeDetectorRef } from '@angular/core';
import { AuthService } from '../../../services/auth-service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-recuperar-pass',
  imports: [FormsModule, CommonModule],
  templateUrl: './recuperar-pass.html',
  styleUrl: './recuperar-pass.css',
})
export class RecuperarPass {
  private authService: AuthService = inject(AuthService);
  private router : Router = inject(Router);
  private cdr = inject(ChangeDetectorRef);                    //actualiza la interfaz de usuario cuando ocurre algun cambio

  public email: string = '';
  public codigo: string = '';
  public nuevaPass: string = '';
  public confirmarPass: string = '';

  public paso: number = 1; 

  solicitarCodigo() {
    if (!this.email) return alert("Introduce tu email");

    this.authService.recuperarPassword(this.email).subscribe({
      next: () => {
        this.paso = 2; 
        this.cdr.detectChanges();                           //evita quedarse en el paso 1 y cambia al paso 2
      },
      error: (err) => alert("Error: " + (err.error?.detalle || "No se pudo enviar el correo"))
    });
  }

  cambiarPassword() {
    if (this.nuevaPass !== this.confirmarPass) return alert("Las contraseñas no coinciden");
    
    const datos = {                                         //viene de ResetPasswordRequest.cs              
      email: this.email,
      codigo: this.codigo,
      nuevaPassword: this.nuevaPass
    };

    this.authService.resetearPassword(datos).subscribe({
      next: () => {
        alert("Contraseña actualizada.");
        this.router.navigate(['/login']);
      },
      error: (err) => alert(err.error || "Error al cambiar la contraseña")
    });
  }
}
