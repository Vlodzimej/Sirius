import { Injectable, Inject } from '@angular/core';
import { User } from './user';
import { Http } from '@angular/http';

@Injectable()
export class UserService {

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) { }

    Create(user: User) {
        this.http.post(this.baseUrl + 'api/user', user).subscribe(result => {
           console.log(result.text);
        }, error => console.error(error));
    }
}