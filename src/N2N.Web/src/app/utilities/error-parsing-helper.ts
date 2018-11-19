import { HttpErrorResponse } from '@angular/common/http';

export class ErrorParsingHelper {

    static getErrorMessage(err: HttpErrorResponse): string {

        if (err.error && err.error.error_description) {
            return err.error.error_description;
        }

        if ( err.message ) {
            return err.message;
        }

        return err as any;
    }
}
