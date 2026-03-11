import { Component, inject } from '@angular/core';
import { NgForm, FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private router = inject(Router);

  iniciarSesion(loginForm: NgForm) {
    if(loginForm.valid){
      console.log("Se ha iniciado sesión correctamente.");
      console.log('Datos recogidos:', loginForm.value);
      this.router.navigate(['/']);
    } else console.log("Faltan campos por rellenar.");  
  }

}
