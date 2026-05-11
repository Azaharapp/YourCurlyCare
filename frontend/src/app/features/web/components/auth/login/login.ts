import { ChangeDetectorRef, Component, inject } from '@angular/core';
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
  private cd = inject(ChangeDetectorRef);

  public recordarSesion: boolean = false;
  public mensajeError: string | null = null;

  iniciarSesion(loginForm: NgForm) {
    this.mensajeError = null;

    if (loginForm.valid) {
      const usuarioExistente = {
        email: loginForm.value.email,
        password: loginForm.value.password,
        recordar: this.recordarSesion
      };

      this.authService.login(usuarioExistente).subscribe({
        next: (respuesta) => {
          this.authService.guardarSesion(respuesta, usuarioExistente.recordar);
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.mensajeError = typeof err.error === 'string'
            ? err.error
            : "Email o contraseña incorrectos";

          this.cd.detectChanges();
          setTimeout(() => {
            this.mensajeError = null;
            this.cd.detectChanges();
          }, 5000);
        }
      });
    } else {
      console.log("Faltan campos por rellenar.");

      this.cd.detectChanges();
    }
  }
}
