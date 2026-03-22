import { Component } from '@angular/core';
import { Inicio } from "./components/inicio/inicio";

@Component({
  selector: 'app-web',
  imports: [ Inicio],
  templateUrl: './web.html',
  styleUrl: './web.css',
})
export class Web {

}
