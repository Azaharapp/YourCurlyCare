import { Component, inject } from '@angular/core';
import { NgForm, FormsModule, NgModel } from '@angular/forms';
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

  public recordarSesion: boolean = false;

  iniciarSesion(loginForm: NgForm) {
    if (loginForm.valid) {
      const usuarioExistente = {
        email: loginForm.value.email,
        password: loginForm.value.password,
        recordar: this.recordarSesion
      };

      this.authService.login(usuarioExistente).subscribe({
        next: (respuesta) => {
          //this.authService.guardarSesion(respuesta, usuarioExistente.recordar);
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.error('Error en el login:', err.error );
        }
      });
    } else console.log("Faltan campos por rellenar.");
  }
}
