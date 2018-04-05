import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Invoice, InvoiceUpdate, Register, Item, ItemDetail } from '../../_models';
import { View } from '../../_interfaces';
import { AlertService, ApiService, PageHeaderService, ModalService } from '../../_services';
import { Router } from '@angular/router';

@Component({
    selector: 'app-invoice-detail',
    templateUrl: './invoice.detail.component.html',
    styleUrls: ['../../../assets/css/accordion.css', '../../../assets/css/modal.css']
})
export class InvoiceDetailComponent implements OnInit, View {
    loading = true;
    // Список существующих наименований
    public items: Item[] = [];
    // Текущая накладная
    public invoice: Invoice = new Invoice;
    // Массив индектификаторов удаленных регистров
    public deletedRegisterIds: string[] = [];
    // Массив добавленных регистров
    public addedRegisters: Register[] = [];
    // Массив изменённых регистров
    public changedRegisters: Register[] = [];
    // Добавляемый регистр
    public register: Register = new Register();
    // Выбранный регистр
    public selectedRegister: Register;
    // Массив регистров текущей накладной
    public registers: Register[] = [];

    constructor(
        private router: Router,
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

    /** Нажатие кнопки добавления нового регистра */
    onCreate() {
        this.register = new Register();
        this.modalService.open('modal-new-register');
    }

    /** Выбор регистра в списке */
    onSelect(registerId: string) {
        this.selectedRegister = this.registers.find(i => i.id == registerId) as Register;
        console.log(this.selectedRegister);
    }

    /** Открытие регистра для редактирования */
    onOpen() {
        if (this.selectedRegister != null) {
            this.register = this.selectedRegister;
            this.modalService.open('modal-edit-register');
        }
    }

    /** Удаление регистра */
    onDelete() {
        console.log("Удаление ID: " + this.selectedRegister.id);
        this.apiService.delete("register", this.selectedRegister.id).subscribe(
            data => {
                // Удаляем регист из массива для отображения
                var deletedRegister = this.registers.find(i => i.id == this.selectedRegister.id) as Register;
                const i = this.registers.indexOf(deletedRegister);
                this.registers.splice(i, 1);
            },
            error => {
                this.alertService.serverError(error);
            });
    }

    /** Добавление регистра */
    onAdd() {
        this.apiService.getById<Item>('item', this.register.itemId).subscribe(
            data => {
                var item = data as Item;
                this.register.item = item;
                this.register.invoiceId = this.invoice.id;
                this.apiService.create<Register>('register', this.register).subscribe(
                    data => {
                        var addedRegister: Register = data;
                        this.register.id = addedRegister.id;
                        // Добавление нового регистра в массив отображения */
                        this.registers.push(this.register);
                        this.modalService.close('modal-new-register');
                    },
                    error => {
                        this.alertService.serverError(error);
                    });
            },
            error => {
                this.alertService.serverError(error);
            });
    }

    /** Изменение регистра */
    onChange() {
        this.apiService.update<Register>('register', this.register.id, this.register).subscribe(
            data => {
                this.apiService.getById<Item>('item', this.register.itemId).subscribe(
                    data => {
                        var item: Item = data;
                        var changedRegister = this.registers.find(i => i.id == this.register.id) as Register;
                        const i = this.addedRegisters.indexOf(changedRegister);
                        this.register.item = item;
                        this.registers[i] = this.register;
                        this.modalService.close('modal-edit-register');
                    },
                    error => {
                        this.alertService.serverError(error);
                    });
            },
            error => {
                this.alertService.serverError(error);
            });
    }

    /** Проведение документа */
    onFix() {
        this.apiService.update<string>('invoice/fix', this.invoice.id).subscribe(
            data => {
                this.invoice.isFixed = true;
                this.alertService.success(data);
            },
            error => {
                this.alertService.serverError(error);
            }
        )
    }
}