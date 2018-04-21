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
    get<T>(controller: string, params: any) {
        return this.http.get(this.baseUrl + 'api/'+controller+'?' + params, this.authenticationService.jwt()).map((response: Response) => response.json() as T);
    }

    getAll<T>(controller: string) {
        return this.http.get(this.baseUrl + 'api/'+controller, this.authenticationService.jwt()).map((response: Response) => response.json() as T[]);
    }

    getById<T>(controller: string, id: any) {
        return this.http.get(this.baseUrl + 'api/'+controller+'/' + id, this.authenticationService.jwt()).map((response: Response) => response.json() as T);
    }

    create<T>(controller: string, object: T) {
        return this.http.post(this.baseUrl + 'api/'+controller, object, this.authenticationService.jwt()).map((response: Response) => response.json() as T);
    }

    update<T>(controller: string, id: string, object?: T) {
        return this.http.put(this.baseUrl + 'api/'+controller+'/' + id, object, this.authenticationService.jwt()).map((response: Response) => response.json() as T);
    }

    delete(controller: string, id: any) {
        return this.http.delete(this.baseUrl + 'api/'+controller+'/' + id, this.authenticationService.jwt());
    }

    deleteIds(controller: string, ids: string[]) {
        return this.http.post(this.baseUrl + 'api/'+controller+'/ids', ids, this.authenticationService.jwt());
    }

    dbReset(){
        return this.http.post(this.baseUrl + 'api/settings/dbreset', { }, this.authenticationService.jwt());       
    }

    
}
