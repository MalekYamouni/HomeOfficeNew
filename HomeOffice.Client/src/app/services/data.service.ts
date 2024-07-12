import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Time } from '@angular/common';
import { OfficeTime } from '../Interfaces/ITime';

@Injectable({
  providedIn: 'root',
})
export class DataService {

  private apiUrl = 'http://localhost:5001/api/details';

  constructor(private _http: HttpClient) {}

  public getAll() : Observable<OfficeTime[]> {
    return this._http.get<OfficeTime[]>(`${this.apiUrl}/getAll`);
  }
}
