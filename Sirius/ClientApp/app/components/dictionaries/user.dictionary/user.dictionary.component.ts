import { Component, OnInit } from '@angular/core';
import { User } from '../../_models';
import { UserService, AlertService, ApiService, PageHeaderService, AuthenticationService } from '../../_services';
import { DatePipe } from '@angular/common'

@Component({
    selector: 'app-user-dictionary',
    templateUrl: './user.dictionary.component.html',
    providers: [DatePipe]
})
export class UserDictionaryComponent implements OnInit {
    public users: User[] = [];
    public currentUserId: string;

    constructor(
        private userService: UserService, 
        private alertService: AlertService, 
        private apiService: ApiService, 
        private pageHeaderService: PageHeaderService, 
        private authenticationService: AuthenticationService,
        public datepipe: DatePipe
    ) { }

    ngOnInit() {
        this.pageHeaderService.changeText('Пользователи');
        this.currentUserId = this.authenticationService.getUserId();
        this.userService.getAll()
            .subscribe(
                data => {
                    this.users = data;
                },
                error => {
                    this.alertService.error('Ошибка загрузки', true);
                });
    }

    removeUser(userId: string) {
        this.userService.delete(userId).subscribe(
            data => {
                //const filtered = this.users.filter((user: User, index) => user.id == userId);
                //const i = this.users.indexOf(filtered[0]);
                //this.users.splice(i, 1);
                this.ngOnInit();
            },
            error => {
                this.alertService.error('Ошибка удаления', true);
            });
    }

    onChangeUserStatus(userId: string, status: boolean = false) {
        this.apiService.update('user/status', userId+'?isConfirmed='+status, '').subscribe(
            data => {
                this.ngOnInit();
                this.alertService.success(data);
            }, 
            error => {
                this.alertService.error(error._body);
            });
    }
}
