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
