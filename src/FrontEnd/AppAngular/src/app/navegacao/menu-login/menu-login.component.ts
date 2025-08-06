import { Component, DoCheck, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LocalStorageUtils } from '../../utils/localstorage';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-menu-login',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './menu-login.component.html'
})
export class MenuLoginComponent implements OnInit, DoCheck {
  email: string = '';
  logado: boolean = false;

  private localStorageUtils = new LocalStorageUtils();

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.verificarLogin();
  }

  ngDoCheck(): void {
    this.verificarLogin();
  }

  verificarLogin(): void {
    const token = this.localStorageUtils.obterTokenUsuario();
    const user = this.localStorageUtils.obterUsuario();

    this.logado = !!token;
    this.email = user?.email ?? '';
  }

  logout(): void {
    this.localStorageUtils.limparDadosLocaisUsuario();
    this.router.navigate(['/home']);
  }
}
