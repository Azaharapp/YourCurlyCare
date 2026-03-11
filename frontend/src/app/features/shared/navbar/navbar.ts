import { Component } from '@angular/core';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-navbar',
  imports: [RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})

export class Navbar {
  public mostrarOpcionesPerfil: boolean = false;      //false porque por defecto el menu de perfil no está desplegado

  desplegarPerfil() {
    this.mostrarOpcionesPerfil = !this.mostrarOpcionesPerfil;
  }

}
