import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Medicine } from '../models/medicine.model';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private http = inject(HttpClient);
  private base = '/api'; // proxy routes /api -> https://localhost:7201

  getMedicines(search?: string): Observable<Medicine[]> {
    let params = new HttpParams();
    if (search) params = params.set('search', search);
    return this.http.get<Medicine[]>(`${this.base}/medicines`, { params });
  }

  addMedicine(dto: any) {
    return this.http.post(`${this.base}/medicines`, dto);
  }

  recordSale(dto: any) {
    return this.http.post(`${this.base}/sales`, dto);
  }
}
