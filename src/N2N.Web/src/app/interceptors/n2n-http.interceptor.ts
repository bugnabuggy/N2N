import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { finalize, tap } from 'rxjs/operators';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest
} from '@angular/common/http';


import { SiteStateService, SecurityService } from '../services';


@Injectable()
export class N2NHttpInterceptor implements HttpInterceptor {
  constructor(
    private siteSvc: SiteStateService,
    private securitySvc: SecurityService
    ) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler):
    Observable<HttpEvent<any>> {
    if (this.securitySvc.accessToken) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${this.securitySvc.accessToken}`
        }
      });
    }

    this.siteSvc.requestsCount++ ;

    return next.handle(req).pipe(
      tap(event => {
      }),
      finalize(() => {
        this.siteSvc.requestsCount--;
      })
    );
  }
}
