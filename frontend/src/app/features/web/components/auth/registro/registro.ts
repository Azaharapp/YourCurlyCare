import { NgForm, FormsModule } from '@angular/forms';
import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth-service';

@Component({
  selector: 'app-registro',
  imports: [FormsModule, RouterLink],
  templateUrl: './registro.html',
  styleUrl: './registro.css',
})
export class Registro {
  private router = inject(Router);
  private authService: AuthService = inject(AuthService);

  registrarse(registroForm: NgForm) {
    if (registroForm.valid) {
      const nuevoUsuario = {
        nombre: registroForm.value.name,
        apellido: registroForm.value.surname,
        username: registroForm.value.username,
        email: registroForm.value.email,
        password: registroForm.value.password
      };

      this.authService.registrar(nuevoUsuario).subscribe({

        next: (respuesta) => {
          this.authService.guardarUsuarioActivo(nuevoUsuario.username);

          if (respuesta && respuesta.id) 
            localStorage.setItem('usuarioId', respuesta.id.toString());
          

          this.router.navigate(['/']);
        },

        error: (errorHttp) => {
          alert(errorHttp.error || "Se ha producido un error al realizar el registro.");
        }
      });

    } else {
      alert("Por favor, rellena todos los campos obligatorios.");
    }
  }

}
