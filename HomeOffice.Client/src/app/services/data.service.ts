import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  private apiUrl = 'https://localhost:5001/api/details'

  constructor(private _http : HttpClient) { }

  getHomeOfficeData(): Observable<any> {
    const userId = 1;
    const url = `${this.apiUrl}/userid/${userId}`;
    return this._http.get<any>(url);
  }
}
