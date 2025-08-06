// src/app/conta/conta.route.ts
import { Routes } from '@angular/router';

export const contaRoutes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./conta.app.component').then(m => m.ContaAppComponent)
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'cadastro',
    loadComponent: () =>
      import('./cadastro/cadastro.component').then(m => m.CadastroComponent)
  }
];
