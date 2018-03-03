import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from './../_services';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

    constructor(private authenticationService:AuthenticationService) { }

    ngOnInit() { }

}
