import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Invoice } from '../../_models';
import { View } from '../../_interfaces';
import { AlertService, ApiService } from '../../_services';

@Component({
    selector: 'app-invoice-detail',
    templateUrl: './invoice.detail.component.html',
})
export class InvoiceDetailComponent implements OnInit, View {
    loading = true;
    public invoice: Invoice;
    constructor(
        private route: ActivatedRoute,
        private apiService: ApiService,
        private alertService: AlertService
    ) { }

    ngOnInit() { 
        var invoiceId = this.route.snapshot.params['id'];
        this.apiService.getById<Invoice>('invoice', invoiceId).subscribe(
            data => {
                this.invoice = data;
                console.log(this.invoice);
                this.loading = false;
            },
            error => {
                this.alertService.error('Ошибка загрузки', true);
                this.loading = false;
            });
    }


}