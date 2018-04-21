import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Invoice, InvoiceUpdate, Register, Item, ItemDetail, InvoiceType, Batch, Vendor } from '../../_models';
import { AlertService, ApiService, PageHeaderService, ModalService, LoadingService } from '../../_services';
import { Router } from '@angular/router';
import { FullDatePipe } from '../../_pipes';
import { Location } from '@angular/common';

@Component({
    selector: 'app-invoice-detail',
    templateUrl: './invoice.detail.component.html',
    styleUrls: [
        '../../../assets/css/accordion.css',
        '../../../assets/css/modal.css',
        '../../../assets/css/invoice/style.css'
    ]
})
export class InvoiceDetailComponent implements OnInit {
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
    public selectedRegister: Register = new Register();
    // Массив регистров текущей накладной
    public registers: Register[] = [];
    // Тип накладной
    public invoiceType: InvoiceType = new InvoiceType();
    // Общая сумма накладной
    public sum: number = 0;
    // Остатки наименования
    public batches: Batch[] = [];
    // Список поставщиков
    public vendors: Vendor[] = [];
    // Поставщк
    public vendorId: string = "";

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private apiService: ApiService,
        private alertService: AlertService,
        private pageHeaderService: PageHeaderService,
        private modalService: ModalService,
        private loadingService: LoadingService,
        private location: Location
    ) { }

    ngOnInit() {
        // Включаем визуализацию загрузки
        this.loadingService.showLoadingIcon();

        var invoiceId = this.route.snapshot.params['id'];
        // Загрузка накладной
        this.apiService.getById<Invoice>('invoice', invoiceId).subscribe(
            data => {

                // Отключаем визуализацию загрузки
                this.loadingService.hideLoadingIcon();
                this.invoice = data;
                console.log(this.invoice);

                this.vendorId = this.invoice.vendorId;

                this.registers = this.invoice.registers;
                var headerText = this.invoice.name + " от " + this.invoice.createDate;
                this.pageHeaderService.changeText(headerText);

                //Вычисление суммы для каждого регистра
                this.registers.map(r => {
                    r.sum = r.amount * r.cost
                });

                this.registers.forEach(element => {
                    this.sum += element.sum;
                });

                // Загрузка данных типа накладной
                this.apiService.getById<InvoiceType>('invoice/type/id', this.invoice.typeId).subscribe(
                    data => {
                        this.invoiceType = data;
                    },
                    error => {
                        this.alertService.serverError(error);
                    });
            },
            error => {
                this.alertService.error('Ошибка загрузки', true);
            });

        // Загрузка списка наименований
        this.apiService.getAll<Item>('item').subscribe(
            data => {
                this.items = data;
            },
            error => {
                this.alertService.serverError(error);
            }
        );

        // Загрузка списка поставщиков
        this.apiService.getAll<Vendor>('vendor').subscribe(
            data => {
                this.vendors = data;
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
        if (!this.invoice.isFixed) {
            this.register = new Register();
            this.modalService.open('modal-new-register');
        }
    }

    /** Выбор регистра в списке */
    onSelect(registerId: string) {
        if (!this.invoice.isFixed) {
            this.selectedRegister = this.registers.find(i => i.id == registerId) as Register;
        }
    }

    /** Открытие регистра для редактирования */
    onOpen() {
        if (!this.invoice.isFixed && this.selectedRegister != null) {
            this.register = this.selectedRegister;
            this.getBatchesByItem();

            this.modalService.open('modal-edit-register');
        }
    }

    /** Удаление регистра */
    onDelete() {
        if (!this.invoice.isFixed) {
            console.log("Удаление ID: " + this.selectedRegister.id);
            this.apiService.delete("register", this.selectedRegister.id).subscribe(
                data => {
                    // Удаляем регист из массива для отображения
                    var deletedRegister = this.registers.find(i => i.id == this.selectedRegister.id) as Register;
                    const i = this.registers.indexOf(deletedRegister);
                    this.registers.splice(i, 1);

                    // Высчитываем общую сумму накладной для отображения
                    this.calcSum();
                },
                error => {
                    this.alertService.serverError(error);
                });
        }
    }

    /** Добавление регистра */
    onAdd() {
        if (!this.invoice.isFixed) {
            this.apiService.getById<Item>('item', this.register.itemId).subscribe(
                data => {
                    var item = data as Item;
                    this.register.name = item.name;
                    this.register.dimension = item.dimension.name;
                    this.register.invoiceId = this.invoice.id;
                    this.apiService.create<Register>('register', this.register).subscribe(
                        data => {
                            var addedRegister: Register = data;
                            // Присваиваем id созданного регистра взятый из базы данных
                            this.register.id = addedRegister.id;
                            // Вычисление суммы для отображения
                            this.register.sum = this.register.cost * this.register.amount;
                            // Добавление нового регистра в массив отображения 
                            this.registers.push(this.register);
                            // Высчитываем общую сумму накладной для отображения
                            this.calcSum();
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
    }

    calcSum() {
        this.sum = 0;
        this.registers.map(r => this.sum += r.sum);
    }

    /** Изменение регистра */
    onChange() {
        if (!this.invoice.isFixed) {
            this.register.invoiceId = this.invoice.id;
            this.apiService.update<Register>('register', this.register.id, this.register).subscribe(
                data => {
                    this.apiService.getById<Item>('item', this.register.itemId).subscribe(
                        data => {
                            // Получаем наименование (возможно, изменённое)
                            var item: Item = data;
                            // Находим регист по id в масстве отображения
                            var changedRegister = this.registers.find(i => i.id == this.register.id) as Register;
                            // Вычисляем его индекс в масстве
                            const i = this.addedRegisters.indexOf(changedRegister);
                            // Присваиваем id накладной
                            // Присваиваем название наименования
                            this.register.name = item.name;
                            // Присваиваем название ед. измерения
                            this.register.dimension = item.dimension.name;
                            // Вычисление суммы для отображения
                            this.register.sum = this.register.cost * this.register.amount;
                            // Обновляем объект регистра в массиве
                            this.registers[i] = this.register;
                            // Высчитываем общую сумму накладной для отображения
                            this.calcSum();
                            // Закрываем модаль
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
    }

    toBack() {
        this.location.back();
    }

    /** Проведение документа */
    onFix() {
        if (!this.invoice.isFixed) {
            this.apiService.update<string>('invoice/fix', this.invoice.id).subscribe(
                data => {
                    this.invoice.isFixed = true;
                    this.alertService.success(data);
                },
                error => {
                    this.alertService.serverError(error);
                });
        }
    }

    /** Получение остатков по номенклатуре */
    getBatchesByItem() {
        this.apiService.getById<Batch[]>('register/item', this.register.itemId).subscribe(
            data => {
                this.batches = data;
            },
            error => {
                this.alertService.serverError(error);
            });

    }

    /** Изменение поставщика */
    onChangeVendor() {
        console.log(this.vendorId);
        var vendor = { vendorId: this.vendorId };
        if (this.vendorId != "") {
            var vendor = { vendorId: this.vendorId };
            this.apiService.update('invoice/vendor', this.invoice.id, vendor).subscribe(
                data => {
                    // this.alertService.success(data.vendorId);
                },
                error => {
                    this.alertService.serverError(error);
                });
        }
    }
}