import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MenuLoginComponent } from '../menu-login/menu-login.component';


@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [RouterModule, CommonModule,MenuLoginComponent],
  templateUrl: './menu-component.html'
})
export class MenuComponent {}
