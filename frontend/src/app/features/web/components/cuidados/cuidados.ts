import { RouterModule } from '@angular/router';
import { Component } from '@angular/core';

@Component({
  selector: 'app-cuidados',
  imports: [ RouterModule],
  templateUrl: './cuidados.html',
  styleUrl: './cuidados.css',
})
export class Cuidados {
  public mostrarFormas: boolean = false;
  public mostrarTecnicaMR: boolean = false;
  public mostrarTecnicaScrunch: boolean = false;
  public mostrarFases: boolean = false;
  
  desplegarFormas() {
    this.mostrarFormas = !this.mostrarFormas;
  }

  desplegarTecnicaMR() {
    this.mostrarTecnicaMR = !this.mostrarTecnicaMR;
  }

  desplegarTecnicaScrunch() {
    this.mostrarTecnicaScrunch = !this.mostrarTecnicaScrunch;
  }

  desplegarFases() {
    this.mostrarFases = !this.mostrarFases;
  }
}
