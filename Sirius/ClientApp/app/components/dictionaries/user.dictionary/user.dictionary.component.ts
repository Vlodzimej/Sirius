import { Component, OnInit } from '@angular/core';
import { User } from '../../_models';
import { UserService, AlertService } from '../../_services';
import { DatePipe } from '@angular/common'

@Component({
    selector: 'app-user-dictionary',
    templateUrl: './user.dictionary.component.html',
    providers: [DatePipe]
})
export class UserDictionaryComponent implements OnInit {
    public users: User[] = [];

    constructor(private userService: UserService, private alertService: AlertService, public datepipe: DatePipe) { }

    ngOnInit() {
        this.userService.getAll()
            .subscribe(
                data => {
                    this.users = data;
                    console.log(this.users);
                },
                error => {
                    this.alertService.error('Ошибка загрузки', true);
                });
    }

    removeUser(userId: any) {
        this.userService.delete(userId).subscribe(
            data => {
                const filtered = this.users.filter((user: User, index) => user.id == userId);
                const i = this.users.indexOf(filtered[0]);
                this.users.splice(i, 1);
            },
            error => {
                this.alertService.error('Ошибка удаления', true);
            });
    }
}
