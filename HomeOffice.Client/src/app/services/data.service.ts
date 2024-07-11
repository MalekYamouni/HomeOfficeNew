import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  private apiUrl = 'http://localhost:5001/api/details'

  constructor(private _http : HttpClient) { }

  getTimeEntriesByDate(date: string): Observable<any> {
    let params = new HttpParams().set('date', date);
    return this._http.get<any>(`${this.apiUrl}/getByDate`, { params });
  }
}
