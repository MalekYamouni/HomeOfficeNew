import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Time } from '../Interfaces/ITime';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TimeService {
  private apiUrl = 'http://localhost:5001/api/time';

  constructor(private _http: HttpClient) {}

  startTime(): Observable<any> {
    return this._http.post<any>(`${this.apiUrl}/start`, {});
  }

  stopTime(): Observable<any> {
    return this._http.post<any>(`${this.apiUrl}/stop`, {});
  }
}
