import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { finalize, tap } from 'rxjs/operators';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest
} from '@angular/common/http';


import { SiteStateService } from '../services/site-state.service';


@Injectable()
export class N2NHttpInterceptor implements HttpInterceptor {
  constructor(public siteSvc: SiteStateService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler):
    Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      tap(event => {
        this.siteSvc.isRequestInProgress = true;
      }),
      finalize(() => {
        this.siteSvc.isRequestInProgress = false;
      })
    );
  }
}
