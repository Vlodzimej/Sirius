import { Component, OnInit } from '@angular/core';
import { PageHeaderService, ApiService, LoadingService } from '../../_services';
import { ReportItem } from '../../_models/report_item';

@Component({
    selector: 'app-common',
    templateUrl: './common.component.html',
    styleUrls: ['./common.component.css'],
})
export class CommonReportComponent implements OnInit {
    reportItems: Array<ReportItem> = [];
    hasReport = false;
    constructor(
        private pageHeaderService: PageHeaderService,
        private apiService: ApiService,
        private loadingService: LoadingService
    ) {}

    ngOnInit() {
        this.pageHeaderService.changeText('Общий отчет');
        this.hasReport = false;
    }

    getReport() {
        this.loadingService.showLoadingIcon();
        this.apiService
            .get<ReportItem[]>('report/getcommonreport', null)
            .subscribe((data) => {
                this.reportItems = data;
                this.loadingService.hideLoadingIcon();
                this.hasReport = true;
            });
    }

    onGetExcelReport() {
        let link = document.createElement('a');
        link.setAttribute('href',`./api/report/getexcelreport`);
        link.setAttribute('target',`_blank`);
        link.click();
    }
}
