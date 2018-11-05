export class Endpoints {
    static site = {
        login: 'login',
        registration: 'registration',
        resetPassword: 'reset_password',
    };

    static apiUrl = 'http://localhost:63354';

    static api = {
        get login() { return Endpoints.apiUrl + '/user/login'; },
        get register() { return Endpoints.apiUrl + '/user/register'; }
    };

}
