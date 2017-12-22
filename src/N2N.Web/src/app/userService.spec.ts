import { async, inject, TestBed } from '@angular/core/testing';

import {
    MockBackend,
    MockConnection
} from '@angular/http/testing';

import { RegistrationComponent } from './registration/registration.component'
import {
    HttpModule,
    Http,
    Headers,
    XHRBackend,
    ResponseOptions,
    Response,
} from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/toPromise';

import { StoreHeaders } from './storeHeaders';
import { StoreLinks } from './storeLinks';
import { UserService } from './userService';

describe('UserService', () => {
    // Mock http response

    let mockResponse = [
        {
            'access_token': '',
            'refresh_token': ''
        }
    ];

    const jsonAndTokenHeaders = new Headers({
        'Content-Type': 'application/json charset=utf-8',
        "Authorization": 'Bearer '
    });


    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [HttpModule],

            providers: [UserService, StoreHeaders, StoreLinks,
                { provide: XHRBackend, useClass: MockBackend }
            ]
        }).compileComponents();
    }));

    it('can instantiate service when inject service',
        inject([UserService], (service: UserService) => {
            expect(service instanceof UserService).toBe(true);
        }));




    it('can provide the mockBackend as XHRBackend',
        inject([XHRBackend], (backend: MockBackend) => {
            expect(backend).not.toBeNull('backend should be provided');
        }));

    describe('when sendUserDataForRegistration', () => {
        let backend: MockBackend;
        let service: UserService;
        let headers: StoreHeaders;
        let links: StoreLinks;
        let response: Response;

        beforeEach(inject([Http, XHRBackend, StoreHeaders, StoreLinks],
            (http: Http, be: MockBackend, storeHeaders: StoreHeaders, storeLinks: StoreLinks) => {
                backend = be;
                headers = storeHeaders;
                links = storeLinks;
                service = new UserService(http, headers, links);

                let options = new ResponseOptions({ status: 200, url: 'http://localhost:63354/User/register', body: mockResponse });
                response = new Response(options);
            }));
        it('should have expected ', async(inject([], () => {
            backend.connections.subscribe((c: MockConnection) => c.mockRespond(response));

            service.sendUserDataForRegistration("", "", "")
                .then(answer => {
                    expect(answer._body).toBe(mockResponse);
                })

        })));
        it('should be OK returning', async(inject([], () => {
            let resp = new Response(new ResponseOptions({ status: 200, body: mockResponse }));
            backend.connections.subscribe((c: MockConnection) => {

                c.mockRespond(resp)
                expect(c.request.url).toEqual('http://localhost:63354/User/register');
            });

            service.sendUserDataForRegistration("", "", "")
                .then(answer => {
                    expect(answer._body).toBe(mockResponse);
                })
        })));
        it('should treat 404 as error', async(inject([], async() => {
            let resp = new Response(new ResponseOptions({ status: 404 }));
            backend.connections.subscribe((c: MockConnection) => c.mockRespond(resp));

            service.sendUserDataForRegistration("", "", "")
                .then(answer => {
                    // fail('should not respond');
                })
                .catch(err => {
                    expect(err).toMatch(/Bad response status/, 'should catch bad response status code');
                    return Promise.reject(null);; // failure is the expected test result
                })


        })));

    });
    describe('when checkUser', () => {
        let backend: MockBackend;
        let service: UserService;
        let headers: StoreHeaders;
        let links: StoreLinks;
        let response: Response;

        beforeEach(inject([Http, XHRBackend, StoreHeaders, StoreLinks],
            (http: Http, be: MockBackend, storeHeaders: StoreHeaders, storeLinks: StoreLinks) => {
                backend = be;
                headers = storeHeaders;
                links = storeLinks;
                service = new UserService(http, headers, links);

                let options = new ResponseOptions({ status: 200, url: 'http://localhost:63354/User/СheckUser' });
                response = new Response(options);
            }));
        it('should have expected ', async(inject([], () => {
            backend.connections.subscribe((c: MockConnection) => c.mockRespond(response));

            service.checkUser()
                .then(answer => {
                    expect(answer._body).toBe(undefined);
                })

        })));
        
        it('should treat 404 as error', async(inject([], async() => {
            let resp = new Response(new ResponseOptions({ status: 404 }));
            backend.connections.subscribe((c: MockConnection) => c.mockRespond(resp));

            service.checkUser()
                .then(answer => {
                    // fail('should not respond');
                })
                .catch(err => {
                    expect(err).toMatch(/Bad response status/, 'should catch bad response status code');
                    return Promise.reject(null);; // failure is the expected test result
                })


        })));

    });
    describe('when LogOut', () => {
        let backend: MockBackend;
        let service: UserService;
        let headers: StoreHeaders;
        let links: StoreLinks;
        let response: Response;

        beforeEach(inject([Http, XHRBackend, StoreHeaders, StoreLinks],
            (http: Http, be: MockBackend, storeHeaders: StoreHeaders, storeLinks: StoreLinks) => {
                backend = be;
                headers = storeHeaders;
                links = storeLinks;
                service = new UserService(http, headers, links);

                let options = new ResponseOptions({ status: 200, url: 'http://localhost:63354/User/LogOut'});
                response = new Response(options);
            }));
        it('should have expected ', async(inject([], () => {
            backend.connections.subscribe((c: MockConnection) => c.mockRespond(response));

            service.logOut()
                .then(answer => {
                    expect(answer._body).toBe(null)
                })

        })));
        
        it('should treat 404 as error', async(inject([], async() => {
            let resp = new Response(new ResponseOptions({ status: 404 }));
            backend.connections.subscribe((c: MockConnection) => c.mockRespond(resp));

            service.logOut()
                .then(answer => {
                    // fail('should not respond');
                })
                .catch(err => {
                    expect(err).toMatch(/Bad response status/, 'should catch bad response status code');
                    return Promise.reject(null);; // failure is the expected test result
                })


        })));

    });
    describe('when logIn', () => {
        let backend: MockBackend;
        let service: UserService;
        let headers: StoreHeaders;
        let links: StoreLinks;
        let response: Response;

        beforeEach(inject([Http, XHRBackend, StoreHeaders, StoreLinks],
            (http: Http, be: MockBackend, storeHeaders: StoreHeaders, storeLinks: StoreLinks) => {
                backend = be;
                headers = storeHeaders;
                links = storeLinks;
                service = new UserService(http, headers, links);

                let options = new ResponseOptions({ status: 200, url: 'http://localhost:63354/User/register', body: mockResponse });
                response = new Response(options);
            }));
        it('should have expected ', async(inject([], () => {
            backend.connections.subscribe((c: MockConnection) => c.mockRespond(response));

            service.logIn("", "", "")
                .then(answer => {
                    expect(answer._body).toBe(mockResponse);
                })

        })));
        it('should be OK returning', async(inject([], () => {
            let resp = new Response(new ResponseOptions({ status: 200, body: mockResponse }));
            backend.connections.subscribe((c: MockConnection) => {

                c.mockRespond(resp)
                expect(c.request.url).toEqual('http://localhost:63354/User/LogIn');
            });

            service.logIn("", "", "")
                .then(answer => {
                    expect(answer._body).toBe(mockResponse);
                })
        })));
        it('should treat 404 as error', async(inject([], async() => {
            let resp = new Response(new ResponseOptions({ status: 404 }));
            backend.connections.subscribe((c: MockConnection) => c.mockRespond(resp));

            service.logIn("", "", "")
                .then(answer => {
                    // fail('should not respond');
                })
                .catch(err => {
                    expect(err).toMatch(/Bad response status/, 'should catch bad response status code');
                    return Promise.reject(null);; // failure is the expected test result
                })


        })));

    });
});




