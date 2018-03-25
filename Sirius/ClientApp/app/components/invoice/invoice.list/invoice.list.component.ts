import { Component, OnInit } from '@angular/core';
import { Invoice, InvoiceListItem } from '../../_models';

import {
    AuthenticationService,
    ApiService,
    AlertService,
    ModalService,
    PageHeaderService
} from '../../_services';

import { Subscription } from 'rxjs/Subscription';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';

@Component({
    selector: 'app-invoice-list',
    templateUrl: './invoice.list.component.html',
})
export class InvoiceListComponent implements OnInit {
    public loading: boolean = true;
    public invoices: InvoiceListItem[] = [];
    public invoice: Invoice = new Invoice();
    private routeSubscription: Subscription;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private authenticationService: AuthenticationService,
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService,
        private pageHeaderService: PageHeaderService) {

        this.routeSubscription = route.params.subscribe(params => this.invoice = params['id']);
    }
    ngOnInit() {
        this.pageHeaderService.changeText("Накладные");
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

    createNewInvoice() {
        var newInvoice: Invoice = new Invoice();
        newInvoice.userId  = this.authenticationService.getUserId();
        this.apiService.create<Invoice>('invoice', newInvoice).subscribe(
            data => {
                this.invoice = data;
                this.router.navigateByUrl('/invoice/' + this.invoice.id);
                //this.tempInvoice = data;
                //const i = this.invoices.indexOf(newInvoice);
                //this.invoices.unshift(this.tempInvoice);
                //this.users.splice(i, 1); - удаление
            },
            error => {
                console.log(error);
            }
        )
    }

    invoiceListItemClick(invoiceId: string) {
        this.router.navigateByUrl('/invoice/' + invoiceId);
    };

}