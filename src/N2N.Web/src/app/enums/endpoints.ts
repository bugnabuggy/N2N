import { environment } from '../../environments/environment';

export class Endpoints {
    static site = {
        login: 'login',
        registration: 'registration',
        resetPassword: 'reset_password',
        dashboard: 'dashboard',
    };

    static apiUrl = environment.apiUrl;

    static api = {
        // get login() { return Endpoints.apiUrl + '/user/login'; },
        get identityServerLogin() { return Endpoints.apiUrl + '/connect/token'; },
        get register() { return Endpoints.apiUrl + '/user/register'; }
    };

}
