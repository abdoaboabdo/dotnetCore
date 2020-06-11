import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { HttpClient, HttpEvent, HttpErrorResponse, HttpEventType, XhrFactory } from  '@angular/common/http';
import { map } from  'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProgressService {
  private uploadProgress:Subject<any> ;

  startTracking(){
    this.uploadProgress=new Subject();
    return this.uploadProgress;
  }

  notify(progress){
    this.uploadProgress.next(progress);
  }

  endTranking(){
    this.uploadProgress.complete();
  }

  constructor() { }
}


@Injectable()
export class BrowserXhr implements XhrFactory {
  constructor() {}
  build(): any {
    return <any>(new XMLHttpRequest());
  }
}


@Injectable({providedIn: 'root'})
export class BrowserXhrWithProgress extends BrowserXhr{

  constructor(private service:ProgressService) { super(); }
  build(): XMLHttpRequest {
    var xhr : XMLHttpRequest = super.build();
    // xhr.onprogress = (event) => {
    //   this.service.downloadProgress.next(this.createProgress(event));
    // };

    xhr.upload.onprogress = (event) => {
      this.service.notify(this.createProgress(event));
    };

    xhr.upload.onloadend = ()=>{
      this.service.endTranking()
    };
    return xhr;
  }
  
  private createProgress(event) {
    return {
      total:event.total,
      percentage:Math.round( event.loaded / event.total *100)
    }
  }
  
}