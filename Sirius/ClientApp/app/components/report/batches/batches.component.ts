import { Component, OnInit } from '@angular/core';
import { Batch, BatchGroup, BatchListElement, BatchListElementType, Item, Category, Vendor } from '../../_models';
import { Filter } from '../../_extends';
import { Router, ActivatedRoute } from '@angular/router';
import { CurrencyPipe } from '../../_pipes';
import { IOption } from 'ng-select';
import { Converter } from '../../_helpers';
import { ApiService, AlertService, ModalService, PageHeaderService, LoadingService, FilterService } from '../../_services';

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
    public optionItems: Array<IOption> = [];
    public optionCategories: Array<IOption> = [];
    public optionVendors: Array<IOption> = [];
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
        private loadingService: LoadingService,
        private filterService: FilterService
    ) { }

    ngOnInit(): void {
        // На всякий случай отключаем визуализацию загрузки, так как загрузка будет происходить после нажатия на кнопку "Сформировать"
        this.loadingService.hideLoadingIcon();

        // Задаём текст заголовка
        this.pageHeaderService.changeText("Остатки");
    }

    getBatchesByFilter() {
        this.loadingService.showLoadingIcon();
        this.isReport = true;
        this.listItems = [];
        var filterString: string = "";
        var filter = this.filterService.getFilter();

        filterString += filter.categoryId != null ? "categoryId=" + filter.categoryId : "";
        filterString += filter.itemId != null ? "&itemId=" + filter.itemId : "";
        filterString += filter.vendorId != null ? "&vendorId=" + filter.vendorId : "";
        filterString += filter.startDate != null ? "&startDate=" + filter.startDate : "";
        filterString += filter.finishDate != null ? "&finishDate=" + filter.finishDate : "";

        this.apiService.getAll<BatchGroup>('register/batches?' + filterString).subscribe(
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

