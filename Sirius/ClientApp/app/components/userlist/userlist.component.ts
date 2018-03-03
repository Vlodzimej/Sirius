import { Component, OnInit } from '@angular/core';
import { UserService } from './../_services';
import { User } from '../_models';
import { AlertService } from './../_services';
@Component({
    selector: 'userlist',
    templateUrl: './userlist.component.html'
})
export class UserListComponent implements OnInit {
    public users: User[] = [];

    constructor(private userService: UserService, private alertService: AlertService) { }

    ngOnInit() {
        this.userService.getAll()
            .subscribe(
            data => {
                this.users = data;
            },
            error => {
                this.alertService.error('Ошибка загрузки', true);
            });
    }
}
