import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title:string = 'CDP Homework 2';

  constructor() {
    console.log('INIT');
  }

  changeTitle(title:string):void {
    this.title = title;
  }
}
