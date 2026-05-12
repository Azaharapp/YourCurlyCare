import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
FormsModule
@Component({
  selector: 'app-productos',
  imports: [CommonModule, FormsModule],
  templateUrl: './productos.html',
  styleUrl: './productos.css',
})
export class Productos {
  private http = inject(HttpClient);
  private cd = inject(ChangeDetectorRef);

  public esAdmin: boolean = localStorage.getItem('rol') === 'Admin';
  
  private url = 'http://localhost:8000/api/ProductoEscaners/admin';
  //private url = 'http://127.0.0.1:5216/api/ProductoEscaners/admin';

  public productos: any[] = [];
  public productoEditando: any = null;
  public mensajeAviso: string | null = null;

  constructor() {
    this.cargarProductos();
  }

  abrirEdicion(producto: any) {
    this.productoEditando = { ...producto };
  }

  guardarEdicion() {
    if (!this.productoEditando) return;

    const nombreCopia = this.productoEditando.nombre;
    const marcaCopia = this.productoEditando.marca;
    const idCopia = this.productoEditando.id;

    const datosGuardados = {
      id: idCopia,
      codigoBarras: this.productoEditando.codigoBarras,
      nombre: nombreCopia,
      marca: marcaCopia,
      ingredientes: this.productoEditando.ingredientes,
      silicona: this.productoEditando.silicona,
      alcohol: this.productoEditando.alcohol,
      sulfato: this.productoEditando.sulfato,
      esApto: this.productoEditando.esApto
    };

    //const urlPut = `http://127.0.0.1:5216/api/ProductoEscaners/${this.productoEditando.id}`;
    const urlPut = `http://localhost:8000/api/ProductoEscaners/${this.productoEditando.id}`;

    this.productoEditando = null;                                           //cierra la ventana de edicion

    this.http.put(urlPut, datosGuardados).subscribe({
      next: () => {
        this.mostrarAviso(`Se ha actualizado el producto ${nombreCopia}  ${marcaCopia} correctamente`);
        this.cargarProductos();                                             //recarga la pagina
      },
      error: () => this.mostrarAviso(`ERROR: No se han podido guardar los cambios del producto  ${this.productoEditando.nombre}  ${this.productoEditando.marca} `)
    });
  }

  cargarProductos() {
    this.http.get<any[]>(this.url).subscribe({
      next: (data) => {
        this.productos = [...data];
        this.cd.detectChanges();
      }
    });
  }

  eliminarProducto(id: number) {
    const productoABorrar = this.productos.find(p => p.id === id);
    const nombreCopia = productoABorrar ? productoABorrar.nombre : '';
    const marcaCopia = productoABorrar ? productoABorrar.marca : '';

    if (confirm('¿Estás seguro de que quieres eliminar este producto?')) {
      const urlDelete = `http://localhost:8000/api/ProductoEscaners/${id}`; 
      //const urlDelete = `http://127.0.0.1:5216/api/ProductoEscaners/${id}`;

      this.http.delete(urlDelete).subscribe({
        next: () => {
          this.mostrarAviso(`Se ha eliminado el producto  ${nombreCopia}  ${marcaCopia} correctamente`);
          this.cargarProductos();                                               //recarga la pagina
        },
        error: (err) => console.error('Error al eliminar:', err)
      });
    }
  }

  mostrarAviso(mensaje: string) {
    this.mensajeAviso = mensaje;
    setTimeout(() => {
      this.mensajeAviso = null;
    }, 3000);
  }

}
