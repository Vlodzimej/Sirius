import { Component, OnInit } from '@angular/core';
import { ApiService, PageHeaderService, AlertService } from '../_services';

@Component({
    selector: 'app-settings',
    templateUrl: './settings.component.html',
})
export class SettingsComponent implements OnInit {
    constructor(
        private apiService: ApiService,
        private pageHeaderService: PageHeaderService,
        private alertService: AlertService
    ) {}

    ngOnInit() {
        this.pageHeaderService.changeText('Настройки');
    }

    dbReset() {
        console.log('click');
        this.apiService.dbReset().subscribe(
            (data) => {
                this.alertService.success('База данных очищена.');
            },
            (error) => {
                this.alertService.serverError(error);
            }
        );
    }

    removeInvoices() {
        this.apiService.removeInvoices().subscribe(
            (data) => this.alertService.success('Накладные удалены'),
            (err) => this.alertService.serverError(err)
        );
    }
}
