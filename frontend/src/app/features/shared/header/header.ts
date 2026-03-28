import { Component, inject } from '@angular/core';
import { AuthService } from '../../web/services/auth-service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-header',
  imports: [RouterModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  public authService: AuthService = inject(AuthService);
  public mostrarOpcionesPerfil: boolean = false;              //false porque por defecto el menu de perfil no está desplegado

  desplegarPerfil() {
    this.mostrarOpcionesPerfil = !this.mostrarOpcionesPerfil;
  }
}
