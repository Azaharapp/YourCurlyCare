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
  public productosService: ProductosService = inject(ProductosService);

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

  eliminarCuenta() {
    const confirmar = confirm("¿Estás seguro de que deseas eliminar tu cuenta?");

    if (confirmar) {
      this.authService.solicitarEliminacion().subscribe({
        next: (res: any) => {
          this.mostrarOpcionesPerfil = false;

          const codigo = prompt("Se ha enviado un código a tu email. Introduce el código aquí para confirmar la baja:");

          if (codigo) {
            this.authService.confirmarEliminacion(codigo).subscribe({
              next: () => {
                alert("Cuenta desactivada correctamente.");
                this.salir();
              },
              error: (err) => alert("El código no es correcto.")
            });
          }
        },
        error: (err) => {
          console.error("Error real:", err);
          alert("Hubo un error al procesar la solicitud de baja.");
        }
      });
    }
  }
}
