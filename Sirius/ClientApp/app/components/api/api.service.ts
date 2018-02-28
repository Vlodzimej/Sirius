import { Injectable, Inject } from '@angular/core';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';

@Injectable()
export class ApiService {

    headers: Headers;
    options: RequestOptions;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) {
        this.headers = new Headers({
            'Content-Type': 'application/json',
            'Accept': 'q=0.8;application/json;q=0.9'
        });
        this.options = new RequestOptions({ headers: this.headers });
    }

    Post(obj: any, controller: string) {
        let body = JSON.stringify(obj);
        var result = this.http.post(this.baseUrl + 'api/'+controller, obj, { headers: this.headers });
        return result;
    }

}