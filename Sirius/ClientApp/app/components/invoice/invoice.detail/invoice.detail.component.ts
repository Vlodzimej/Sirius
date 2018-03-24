import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Invoice, Register } from '../../_models';
import { View } from '../../_interfaces';
import { AlertService, ApiService, PageHeaderService } from '../../_services';

@Component({
    selector: 'app-invoice-detail',
    templateUrl: './invoice.detail.component.html',
    styleUrls: [
        '../../../assets/css/accordion.css'
    ]
})
export class InvoiceDetailComponent implements OnInit, View {
    loading = true;
    public invoice: Invoice = new Invoice;
    public registers: Register[] = [];
    constructor(
        private route: ActivatedRoute,
        private apiService: ApiService,
        private alertService: AlertService,
        private pageHeaderService: PageHeaderService
    ) { }

    ngOnInit() {
        var invoiceId = this.route.snapshot.params['id'];
        this.apiService.getById<Invoice>('invoice', invoiceId).subscribe(
            data => {
                this.invoice = data;
                console.log(this.invoice);
                this.loading = false;

                var headerText = this.invoice.name + " от " + this.invoice.createDate;
                this.pageHeaderService.changeText(headerText);
                

            },
            error => {
                this.alertService.error('Ошибка загрузки', true);
                this.loading = false;
            });
    }

    addRegister() {
        var newRegister = new Register();
        this.registers.push(newRegister);
    }

}