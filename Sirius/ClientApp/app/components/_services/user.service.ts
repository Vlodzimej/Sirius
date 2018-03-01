import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';

import { User } from '../_models';
import { Observable } from 'rxjs/Rx';

@Injectable()
export class UserService {
    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) { }

    getAll() {
        return this.http.get(this.baseUrl + 'api/user', this.jwt()).map((response: Response) => response.json());
    }

    getById(id: number) {
        return this.http.get(this.baseUrl + 'api/user/' + id, this.jwt()).map((response: Response) => response.json());
    }

    create(user: User) {
        return this.http.post(this.baseUrl + 'api/user', user, this.jwt());
    }

    update(user: User) {
        return this.http.put(this.baseUrl + 'api/user/' + user.id, user, this.jwt());
    }

    delete(id: number) {
        return this.http.delete(this.baseUrl + 'api/user/' + id, this.jwt());
    }

    // private helper methods

    private jwt() {
        let headers = new Headers();
        // create authorization header with jwt token
        var fromLocal = localStorage.getItem('currentUser');
        if (fromLocal != null) {
            let currentUser = JSON.parse(fromLocal);
            if (currentUser && currentUser.token) {
                let headers = new Headers({ 'Authorization': 'Bearer ' + currentUser.token });
            }
        }
        return new RequestOptions({ headers: headers });
    }
}

/*
import { Injectable, Inject } from '@angular/core';
import { User } from './user';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import { ApiService } from './../api/api.service';

@Injectable()
export class UserService {

    state: string = "in-progress";
    constructor(private apiService: ApiService) { }

    Create(user: User)
    {
        return this.apiService.Post(user, 'user');
    }

    Login(user: User)
    {
        return this.apiService.Post(user, "user/login");
    }
}
*/
