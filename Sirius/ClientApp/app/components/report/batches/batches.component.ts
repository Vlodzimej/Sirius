import { Component, OnInit } from '@angular/core';
import { Batch, BatchGroup, BatchListElement, BatchListElementType, Item, Category, Vendor } from '../../_models';
import { Filter } from '../../_extends';
import { Router, ActivatedRoute } from '@angular/router';
import { Converter } from '../../_helpers';
import { ApiService, AlertService, ModalService, PageHeaderService, LoadingService, FilterService } from '../../_services';
import { LocalizedCurrencyPipe } from '../../_pipes';

@Component({
    selector: 'app-batches',
    templateUrl: './batches.component.html',
    styleUrls: [
        '../../../assets/css/accordion.css',
        '../../../assets/css/modal.css'
    ]
})
export class BatchesComponent implements OnInit {

    public batchGroups: BatchGroup[] = [];
    public batches: Batch[] = [];
    public listItems: BatchListElement[] = [];
    public filter: Filter = new Filter();
    // Признак первого сформированного очета
    public isReport: boolean = false;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService,
        private pageHeaderService: PageHeaderService,
        public loadingService: LoadingService,
        private filterService: FilterService
    ) { }

    ngOnInit(): void {
        // Устанавливаем поля фильтра
        this.filterService.setFilter({ category: true, item: true, isDynamic: true });
        // Очистка полей фильтра
        this.filterService.cleanFilter();
        // На всякий случай отключаем визуализацию загрузки, так как загрузка будет происходить после нажатия на кнопку "Сформировать"
        this.loadingService.hideLoadingIcon();

        // Задаём текст заголовка
        this.pageHeaderService.changeText("Остатки");
    }

    getBatchesByFilter() {
        this.loadingService.showLoadingIcon();
        this.isReport = true;
        this.listItems = [];
        var params: string = "";

        // Получение критериев фильтрации 
        var filter = this.filterService.getFilter();

        params += filter.categoryId != null ? "categoryId=" + filter.categoryId : "";
        params += filter.itemId != null ? "&itemId=" + filter.itemId : "";
        params += filter.vendorId != null ? "&vendorId=" + filter.vendorId : "";
        params += filter.startDate != null ? "&startDate=" + filter.startDate : "";
        params += filter.finishDate != null ? "&finishDate=" + filter.finishDate : "";
        params += filter.isDynamic == true ? "&isDynamic=true" : "";

        this.apiService.get<BatchGroup[]>('register/batches', params).subscribe(
            data => {
                // Отключаем визуализацию загрузки
                this.loadingService.hideLoadingIcon();
                // Получаем структурированные данные и создаём сквозной массив для формирования таблицы
                this.batchGroups = data;
                this.batchGroups.forEach(element => {
                    if (element.batches.length > 0) {
                        var entry: BatchListElement = { text: element.name, cost: 0, amount: 0, sum: 0, type: BatchListElementType.Head };
                        this.listItems.push(entry);
                        var amount: number = 0; // Для подсчета общего кол-ва 
                        var sum: number = 0; // Общая сумма для одного наименования
                        element.batches.forEach(batch => {
                            batch.sum = batch.cost * batch.amount;
                            amount += batch.amount;
                            sum += batch.sum;
                            var entry: BatchListElement = { text: "", cost: batch.cost, amount: batch.amount, sum: batch.sum, type: BatchListElementType.Batch };
                            this.listItems.push(entry);
                        });
                        // Закрывающий элемент группы в таблице
                        var lastElement: BatchListElement = { text: 'Всего: ' + amount, cost: 0, amount: 0, sum: sum, type: BatchListElementType.Footer };
                        this.listItems.push(lastElement);
                    }
                });
            },
            error => {
                this.alertService.serverError(error);
            });
    }
}

