import { VehicleService } from './../../services/vehicle.service';
import { Component, OnInit } from '@angular/core';
import { Vehicle, KeyValuePair } from 'src/app/models/vehicle';
import { Observable } from 'rxjs';
import { HttpClient , HttpResponse} from '@angular/common/http';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {
  private readonly PAGE_SIZE=3;
  dtOptions: DataTables.Settings = {};
  makes:any[];
  query:any={
    pageSize:this.PAGE_SIZE
  };
  queryResult:any={};
  totalItems=10;
  columns=[
    { title : 'Id' },
    { title : 'Make', key : 'make', isSortable : true },
    { title : 'Model', key : 'model', isSortable : true },
    { title : 'Contact Name', key : 'contactName', isSortable : true },
    {  },
  ];

  constructor(private vehicleService:VehicleService,private http:HttpClient) { }

  ngOnInit() {
    
    var sources:object[]=[
      this.vehicleService.getMakes(),
      this.vehicleService.getVehicles(this.query)
    ];
    Observable.forkJoin(sources).subscribe(data=>{
      this.makes=data[0];
      this.queryResult=data[1];
    });


  }

  populateVehicles(){
    this.vehicleService.getVehicles(this.query)
      .subscribe(result=>{
        this.queryResult=result;
    });
  }

  onFilterChange(){
    this.query.page=1;
    this.populateVehicles();
  }

  resetFilter(){
    this.query={
      page:1,
      pageSize:this.PAGE_SIZE
    };
    this.populateVehicles()
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

  onPageChange(page){
    this.query.page=page;
    this.populateVehicles();
  }

}
