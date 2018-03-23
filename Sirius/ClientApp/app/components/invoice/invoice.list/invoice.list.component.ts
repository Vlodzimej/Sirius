import { Component, OnInit } from '@angular/core';
import { Invoice, InvoiceListItem } from '../../_models';
import { ApiService, AlertService, ModalService } from '../../_services';
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
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService) {

        this.routeSubscription = route.params.subscribe(params => this.invoice = params['id']);
    }
    ngOnInit() {
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

    invoiceListItemClick(invoiceId: string) {
        this.router.navigateByUrl('/invoice/'+invoiceId);
    };

}