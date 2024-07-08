import { Component, OnInit } from '@angular/core';
import { User } from './Interfaces/user';
import { UserService } from './services/user.service';
import { tap } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  public user: User[] = [];

  constructor(private _service: UserService) {}

  ngOnInit() {
    this._service.load().subscribe((u) => (this.user = u));
  }
}
