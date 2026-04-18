import { Component, inject, Inject, output } from '@angular/core';
import { Inicio } from "./components/inicio/inicio";
import { ProductosService } from './services/productos-service';
import { ProductoEscanerI } from './models/productoEscaneri';
import { Escaner } from "./components/escaner/escaner";
import { Cuidados } from "./components/cuidados/cuidados";

@Component({
  selector: 'app-web',
  imports: [Inicio],
  templateUrl: './web.html',
  styleUrl: './web.css',
})
export class Web {

}
