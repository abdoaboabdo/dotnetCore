import { ErrorHandler, Inject, NgZone } from "@angular/core";
import { ToastrService } from "ngx-toastr";

// @Injectable() // <<<=== required if the constructor has parameters 
export class AppErrorHandler implements ErrorHandler {
    constructor(
        private ngZone:NgZone,
        @Inject(ToastrService) private toastrService:ToastrService
        ) {}
    handleError(error: any): void {
        this.ngZone.run(()=>{
            this.toastrService.error('An unExpected error happened','Error')
        });
    }
}