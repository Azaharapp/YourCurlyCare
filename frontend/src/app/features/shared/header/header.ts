import { Component, inject } from '@angular/core';
import { AuthService } from '../../web/services/auth-service';
import { RouterModule } from '@angular/router';
import { ProductosService } from '../../web/services/productos-service';
import { ProductoEscanerI } from '../../web/models/productoEscaneri';

@Component({
  selector: 'app-header',
  imports: [RouterModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  public authService: AuthService = inject(AuthService);
  public productosService : ProductosService = inject(ProductosService);
 
  public resultados: ProductoEscanerI[] = [];

  public productos = this.productosService.getProducto;
  public mostrarOpcionesPerfil: boolean = false;              //false porque por defecto el menu de perfil no está desplegado
  public buscando: boolean = false;

  desplegarPerfil() {
    this.mostrarOpcionesPerfil = !this.mostrarOpcionesPerfil;
  }

  onBuscar(busqueda: string) {
    this.buscando = busqueda.length > 2;

    if (this.buscando) {
      this.productosService.buscarProductos(busqueda).subscribe({
        next: (res) => this.resultados = res, 
        error: () => this.resultados = []
      });
    } else {
      this.resultados = [];
    }
  }

  salir() {
    this.authService.cerrarSesion();
    this.mostrarOpcionesPerfil = false; 
  }
}
