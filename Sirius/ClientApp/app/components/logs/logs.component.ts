import { Component, OnInit } from '@angular/core';
import { ApiService, LoadingService, PageHeaderService } from '../_services';
import { Log } from '../_models/log';

@Component({
    templateUrl: 'logs.component.html',
})
export class LogsComponent implements OnInit {

    logs: Array<Log> = [];

    constructor(private apiService: ApiService, public loadingService: LoadingService, private pageHeaderService: PageHeaderService) { }

    ngOnInit() {
        this.pageHeaderService.changeText("Журнал действий");
        this.loadingService.showLoadingIcon();
        this.apiService
            .getAll<Log>('log')
            .subscribe(res => {
                this.loadingService.hideLoadingIcon();
                this.logs = [...res].reverse();
            });
    }
}
