import { Component, OnInit } from '@angular/core';
import { InvoiceType, Register } from '../../_models';
import { Router, ActivatedRoute } from '@angular/router';
import { LoadingIconComponent } from '../../_directives';
import {
    ApiService,
    AlertService,
    ModalService,
    PageHeaderService,
} from '../../_services';

@Component({
    selector: 'app-report',
    templateUrl: './report.component.html'
})
export class ReportComponent implements OnInit {

    public typeAlias: string;
    public type: InvoiceType;
    public registers: Register[] = [];
    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService,
        private pageHeaderService: PageHeaderService
    ) { }

    ngOnInit(): void {
        // Подписка на изменения параметра в маршруте
        this.route.params.subscribe(params => {
            this.typeAlias = this.route.snapshot.params['typealias'];
            // Получение данных о текущем типе накладной
            this.apiService.getById<InvoiceType>('invoice/type/alias', this.typeAlias).subscribe(
                data => {
                    this.type = data;
                    var title = this.type.name.toLowerCase();
                    this.pageHeaderService.changeText('Отчет по '+title+'у');
                    this.apiService.getById<Register[]>('register/type', this.typeAlias).subscribe(
                        data => {
                            this.registers = data;
                            console.log(this.registers);
                        },
                        error => {

                        }
                    );

                },
                error => {
                    this.alertService.serverError(error);
                });
        });
    }
}
