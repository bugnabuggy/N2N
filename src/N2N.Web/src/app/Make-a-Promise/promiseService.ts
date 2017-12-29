import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { StoreHeaders } from '../storeHeaders';
import { StoreLinks } from '../storeLinks';
import { Data } from '@angular/router/src/config';



@Injectable()
export class PromiseService {
    // URL to web api
    constructor(
        private http: Http,
        private _storeHeaders: StoreHeaders,
        private _storeLinks: StoreLinks
    ) { }

    test="ddddd";
    savePromiseOnServer(textPromise: string, dataImplementationPromise: Data, isPublish: boolean): Promise<any> {
        var data = {
            textPromise,
            dataImplementationPromise,
            isPublish
        };
        debugger
        this.test="xaxaxaxaxa";
        return this.http.post(
            this._storeLinks.SavePromiseOnServerUrl,
            data,
            { headers: this._storeHeaders.jsonAndTokenHeaders }
        )
            .toPromise()
            .then(resp => { return resp })
            .catch(this.handleError);
    }
    private handleError(error: any): Promise<any> {

        return Promise.reject(error);
      }
}