import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { SaveVehicle, Vehicle } from '../models/vehicle';


@Injectable({
  providedIn: 'root'
})
export class VehicleService {

  private readonly VehiclesEndpoient = '/api/vehicles' ; 
  constructor(private http:HttpClient) { }
  getMakes(){
    return this.http.get<any[]>('/api/makes');
  }

  getFeatures(){
    return this.http.get<any[]>('/api/features');
  }

  createVehicle(vehicle:SaveVehicle){
    return this.http.post(this.VehiclesEndpoient,vehicle);
  }

  getVehicle(id:number){
    return this.http.get(this.VehiclesEndpoient+'/'+id);
  }

  updateVehicle(vehicle:SaveVehicle){
    return this.http.put(this.VehiclesEndpoient+'/'+vehicle.id,vehicle);
  }

  deleteVehicle(id){
    return this.http.delete(this.VehiclesEndpoient+'/'+id);
  }

  getVehicles(filter){
    return this.http.get(this.VehiclesEndpoient+'?'+this.toQueryString(filter));
  }

  toQueryString(obj){
    var parts = [];
    for ( var property in obj) {
      var value = obj[property];
      if (value != null && value != undefined) {
        parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value));
      }
    }
    return parts.join('&');
  }
}
