export class Endpoints {
    static site = {
        login: 'login',
        registration: 'registration',
        resetPassword: 'reset_password',
        dashboard: 'dashboard',
    };

    static apiUrl = 'http://localhost:56199';

    static api = {
        get login() { return Endpoints.apiUrl + '/user/login'; },
        get register() { return Endpoints.apiUrl + '/user/register'; }
    };

}
