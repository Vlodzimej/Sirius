import { Component, OnInit } from '@angular/core';
import { ApiService } from '../_services';
@Component({
    selector: 'app-settings',
    templateUrl: './settings.component.html',
})
export class SettingsComponent implements OnInit {
    constructor(private apiService: ApiService) { }

    ngOnInit() { }

    dbReset(){
        console.log("click");
        this.apiService.dbReset().subscribe(
            data => {
            console.log(data);
        },
        error => {
            console.log(error);
        });
    }
}