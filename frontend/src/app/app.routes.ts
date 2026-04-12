import { Web } from './features/web/web';
import { Inicio } from './features/web/components/inicio/inicio';
import { Routes } from '@angular/router';
import { Login } from './features/web/components/auth/login/login';
import { Registro } from './features/web/components/auth/registro/registro';
import { Component } from '@angular/core';
import { Cuidados } from './features/web/components/cuidados/cuidados';
import { Escaner } from './features/web/components/escaner/escaner';
import { Test } from './features/web/components/test/test';
import { Validacion } from './features/web/components/auth/validacion/validacion';
import { RecuperarPass } from './features/web/components/auth/recuperar-pass/recuperar-pass';

export const routes: Routes = [
    { path: '', component: Web },
    { path: 'login', component: Login },
    { path: 'registro', component: Registro },
    { path: 'inicio', component: Inicio },
    { path: 'cuidados', component: Cuidados },
    { path: 'escaner', component: Escaner },
    { path: 'test', component: Test },
    { path: 'validacion', component: Validacion },
    { path: 'recuperar', component: RecuperarPass },
    { path: '', redirectTo: 'inicio', pathMatch: 'full' },
    { path: '**', redirectTo: '' }
];
