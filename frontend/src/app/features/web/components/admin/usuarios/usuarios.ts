import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-usuarios',
  imports: [FormsModule],
  templateUrl: './usuarios.html',
  styleUrl: './usuarios.css',
})
export class Usuarios {
  private http = inject(HttpClient);
  private cd = inject(ChangeDetectorRef);

  private url = 'http://localhost:8000/api/Usuarios';
  //private url = 'http://127.0.0.1:5216/api/Usuarios';

  public esAdmin: boolean = localStorage.getItem('rol') === 'Admin';

  public usuarios: any[] = [];
  public usuarioEditando: any = null;
  public mensajeAviso: string | null = null;

  constructor() {
    this.cargarUsuarios();
  }

  abrirEdicion(usuario: any) {
    this.usuarioEditando = { ...usuario };
  }

  guardarEdicion() {
    if (!this.usuarioEditando) return;

    const usernameCopia = this.usuarioEditando.username;
    const idCopia = this.usuarioEditando.id;

    const datosGuardados = {
      id: idCopia,
      codigoBarras: this.usuarioEditando.codigoBarras,
      nombre: this.usuarioEditando.nombre,
      apellido: this.usuarioEditando.apellido,
      username: usernameCopia,
      email: this.usuarioEditando.email,
      fechaRegistro: this.usuarioEditando.fechaRegistro
    };

    //const urlPut = `http://127.0.0.1:5216/api/Usuarios/${idCopia}`;
    const urlPut = `http://localhost:8000/api/Usuarios/${idCopia}`;

    this.usuarioEditando = null;                                          //cierra la ventana de edicion

    this.http.put(urlPut, datosGuardados).subscribe({
      next: () => {
        this.mostrarAviso(`Se ha actualizado el usuario ${usernameCopia} correctamente`);
        this.cargarUsuarios();                                             //recarga la pagina
      },
      error: () => this.mostrarAviso(`ERROR: No se han podido guardar los cambios del usuario  ${usernameCopia} `)
    });
  }

  cargarUsuarios() {
    this.http.get<any[]>(this.url).subscribe({
      next: (data) => {
        this.usuarios = [...data];
        this.cd.detectChanges();
      }
    });
  }

  eliminarUsuario(id: number) {
    const usuarioABorrar = this.usuarios.find(u => u.id === id);
    const usernameCopia = usuarioABorrar ? usuarioABorrar.username : '';

    if (confirm('¿Estás seguro de que quieres eliminar este usuario?')) {
      const urlDelete = `http://localhost:8000/api/Usuarios/${id}`; 
      //const urlDelete = `http://127.0.0.1:5216/api/Usuarios/${id}`;

      this.http.delete(urlDelete).subscribe({
        next: () => {
          this.mostrarAviso(`Se ha eliminado el usuario  ${usernameCopia} correctamente`);
          this.cargarUsuarios();                                          //recarga la pagina
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
