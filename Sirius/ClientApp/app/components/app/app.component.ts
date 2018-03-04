import { Component, OnInit } from '@angular/core';
import { RouterModule, Router, NavigationStart, NavigationEnd } from '@angular/router';
import { AuthenticationService } from './../_services';
@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

    public showNavBar: boolean = false;
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService) {

    }

    ngOnInit() {
        this.router.events.subscribe((event) => {
            if (event instanceof NavigationEnd) {
                this.showNavBar = this.authenticationService.checkAuth();
                if(this.router.url == '/login') {
                    this.showNavBar = false;
                }
            }
        });
    }

}
