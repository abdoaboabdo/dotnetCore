import { VehicleService } from '../../services/vehicle.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ToastContainerDirective, ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import  'rxjs/add/observable/forkJoin';
import { SaveVehicle, Vehicle } from 'src/app/models/vehicle';
import * as _ from 'underscore';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
  makes:any[];
  features:any[];
  models:any[];
  vehicle: SaveVehicle = {
    id: 0,
    makeId: 0,
    modelId: 0,
    isRegistered: false,
    features: [],
    contact: {
      name: '',
      email: '',
      phone: '',
    }
  };
  // @ViewChild(ToastContainerDirective, {static: true}) toastContainer: ToastContainerDirective;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private vehicleService:VehicleService,
    private toastrService:ToastrService
  ) { 
    route.params.subscribe(p=>{
      this.vehicle.id = +p['id']
    });
  }

  ngOnInit() {
    var sources:object[]=[
      this.vehicleService.getMakes(),
      this.vehicleService.getFeatures()
    ]
    if (this.vehicle.id) {
      sources.push( this.vehicleService.getVehicle(this.vehicle.id) )
    }
    Observable.forkJoin(sources)
      .subscribe(data=>{
        this.makes=data[0];
        this.features=data[1];
        if (this.vehicle.id>0) {
          this.setVehicle(data[2]);
          this.populateModels();
        }
      },error=>{
        if (error.status == 404) {
          this.router.navigate(['/']);
        }
      })
    
  }

  private setVehicle(v:Vehicle){
    this.vehicle.id=v.id; ;
    this.vehicle.makeId=v.make.id;
    this.vehicle.modelId=v.model.id;
    this.vehicle.isRegistered=v.isRegistered;
    this.vehicle.contact=v.contact;
    this.vehicle.features=_.pluck(v.features,'id');
  }

  onMarkChange(){
    this.populateModels();
    this.vehicle.modelId=null;
  }

  private populateModels() {
    var selectedMake = this.makes.find(m => m.id == this.vehicle.makeId);
    this.models = selectedMake ? selectedMake.models : [];
  }

  onFeatureToggle(featureId, $event){
    if($event.target.checked)
      this.vehicle.features.push(featureId)
    else{
      var index = this.vehicle.features.indexOf(featureId);
      this.vehicle.features.splice(index,1)
    }
  }
  submit(){
    if (this.vehicle.id > 0) {
      this.vehicleService.updateVehicle(this.vehicle)
        .subscribe(x=>{
          this.toastrService.success('The Vehicle was successfully Updated','Success',{
            closeButton:true,
            timeOut:5000,
          })
        })
    }else{
      this.vehicle.id=0;
      this.vehicleService.createVehicle(this.vehicle)
        .subscribe(
          res=>{
            this.toastrService.success('The Vehicle was successfully Created','Success',{
              closeButton:true,
              timeOut:5000,
            })
          }
        );
    }
  }

  delete(){
    if (confirm('Are you Sure?')) {
      this.vehicleService.deleteVehicle(this.vehicle.id)
        .subscribe(res=>{
          this.toastrService.error('The Vehicle was successfully deleted','Success',{
            closeButton:true,
            timeOut:5000,
          });
        this.router.navigate(['/']);
      });
    }
  }
}
