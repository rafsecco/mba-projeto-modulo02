import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [],
  template: `
    <footer class="bg-dark fixed-bottom footer">
      <div>
        <p style="padding-top: 10px;" class="text-white">
          Mini loja virtual - MBA DEVXPERT FULL STACK .NET - Desenvolvedor.io
        </p>
      </div>
    </footer>
  `
})
export class FooterComponent {

}
