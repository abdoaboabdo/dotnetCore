import { Component, OnInit, ViewChild } from '@angular/core';
 
import { ToastContainerDirective, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
  // @ViewChild(ToastContainerDirective, {static: true}) toastContainer: ToastContainerDirective;
 
  constructor(private toastrService: ToastrService) {}
  ngOnInit() {
    // this.toastrService.overlayContainer = this.toastContainer;
  }
  onClick() {
    // this.toastrService.success('in div');
  }
}
