import { Component, inject } from '@angular/core';
import { NgForm, FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth-service';

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private router = inject(Router);
  private authService: AuthService = inject(AuthService);

  iniciarSesion(loginForm: NgForm) {
    if (loginForm.valid) {
      const usuarioExistente = {
        email: loginForm.value.email,
        password: loginForm.value.password
      };

      this.authService.login(usuarioExistente).subscribe({
        next: (respuesta) => {
          if (respuesta.token)
            localStorage.setItem('token', respuesta.token);

          this.authService.guardarUsuarioActivo(respuesta.username);
          this.router.navigate(['/']);
        },

        error: (errorHttp) => {
          alert(errorHttp.error || "Se ha producido un error al intentar iniciar sesión");
        }
      });

    } else console.log("Faltan campos por rellenar.");
  }
}
