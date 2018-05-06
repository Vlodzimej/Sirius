import { Component, OnInit } from '@angular/core';
import { Invoice, InvoiceType } from '../_models';
import { ApiService, AuthenticationService, AlertService, PageHeaderService } from '../_services';
import { Router } from '@angular/router';
@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

    constructor(
        private apiService: ApiService,
        private authenticationService: AuthenticationService,
        private alertService: AlertService,
        private router: Router,
        private pageHeaderService: PageHeaderService
    ) { }

    ngOnInit(){
        this.pageHeaderService.changeText("Главное меню");
    }

    onCreate() {
        this.apiService.getById<InvoiceType>('invoice/type/alias', 'consumption').subscribe(
            data => {
                var invoiceType: InvoiceType = data;
                var newInvoice: Invoice = new Invoice();

                newInvoice.typeId = invoiceType.id;
                newInvoice.factor = invoiceType.factor;
                newInvoice.userId = this.authenticationService.getUserId();

                this.apiService.create<Invoice>('invoice', newInvoice).subscribe(
                    data => {
                        var invoice: Invoice = data;
                        this.router.navigateByUrl('/invoice/' + invoice.id);
                    },
                    error => {
                        this.alertService.serverError(error);
                    }
                )
            },
            error => {
                this.alertService.serverError(error);
            });
    }

}
