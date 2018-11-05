export interface LoginContract {
    access_token: string;
    refresh_token?: string;
    expiration_date?: Date;
}
