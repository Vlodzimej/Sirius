import { Component, OnInit } from '@angular/core';
import { InvoiceType, Register } from '../../_models';
import { Router, ActivatedRoute } from '@angular/router';
import { LoadingIconComponent } from '../../_directives';
import { ApiService, AlertService, ModalService, PageHeaderService, FilterService, LoadingService } from '../../_services';
import { LocalizedCurrencyPipe } from '../../_pipes';

@Component({
    selector: 'app-report',
    templateUrl: './report.component.html'
})
export class ReportComponent implements OnInit {

    public typeAlias: string;
    public type: InvoiceType;
    public registers: Register[] = [];

    public sum: number = 0;
    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService,
        private pageHeaderService: PageHeaderService,
        private filterService: FilterService,
        public loadingService: LoadingService
    ) { }

    ngOnInit(): void {
        // Настройка фильтра
        this.filterService.setFilter({ category: true, item: true, date: true });
        // Очистка полей фильтра
        this.filterService.cleanFilter();        
        // Подписка на изменения параметра в маршруте
        this.route.params.subscribe(params => {
            this.typeAlias = this.route.snapshot.params['typealias'];
            // Получение данных о текущем типе накладной
            this.apiService.getById<InvoiceType>('invoice/type/alias', this.typeAlias).subscribe(
                data => {
                    this.type = data;
                    /** Формируем заголовок */
                    var title = "";
                    if (this.typeAlias == 'writeoff') {
                        title = 'Отчет по списанию';
                    } else {
                        var title = this.type.name.toLowerCase();
                        title = 'Отчет по ' + title + 'у';
                    }
                    this.pageHeaderService.changeText(title);
                    /** */
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

        var params: string = "typeid=" + this.type.id;
        params += filter.categoryId != null ? "&categoryId=" + filter.categoryId : "";
        params += filter.itemId != null ? "&itemId=" + filter.itemId : "";
        params += filter.startDate != null && typeof (filter.startDate) != 'undefined' ? "&startDate=" + filter.startDate : "";
        params += filter.finishDate != null && typeof (filter.finishDate) != 'undefined' ? "&finishDate=" + filter.finishDate : "";
        this.apiService.get<Register[]>('register', params).subscribe(
            data => {
                this.registers = data;

                this.sum = 0;
                this.registers.forEach(x => {
                    this.sum += x.sum;
                });

                this.loadingService.hideLoadingIcon();
            },
            error => {
                this.alertService.serverError(error);
            });
    }
}
