import { Injectable, Inject } from '@angular/core';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import { AuthenticationService } from './../_services';

@Injectable()
export class ApiService {

    headers: Headers;
    options: RequestOptions;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private authenticationService:AuthenticationService) {
        this.headers = new Headers({
            'Content-Type': 'application/json',
            'Accept': 'q=0.8;application/json;q=0.9'
        });
        this.options = new RequestOptions({ headers: this.headers });
    }

    getAll<T>(controller: string) {
        return this.http.get(this.baseUrl + 'api/'+controller, this.authenticationService.jwt()).map((response: Response) => response.json() as T[]);
    }

    getById(id: any) {
        return this.http.get(this.baseUrl + 'api/{controller}/' + id, this.authenticationService.jwt()).map((response: Response) => response.json());
    }

    create<T>(controller: string, object: T) {
        return this.http.post(this.baseUrl + 'api/'+controller, object, this.authenticationService.jwt());
    }

    update<T>(controller: string, id: string, object: T) {
        return this.http.put(this.baseUrl + 'api/'+controller+'/' + id, object, this.authenticationService.jwt());
    }

    delete(id: any, controller: string) {
        return this.http.delete(this.baseUrl + 'api/'+controller+'/' + id, this.authenticationService.jwt());
    }
}
