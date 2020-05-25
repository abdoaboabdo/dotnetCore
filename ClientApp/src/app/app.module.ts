import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule, ToastContainerModule } from 'ngx-toastr';
import { DataTablesModule } from 'angular-datatables';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { VehicleFormComponent } from './components/vehicle-form/vehicle-form.component';
import { VehicleService } from './services/vehicle.service';
import { AppErrorHandler } from './app.error-handler';
import { VehicleListComponent } from './components/vehicle-list/vehicle-list.component';
import { VehicleRouteComponent } from './components/vehicle-route/vehicle-route.component';
import { PaginationComponent } from './components/shared/pagination/pagination.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    VehicleFormComponent,
    VehicleListComponent,
    VehicleRouteComponent,
    PaginationComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'vehicles', component: VehicleRouteComponent , children:[
        { path: '', component: VehicleListComponent },
        { path: 'new', component: VehicleFormComponent },
        { path: ':id', component: VehicleFormComponent },
      ]},
      
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      // { path: '**', redirectTo: '/'}
    ]),
    CommonModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot({ 
      timeOut: 5000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
      closeButton:true,
     }), // ToastrModule added
    ToastContainerModule,
    DataTablesModule
  ],
  providers: [
    {provide : ErrorHandler, useClass : AppErrorHandler},
    VehicleService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
