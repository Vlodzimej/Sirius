import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { AuthenticationService } from './../_services';
import { Dimension } from '../_models';

@Injectable()
export class UserService {
    constructor(private http: Http,
        @Inject('BASE_URL') private baseUrl: string,
        private authenticationService: AuthenticationService) { }

    getAll() {
        return this.http.get(this.baseUrl + 'api/dimension', this.authenticationService.jwt()).map((response: Response) => response.json() as Dimension[]);
    }

    getById(id: any) {
        return this.http.get(this.baseUrl + 'api/dimension/' + id, this.authenticationService.jwt()).map((response: Response) => response.json());
    }

    create(dimension: Dimension) {
        return this.http.post(this.baseUrl + 'api/dimension', dimension, this.authenticationService.jwt());
    }

    update(dimension: Dimension) {
        return this.http.put(this.baseUrl + 'api/dimension/' + dimension.id, dimension, this.authenticationService.jwt());
    }

    delete(id: any) {
        return this.http.delete(this.baseUrl + 'api/dimension/' + id, this.authenticationService.jwt());
    }
}
