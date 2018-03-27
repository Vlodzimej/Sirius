import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Invoice, Register, Item, ItemDetail } from '../../_models';
import { View } from '../../_interfaces';
import { AlertService, ApiService, PageHeaderService, ModalService } from '../../_services';

@Component({
    selector: 'app-invoice-detail',
    templateUrl: './invoice.detail.component.html',
    styleUrls: ['../../../assets/css/accordion.css', '../../../assets/css/modal.css']
})
export class InvoiceDetailComponent implements OnInit, View {
    loading = true;
    public items: Item[] = [];
    public invoice: Invoice = new Invoice;
    public addedRegisters: Register[] = [];
    public register: Register = new Register();
    public selectedRegister: Register;
    public registers: Register[] = [];
    constructor(
        private route: ActivatedRoute,
        private apiService: ApiService,
        private alertService: AlertService,
        private pageHeaderService: PageHeaderService,
        private modalService: ModalService
    ) { }

    ngOnInit() {
        var invoiceId = this.route.snapshot.params['id'];
        this.apiService.getById<Invoice>('invoice', invoiceId).subscribe(
            data => {
                this.invoice = data;
                this.registers = this.invoice.registers;

                this.loading = false;
                var headerText = this.invoice.name + " от " + this.invoice.createDate;
                this.pageHeaderService.changeText(headerText);
            },
            error => {
                this.alertService.error('Ошибка загрузки', true);
                this.loading = false;
            });

        this.apiService.getAll<Item>('item').subscribe(
            data => {
                this.items = data;
                console.log(this.items);
            },
            error => {
                this.alertService.serverError(error);
            }
        );
    }

    /**Открытие модального окна  */
    onOpenModal(id: string) {
        this.modalService.open(id);
    }

    /**Закрытие модального окна */
    onCloseModal(id: string) {
        this.modalService.close(id);
    }
    // Нажатие кнопки добавления нового регистра
    onCreate() {
        this.register = new Register();
        this.modalService.open('modal-new-register');
    }

    addRegister() {

        this.apiService.getById<Item>('item', this.register.itemId).subscribe(
            data => {
                var item = data as Item;
                this.register.item = item;
                this.register.invoiceId = this.invoice.id;
                this.apiService.create<Register>('register', this.register).subscribe(
                    data => {
                        // Добавление регистра в список для отображения
                        this.registers.push(this.register);
                        // Добавление регистра в список новых регистров для добавления в базу данных
                        this.addedRegisters.push(this.register);
                        this.modalService.close('modal-new-register');
                        console.log(data);
                    },
                    error => {
                        this.alertService.serverError(error);
                    });
            },
            error => {
                this.alertService.serverError(error);
            }
        );

    }

    onSave() {
        /*
        this.apiService.create<Register[]>('register', this.addedRegisters).subscribe(
            data => {
                console.log(data);
            },
            error => {
                console.log(error);
            }
        );*/
        this.invoice.registers = this.registers;
        console.log(this.invoice);
        this.apiService.update<Invoice>('invoice', this.invoice.id, this.invoice).subscribe(
            data => {
                console.log(data);
            },
            error => {
                console.log(error);
            }
        );
    }

}