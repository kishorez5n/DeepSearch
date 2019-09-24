import { Component } from '@angular/core';

@Component({
  selector: 'pm-root',
  template: `
    <div class='card-container'>
      <router-outlet></router-outlet>
    </div>
    `,
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  pageTitle = '';
}
