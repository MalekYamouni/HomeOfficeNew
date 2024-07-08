import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../Interfaces/user';

@Injectable({ providedIn: 'root' })
export class UserService {
  constructor(private _http: HttpClient) {}

  // Test um die Daten aus der Datenbank zu bekommen erfolgreich
  public load(): Observable<User[]> {
    return this._http.get<User[]>('api/user');
  }
}
