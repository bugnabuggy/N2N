import { Injectable } from '@angular/core';

@Injectable()

export class StoreLinks {
    public registerUrl = 'http://localhost:63354/User/register';
    public logoutUrl = 'http://localhost:63354/User/LogOut';
    public СheckUserUrl = 'http://localhost:63354/User';
    public logInUrl='http://localhost:63354/User/LogIn';
    public isAuthorizationUrl='http://localhost:63354/User/IsAuthorization';
    public SavePromiseOnServerUrl='http://localhost:63354/Promise';

}