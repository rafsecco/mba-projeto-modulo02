import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MenuComponent } from "./navegacao/menu/menu-component";
import { FooterComponent } from './navegacao/footer/footer-component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, MenuComponent, FooterComponent],
  templateUrl: './app.html'
})
export class App {
  protected title = 'AppAngular';
}
