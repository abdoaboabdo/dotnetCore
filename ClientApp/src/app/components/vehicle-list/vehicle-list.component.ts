import { VehicleService } from './../../services/vehicle.service';
import { Component, OnInit } from '@angular/core';
import { Vehicle, KeyValuePair } from 'src/app/models/vehicle';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {
  vehicles:Vehicle[];
  makes:any[];
  query:any={};
  columns=[
    { title : 'Id' },
    { title : 'Make', key : 'make', isSortable : true },
    { title : 'Model', key : 'model', isSortable : true },
    { title : 'Contact Name', key : 'contactName', isSortable : true },
    {  },
  ];

  constructor(private vehicleService:VehicleService) { }

  ngOnInit() {
    
    var sources:object[]=[
      this.vehicleService.getMakes(),
      this.vehicleService.getVehicles(this.query)
    ];
    Observable.forkJoin(sources).subscribe(data=>{
      this.makes=data[0];
      this.vehicles=data[1];
    });
    // this.vehicleService.getMakes().subscribe(makes=>{
    //   this.makes=makes;
    // });
    // this.vehicleService.getVehicles().subscribe(vehicles=>{
    //   this.vehicles = this.allVehicles = vehicles;
    // });
  }

  populateVehicles(){
    this.vehicleService.getVehicles(this.query).subscribe(vehicles=>{
      this.vehicles = vehicles;
    });
  }

  onFilterChange(){
    this.populateVehicles();
  }

  resetFilter(){
    this.query={};
    this.onFilterChange()
  }

  sortBy(columnName:string){
    if (this.query.sortBy === columnName) {
      this.query.IsSortAscending=!this.query.IsSortAscending;
    }else{
      this.query.sortBy = columnName;
      this.query.IsSortAscending=true;
    }
    this.populateVehicles();
  }

}
