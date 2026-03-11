import { Routes } from '@angular/router';
import { Login } from './features/components/auth/login/login';
import { Registro } from './features/components/auth/registro/registro';




export const routes: Routes = [
    { path: 'login', component: Login },
    { path: 'registro', component: Registro },
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: '**', redirectTo: '/login' }
];
