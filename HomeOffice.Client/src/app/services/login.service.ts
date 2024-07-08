import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../Interfaces/user';
@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private apiUrl = 'http://localhost:5001/api/auth/login';

  constructor(private _http: HttpClient) {}

  public easylogin(username: string, password: string) {
    return this._http.post<{ message: string }>(this.apiUrl, {
      username,
      password,
    });
  }
}
