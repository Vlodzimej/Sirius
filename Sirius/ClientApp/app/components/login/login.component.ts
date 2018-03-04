import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AlertService, AuthenticationService } from './../_services';

@Component({
    templateUrl: 'login.component.html'
})

export class LoginComponent implements OnInit {
    model: any = {};
    loading = false;
    returnUrl: string = "";

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private alertService: AlertService) { }

    ngOnInit() {
        // reset login status
        this.authenticationService.logout();

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
                    this.alertService.error('Неверный логин или пароль');
                    this.loading = false;
                });
    }
}


/*
import { Component } from '@angular/core';
import { User } from './../user/user';
import { UserService } from './../user/user.service';
import { PageStates } from './../states/page.states';
import { Router } from '@angular/router';

@Component({
    selector: 'login',
    templateUrl: './login.component.html'
})
export class LoginComponent
{
    public user: User = new User();
    public state: PageStates = PageStates.InProgress;

    constructor(private userService: UserService, private router: Router) { }

    Login() {
        console.log(this.user);
        this.userService.Login(this.user).subscribe(
            response => {
                this.state = PageStates.Success;
                this.router.navigateByUrl('/home');
                console.log(response.json());
            
            },
            error => {
                switch(error.status)
                {
                    case 400: this.state = PageStates.Failed; break;
                    case 404: this.state = PageStates.ServerError; break;
                }
                console.error(`Error: ${error.status} ${error.statusText}`);
            }
        );
    }
}
*/