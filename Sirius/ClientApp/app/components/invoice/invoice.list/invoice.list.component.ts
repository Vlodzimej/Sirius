import { Component, OnInit } from '@angular/core';
import { Invoice, InvoiceListItem, InvoiceType } from '../../_models';

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
    styleUrls: ['../../../assets/css/accordion.css', '../../../assets/css/invoice/style.css']
})
export class InvoiceListComponent implements OnInit {
    public typeAlias: string;
    public type: InvoiceType;
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


        this.route.params.subscribe(params => {
            this.typeAlias = this.route.snapshot.params['typealias'];

        this.apiService.getById<InvoiceType>('invoice/type/alias', this.typeAlias).subscribe(
            data => {
                this.type = data;
                this.pageHeaderService.changeText(this.type.name);

                this.apiService.getById<InvoiceListItem[]>('invoice/type', this.type.id).subscribe(
                    data => {
                        this.invoices = data;
                        this.loading = false;
                    },
                    error => {
                        this.alertService.serverError(error);
                        this.loading = false;
                    });

            },
            error => {
                this.alertService.serverError(error);
            });
        });

    }

    onCreate() {

        var newInvoice: Invoice = new Invoice();

        newInvoice.typeId = this.type.id;
        newInvoice.userId = this.authenticationService.getUserId();
        newInvoice.factor = this.type.factor;

        this.apiService.create<Invoice>('invoice', newInvoice).subscribe(
            data => {
                this.invoice = data;
                this.router.navigateByUrl('/invoice/' + this.invoice.id);
            },
            error => {
                this.alertService.serverError(error);
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
                this.alertService.serverError(error);
            }
        )

    }

}