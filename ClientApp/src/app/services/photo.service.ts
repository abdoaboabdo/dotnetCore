import { HttpClient,HttpEvent, HttpErrorResponse, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from  'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  constructor(private http:HttpClient) { }

  getPhotos(vehicleId){
    return this.http.get<any[]>(`/api/vehicles/${vehicleId}/photos`);
  }


  upload(vehicleId,photo){
    var formData=new FormData();
    formData.append('file',photo);
    return this.http.post(`/api/vehicles/${vehicleId}/photos`,formData);
  }
}
