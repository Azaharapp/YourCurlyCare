import { NgForm, FormsModule } from '@angular/forms';
import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-registro',
  imports: [FormsModule, RouterLink],
  templateUrl: './registro.html',
  styleUrl: './registro.css',
})
export class Registro {
  private router = inject(Router);

  registrarse(registroForm: NgForm) {
    if(registroForm.valid){
      console.log("Se ha registrado correctamente al usuario.");
      console.log('Datos recogidos:', registroForm.value);
      this.router.navigate(['/']);
    } else console.log("Faltan campos por rellenar.");  
  }

}
