import { Routes } from '@angular/router';
import { HomeComponent } from './navegacao/home/home-component';
import { DetalheComponent } from './produto/detalhe/detalhe-component';
import { contaRoutes } from './conta/conta.route';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'produtos/detalhe/:id', component: DetalheComponent },
  {
    path: 'conta',
    children: contaRoutes 
  }
];
