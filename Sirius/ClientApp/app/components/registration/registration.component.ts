import { Component, OnInit, Inject } from '@angular/core';
import { Location } from '@angular/common';
import { Http } from '@angular/http';
import { User } from './../user/user';
import { UserService } from './../user/user.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-registration',
    templateUrl: './registration.component.html',
    styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

    public user: User = new User();
    public state: string = "in-progress";
    public success: boolean = false;
    public fail: boolean = false;

    constructor(private location: Location, private userService: UserService, private router: Router) { }

    ngOnInit() { }

    // Создание пользователя
    CreateUser() {
        this.userService.Create(this.user).subscribe(
            response => {
                this.state = "success";
                console.log(response.json());
            },
            error => {
                this.state = "fail";
                console.error(`Error: ${error.status} ${error.statusText}`)
            }
        );
    }

    // Назад
    Cancel() {
        this.location.back();
    }
}