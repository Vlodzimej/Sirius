import { Component, OnInit } from '@angular/core';
import { Invoice } from '../../_models';
import { ApiService, AlertService } from '../../_services';

@Component({
    selector: 'app-arrival-invoice',
    templateUrl: './arrival.invoice.component.html'
})
export class ArrivalInvoiceComponent implements OnInit {
    public invoice: Invoice = new Invoice();
    constructor(private apiService: ApiService, private alertService: AlertService) { }

    ngOnInit() { }


    loadInvoice()
    {

    }

    createTestInvoice() 
    {

        this.invoice = new Invoice();
        this.apiService.create<Invoice>('invoice', this.invoice).subscribe(
            data => {
                console.log(this.invoice);
                this.ngOnInit();
            },
            error => {
                this.alertService.error('Ошибка записи', true);
            });
    }
}