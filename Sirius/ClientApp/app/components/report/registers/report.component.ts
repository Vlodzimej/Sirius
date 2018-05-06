import { Component, OnInit } from '@angular/core';
import { InvoiceType, Register } from '../../_models';
import { Router, ActivatedRoute } from '@angular/router';
import { LoadingIconComponent } from '../../_directives';
import {
    ApiService,
    AlertService,
    ModalService,
    PageHeaderService,
    FilterService,
    LoadingService
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
        private pageHeaderService: PageHeaderService,
        private filterService: FilterService,
        private loadingService: LoadingService
    ) { }

    ngOnInit(): void {
        // Настройка фильтра
        this.filterService.setFilter({ item: true, date: true});
        // Подписка на изменения параметра в маршруте
        this.route.params.subscribe(params => {
            this.typeAlias = this.route.snapshot.params['typealias'];
            // Получение данных о текущем типе накладной
            this.apiService.getById<InvoiceType>('invoice/type/alias', this.typeAlias).subscribe(
                data => {
                    this.type = data;
                    var title = this.type.name.toLowerCase();
                    this.pageHeaderService.changeText('Отчет по '+title+'у');
                    this.getRegistersByFilter();
                },
                error => {
                    this.alertService.serverError(error);
                });
        });
    }

    getRegistersByFilter() {
        // Показываем иконку загрузки
        this.loadingService.showLoadingIcon();
        // Получение критериев фильтрации 
        var filter = this.filterService.getFilter();

        var params: string = "typeid="+this.type.id;
        params += filter.itemId != null ? "&itemId=" + filter.itemId : "";
        params += filter.startDate != null && typeof(filter.startDate) != 'undefined' ? "&startDate=" + filter.startDate : "";
        params += filter.finishDate != null && typeof(filter.finishDate) != 'undefined'? "&finishDate=" + filter.finishDate : "";
        this.apiService.get<Register[]>('register', params).subscribe(
            data => {
                this.registers = data;
                this.loadingService.hideLoadingIcon();
            },
            error => {
                this.alertService.serverError(error);
            });
    }
}
