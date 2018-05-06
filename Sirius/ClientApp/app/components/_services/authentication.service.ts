import { OnInit } from '@angular/core';
import { Injectable, Inject } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';

import 'rxjs/add/operator/map';

@Injectable()
export class AuthenticationService {

    constructor(
        private http: Http, 
        @Inject('BASE_URL') 
        private baseUrl: string, 
        private router: Router
    ) { }

    login(username: string, password: string) {
        return this.http.post(this.baseUrl + 'api/user/authenticate', { username: username, password: password })
            .map((response: Response) => {
                // login successful if there's a jwt token in the response
                let user = response.json();
                if (user && user.token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', JSON.stringify(user));
                }
            });
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
    }

    // private helper methods
    jwt(): RequestOptions {
        // create authorization header with jwt token
        let cookies = localStorage.getItem('currentUser');
        // console.log("Current User: " + cookies);
        if (cookies) {
            let currentUser = JSON.parse(cookies);
            if (currentUser && currentUser.token) {
                let headers = new Headers({ 'Authorization': 'Bearer ' + currentUser.token });
                return new RequestOptions({ headers: headers });
            }
        } else {
            this.router.navigateByUrl('/login');
        }
        return new RequestOptions();
    }

    checkAuth() {
        // remove user from local storage to log user out
        if (localStorage.getItem('currentUser')) {
            return true;
        }
        return false;
    }

    getUserId(): string {
        // create authorization header with jwt token
        let cookies = localStorage.getItem('currentUser');
        // console.log("Current User: " + cookies);
        if (cookies) {
            let currentUser = JSON.parse(cookies);
            return currentUser.id;
        }
        return "";
    }
}