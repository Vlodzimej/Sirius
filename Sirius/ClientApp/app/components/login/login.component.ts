import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AlertService, AuthenticationService, PageHeaderService, ApiService } from './../_services';

@Component({
    templateUrl: 'login.component.html'
})

export class LoginComponent implements OnInit {
    model: any = {};
    loading = false;
    returnUrl: string = "";
    userAmount: number = 0;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private alertService: AlertService,
        private pageHeaderService: PageHeaderService,
        private apiService: ApiService) { }

    ngOnInit() {
        // Получение кол-ва зарегистрированных пользователей
        this.apiService.get<number>('user/amount', "").subscribe(
            data => {
                this.userAmount = data;
                    if(this.userAmount == 0) {
                    this.alertService.error("В базе данных отстуствует информация о пользователях. Необходимо зарегистрировать первую учетную запись.");
                }
            },
            error => {
                this.alertService.serverError(error);
            });
        // Сброс аутентификации 
        this.authenticationService.logout();
        // Заголовок
        this.pageHeaderService.changeText('Вход');
        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    login() {
        this.loading = true;
        this.authenticationService.login(this.model.username, this.model.password)
            .subscribe(
                data => {
                    this.router.navigate([this.returnUrl]);
                },
                error => {
                    this.alertService.error(error._body);
                    this.loading = false;
                });
    }
}
