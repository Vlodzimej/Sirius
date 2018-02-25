import { Component, OnInit, Inject } from '@angular/core';
import { Location } from '@angular/common';
import { Http } from '@angular/http';
import { User } from './../user/user';
import { UserService } from './../user/user.service';

@Component({
    selector: 'app-registration',
    templateUrl: './registration.component.html',
    styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
    public user: User = new User("", "");

    constructor(private location: Location, private userService: UserService) { }

    ngOnInit() { }

    // Создание пользователя
    CreateUser() {
        console.log(this.user);
        this.userService.Create(this.user);
    }

    // Назад
    Cancel() {
        this.location.back();
    }
}