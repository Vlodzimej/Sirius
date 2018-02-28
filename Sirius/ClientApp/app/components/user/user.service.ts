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
