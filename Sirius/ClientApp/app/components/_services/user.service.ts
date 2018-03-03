import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { AuthenticationService } from './../_services';
import { User } from '../_models';

@Injectable()
export class UserService {
    constructor(private http: Http,
        @Inject('BASE_URL') private baseUrl: string,
        private authenticationService: AuthenticationService) { }

    getAll() {
        console.log();
        return this.http.get(this.baseUrl + 'api/user', this.authenticationService.jwt()).map((response: Response) => response.json());
    }

    getById(id: number) {
        return this.http.get(this.baseUrl + 'api/user/' + id, this.authenticationService.jwt()).map((response: Response) => response.json());
    }

    create(user: User) {
        return this.http.post(this.baseUrl + 'api/user', user, this.authenticationService.jwt());
    }

    update(user: User) {
        return this.http.put(this.baseUrl + 'api/user/' + user.id, user, this.authenticationService.jwt());
    }

    delete(id: number) {
        return this.http.delete(this.baseUrl + 'api/user/' + id, this.authenticationService.jwt());
    }
}
