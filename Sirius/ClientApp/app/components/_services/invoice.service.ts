import { Injectable, Inject } from '@angular/core';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import { AuthenticationService } from './../_services';
import { Invoice } from './../_models';

@Injectable()
export class InvoiceService {

    headers: Headers;
    options: RequestOptions;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private authenticationService:AuthenticationService) {
        this.headers = new Headers({
            'Content-Type': 'application/json',
            'Accept': 'q=0.8;application/json;q=0.9'
        });
        this.options = new RequestOptions({ headers: this.headers });
    }

    getByInvoiceId<InvoiceT>(controller: string, id: any) {
        return this.http.get(this.baseUrl + 'api/invoice/' + id, this.authenticationService.jwt()).map((response: Response) => response.json() as Invoice);
    }
}