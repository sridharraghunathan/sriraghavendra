import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LocalStorageService } from '@app-services/localstorage.service';
import { globaldata } from '@app/models/constants';
import { IUser } from '@app/models/IUser';
import { finalize } from 'rxjs/operators';
import { BusyService } from '@app-services/busy.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(private localStorage: LocalStorageService, private busyService: BusyService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    /* mention controller name and make sure that generic information are in the common controller no need to id token

    //https://localhost/home/auth 1
    //https://localhost/home/common 1
    //https://localhost/home/aradhana 0 0

    if we dont want to send the token to backend API call then we need to skip those itesm so created new Array 

    */
    let exceptionList = ['authorization', 'common'];

    let initialState = 0;
    // Loading Spinner is Injected here via service so while sending the request to the backend
    // API Call then Loading spinner will start to Run

    this.busyService.busy();

    exceptionList.map((el) => {
      if (req.url.includes(el)) {
        initialState += 1;
      }
    });

    if (initialState === 0) {
      req = this.addHeaders(req);
    }
    return next.handle(req).pipe(
      // Delaying the repsonse for 1s for showing the spinner if the request is faster
      // we can also remove it if not needed
      // delay(1000),
      // once the request got completed then we are closing the Spinner service
      finalize(() => {
        this.busyService.idle();
      })
    );
  }

  private addHeaders(req: HttpRequest<any>) {
    const token = (this.localStorage.getLocalStorageByKey(globaldata.userData) as IUser)?.idToken;
    /* modify the request attaching the header with JWT if the token is available for each request */
    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: token,
        },
      });
    }
    return req;
  }
}
