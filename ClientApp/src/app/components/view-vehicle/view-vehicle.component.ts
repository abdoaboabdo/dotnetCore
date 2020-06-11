import { NgZone } from '@angular/core';
import { PhotoService } from './../../services/photo.service';
import { ToastrService } from 'ngx-toastr';
import { VehicleService } from './../../services/vehicle.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ProgressService } from 'src/app/services/progress.service';

@Component({
  selector: 'app-view-vehicle',
  templateUrl: './view-vehicle.component.html',
  styleUrls: ['./view-vehicle.component.css']
})
export class ViewVehicleComponent implements OnInit {
  @ViewChild('fileInput',{static:false}) fileInput:ElementRef;
  vehicle: any;
  vehicleId: number; 
  photos:any[];
  progress:any;
  constructor(
    private zone:NgZone,
    private route: ActivatedRoute, 
    private router: Router,
    private toastrService: ToastrService,
    private photoService:PhotoService,
    private progressService:ProgressService,
    private vehicleService: VehicleService
  ) {
    route.params.subscribe(p => {
      this.vehicleId = +p['id'];
      if (isNaN(this.vehicleId) || this.vehicleId <= 0) {
        router.navigate(['/vehicles']);
        return; 
      }
    });
  }

  ngOnInit() {
    this.photoService.getPhotos(this.vehicleId)
      .subscribe(photos=>this.photos=photos);
    this.vehicleService.getVehicle(this.vehicleId)
      .subscribe(
        v => this.vehicle = v,
        err => {
          if (err.status == 404) {
            this.router.navigate(['/vehicles']);
            return; 
          }
        });
  }

  delete() {
    if (confirm("Are you sure?")) {
      this.vehicleService.deleteVehicle(this.vehicle.id)
        .subscribe(x => {
          this.router.navigate(['/vehicles']);
        });
    }
  }

  onUploadPhoto(){

    

    this.progressService.startTracking()
      .subscribe(progress=>{
        this.zone.run(()=>this.progress=progress)
      },null,
      ()=>{this.progress=null});

    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;
    var file = nativeElement.files[0]
    nativeElement.value='';
    this.photoService.upload(this.vehicleId,file)
      .subscribe(photo=>{
        this.photos.push(photo)
      },error=>{
        this.toastrService.error(error.text(),'Error')
      })
  }
}
