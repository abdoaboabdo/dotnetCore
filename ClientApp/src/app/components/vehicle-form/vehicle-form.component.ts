import { VehicleService } from '../../services/vehicle.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
  makes:any[];
  features:any[];
  models:any[];
  vehicle:any={
    features:[],
    contact:{}
  };
  constructor(
    private vehicleService:VehicleService) { }

  ngOnInit() {
    this.vehicleService.getMakes().subscribe(res => this.makes=res )
    this.vehicleService.getFeatures().subscribe(res => this.features=res )
  }

  onMarkChange(){
    var selectedMake = this.makes.find(m => m.id == this.vehicle.makeId);
    this.models = selectedMake ? selectedMake.models : [];
    delete this.vehicle.modelId;
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
    this.vehicleService.createVehicle(this.vehicle)
      .subscribe(res=>console.log(res));
  }

}
