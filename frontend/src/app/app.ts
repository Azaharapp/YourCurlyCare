import { Component, signal } from '@angular/core';
import { Web } from "./features/web/web";
import { RouterModule } from "@angular/router";
import { Header } from "./features/shared/header/header";
import { Footer } from "./features/shared/footer/footer";

@Component({
  selector: 'app-root',
  imports: [ RouterModule, Header, Footer],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('frontend');
}
