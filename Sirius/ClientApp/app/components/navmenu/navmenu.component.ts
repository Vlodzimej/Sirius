import { Component } from '@angular/core';
import { AuthenticationService } from '../_services';

@Component({
    selector: 'app-nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css'],
})
export class NavMenuComponent {
    isAdmin: boolean = false;
    constructor(private authenticationService: AuthenticationService) {}
    ngOnInit() {
        this.authenticationService.checkAdmin().subscribe(res => {
            this.isAdmin = res.json();
        });
    }
}
