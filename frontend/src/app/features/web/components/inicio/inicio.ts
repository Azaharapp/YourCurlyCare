import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth-service';
import { Usuarios } from '../admin/usuarios/usuarios';
import { Productos } from '../admin/productos/productos';

@Component({
  selector: 'app-inicio',
  imports: [RouterModule, Usuarios, Productos],
  templateUrl: './inicio.html',
  styleUrl: './inicio.css',
})
export class Inicio {
 public authService: AuthService = inject(AuthService);       //se injecta para usarlo a la hora de los roles

}
