import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";


@Injectable({
  providedIn: 'root'
})
export class VehicleService {

  constructor(private http:HttpClient) { }
  getMakes(){
    return this.http.get<any[]>('/api/makes');
  }

  getFeatures(){
    return this.http.get<any[]>('/api/features');
  }
}
