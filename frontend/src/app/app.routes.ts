import { Web } from './features/web/web';
import { Inicio } from './features/web/components/inicio/inicio';
import { Routes } from '@angular/router';
import { Login } from './features/web/components/auth/login/login';
import { Registro } from './features/web/components/auth/registro/registro';
import { Component } from '@angular/core';




export const routes: Routes = [
    { path: '', component: Web },
    { path: 'login', component: Login },
    { path: 'registro', component: Registro },
    { path: 'inicio', component: Inicio },
    { path: '', redirectTo: 'inicio', pathMatch: 'full' },
    { path: '**', redirectTo: '' }
];
