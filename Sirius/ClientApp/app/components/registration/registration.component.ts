import { Component, OnInit, Inject } from '@angular/core';
import { Location } from '@angular/common';
import { User } from './../user/user';
import { UserService } from './../user/user.service';
import { PageStates } from './../states/page.states';

@Component({
    selector: 'app-registration',
    templateUrl: './registration.component.html',
    styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

    public user: User = new User();
    public state: PageStates = PageStates.InProgress;

    constructor(private location: Location, private userService: UserService) { }

    ngOnInit() { }

    // Создание пользователя
    CreateUser() {
        
        this.userService.Create(this.user).subscribe(
            response => {
                this.state = PageStates.Success;
                console.log(response.json());
            },
            error => {
                this.state = PageStates.Failed;
                console.error(`Error: ${error.status} ${error.statusText}`);
            }
        );
    }

    // Назад
    Cancel() {
        this.location.back();
    }
}