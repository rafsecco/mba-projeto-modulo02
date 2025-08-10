import { Component, OnInit } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MenuLoginComponent } from '../menu-login/menu-login.component';
import { LocalStorageUtils } from '../../utils/localstorage';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [RouterModule, CommonModule, MenuLoginComponent],
  templateUrl: './menu-component.html',
})
export class MenuComponent implements OnInit {
  logado = false;
  private localStorageUtils = new LocalStorageUtils();

  ngOnInit(): void {
    let token = this.localStorageUtils.obterTokenUsuario();
    this.logado = token != undefined;
  }
}
