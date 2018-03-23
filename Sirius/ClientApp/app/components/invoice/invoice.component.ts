import { Component, OnInit } from '@angular/core';
import { Invoice, InvoiceListItem } from '../_models';
import { ApiService, AlertService, ModalService } from '../_services';
import { View } from '../_interfaces';
import { Router, ActivatedRoute, Params, Data } from '@angular/router';

enum Mode { List = 1, Detail = 2 };

@Component({
    selector: 'app-invoice',
    templateUrl: './invoice.component.html',
})
export class InvoiceComponent implements OnInit, View {
    // Метка процесса загрузки данных представления
    loading = true;
    // Режим представления (List(1) - список, Detail(2) - просмотр накладной)
    public mode: Mode = 1;
    // Псевдоним типа накладной
    public alias: string;
    // Тип накладной
    public typeId: string;
    // Идентификатор текущей накладной
    public currentInvoiceId: string;
    // Коэффициент  определяющий действие регистров текущей накладной при её проведении ((1) - приход; (-1) - расход)
    public factor: number;
    // Список накладных
    public invoices: InvoiceListItem[] = [];
    // Текущая накладная
    public invoice: Invoice = new Invoice();
    public  id: any;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService,) { }

    ngOnInit() {

        this.id = this.route.snapshot.params['id'];
        console.log(this.id);
        // this.route.snapshot.data['type'];

        this.apiService.getAll<InvoiceListItem>('invoice').subscribe(
            data => {

                this.invoices = data;
                console.log(this.invoices);
                this.loading = false;
            },
            error => {
                this.alertService.error('Ошибка загрузки', true);
                this.loading = false;
            });
    }

}