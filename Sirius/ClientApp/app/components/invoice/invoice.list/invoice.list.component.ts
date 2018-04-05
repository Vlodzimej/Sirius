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
    styleUrls: ['../../../assets/css/accordion.css']
})
export class InvoiceListComponent implements OnInit {
    public type: string;
    public loading: boolean = true;
    public invoices: InvoiceListItem[] = [];
    public invoice: Invoice = new Invoice();
    public selectedInvoice: Invoice;
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
        var headerText: string = "";
        this.type = this.route.snapshot.params['type'];

        switch (this.type) {
            case 'arrival':
                headerText = "Приход";

                break;
            case 'consumption':
                headerText = "Расход";
                break;

            case 'writeoff':
                headerText = "Списание";
                break;
        }

        this.pageHeaderService.changeText(headerText);

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

    onCreate() {
        var newInvoice: Invoice = new Invoice();
        newInvoice.userId = this.authenticationService.getUserId();
        this.apiService.create<Invoice>('invoice', newInvoice).subscribe(
            data => {
                this.invoice = data;
                this.router.navigateByUrl('/invoice/' + this.invoice.id);
            },
            error => {
                console.log(error);
            }
        )
    }

    onOpen(invoiceId: string) {
        this.router.navigateByUrl('/invoice/' + invoiceId);
    };

    onSelect(invoiceId: string) {
        this.selectedInvoice = this.invoices.find(i => i.id == invoiceId) as Invoice;
    }

    onDelete(invoiceId: string) {

        this.apiService.delete('invoice', invoiceId).subscribe(
            data => {
                var deletedInvoice = this.invoices.find(i => i.id == invoiceId) as Invoice;
                if (deletedInvoice.isFixed) {
                    this.alertService.error('Документ проведён. Удаление невозможно.');
                } else {
                    const i = this.invoices.indexOf(deletedInvoice);
                    this.invoices.splice(i, 1);
                    delete (this.selectedInvoice);
                }
            },
            error => {
                var e = error as Response;
                this.alertService.serverError(error);
            }
        )

    }

}