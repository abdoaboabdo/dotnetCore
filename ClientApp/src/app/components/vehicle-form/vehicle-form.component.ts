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
  vehicle:any={};
  constructor(
    private vehicleService:VehicleService) { }

  ngOnInit() {
    this.vehicleService.getMakes().subscribe(res => this.makes=res )
    this.vehicleService.getFeatures().subscribe(res => this.features=res )
  }

  onMarkChange(){
    var selectedMake = this.makes.find(m => m.id == this.vehicle.make);
    console.log(selectedMake);
    this.models = selectedMake ? selectedMake.models : [];
  }
}
