import { Component, OnInit } from '@angular/core';

import { FormControl } from '@angular/forms';

import { ActivatedRoute } from '@angular/router';
import { Invoice, InvoiceUpdate, Register, Item, ItemDetail, InvoiceType, Batch, Vendor, Category } from '../../_models';
import { AlertService, ApiService, PageHeaderService, ModalService, LoadingService } from '../../_services';
import { Router } from '@angular/router';
import { FullDatePipe } from '../../_pipes';
import { Location } from '@angular/common';

// Классы для работы с выпадающим списком
import { IOption } from 'ng-select';
import { Converter } from '../../_helpers';
import { FormGroup } from '@angular/forms';

import { ModalType } from '../../_extends';

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
    public selectedRegister: Register;
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
    // Поставщик
    public vendorId: string = "";

    public optionItems: Array<IOption> = [];
    public optionCategories: Array<IOption> = [];
    public optionBatches: Array<IOption> = [];

    public form: FormGroup;
    
    // Категория добавляемой/изменяемой позиции
    public categoryId: string = "";
    
    /**
     *  Костыль для привязки значения цены регистра (для определения остатка) к списку ng-select, так как number принимать мы не хотим, приходится перед
     * открытием регистра на редактирование переводить значение цены в строку и назначать переменной registerCost
    */
    public registerCost: string = "";

    public modal: ModalType = new ModalType();
    private newRegisterModal: ModalType;
    private editRegisterModal: ModalType;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private apiService: ApiService,
        private alertService: AlertService,
        private pageHeaderService: PageHeaderService,
        private modalService: ModalService,
        private loadingService: LoadingService,
        private location: Location
    ) {
        this.newRegisterModal = {
            type: 'new',
            title: 'Новая позиция',
            submit: 'Добавить'
        }

        this.editRegisterModal = {
            type: 'edit',
            title: 'Редактирование позиции',
            submit: 'Изменить'
        }
    }

    ngOnInit() {
        // Включаем визуализацию загрузки
        this.loadingService.showLoadingIcon();
        /*
                this.form = new FormGroup({
                    character: new FormControl('', Validators.required)
                });*/

        var invoiceId = this.route.snapshot.params['id'];
        // Загрузка накладной
        this.apiService.getById<Invoice>('invoice', invoiceId).subscribe(
            data => {

                // Отключаем визуализацию загрузки
                this.loadingService.hideLoadingIcon();
                this.invoice = data;

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

    /**
     * Событие: Открытие модального окна
      */
    onOpenModal(id: string) {
        this.modalService.open(id);
    }

    /**
     * Событие: Закрытие модального окна
     */
    onCloseModal(id: string) {
        this.modalService.close(id);
    }

    /** 
     * Событие: Нажатие кнопки добавления нового регистра
     */
    onCreate() {
        if (!this.invoice.isFixed) {
            this.categoryId = "";
            // Загрузка списка категорий
            this.getAllCategories();
            delete(this.register);
            this.register = new Register();
            this.modal = this.newRegisterModal;
            this.modalService.open('modal-register');
        }
    }

    /** 
     * Событие: Выбор регистра в списке
     */
    onSelect(registerId: string) {
        if (!this.invoice.isFixed) {
            this.selectedRegister = this.registers.find(i => i.id == registerId) as Register;
        }
    }

    /** 
     * Событие: Открытие регистра для редактирования
     */
    onOpen() {
        
        if (!this.invoice.isFixed && this.selectedRegister != null) {
            this.categoryId = "";
            this.register = this.selectedRegister;
            this.registerCost = this.register.cost.toString();
            // Загрузка списка категорий
            this.getAllCategories();
            this.getAllItems();
            this.getBatchesByItem();
            
            this.modal = this.editRegisterModal;
            this.modalService.open('modal-register');
        }
        console.log(this.registerCost);
    }

    /** 
     * Событие: Удаление регистра
     */
    onDelete() {
        if (!this.invoice.isFixed && this.selectedRegister != null) {
            console.log("Удаление ID: " + this.selectedRegister.id);
            this.apiService.delete("register", this.selectedRegister.id).subscribe(
                data => {
                    // Удаляем регист из массива для отображения
                    var deletedRegister = this.registers.find(i => i.id == this.selectedRegister.id) as Register;
                    const i = this.registers.indexOf(deletedRegister);
                    this.registers.splice(i, 1);

                    // Высчитываем общую сумму накладной для отображения
                    this.calcSum();

                    // Удаляем выбранный регистр из памяти
                    delete (this.selectedRegister);
                },
                error => {
                    this.alertService.serverError(error);
                });
        }
    }

    /** 
     * Событие: Добавление регистра
     */
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
                            this.modalService.close('modal-register');
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

    /** 
     * Событие: Изменение регистра
     */
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
                            this.modalService.close('modal-register');
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

    /**
     * Назад
     */
    toBack() {
        this.location.back();
    }

    /** 
     * Получение остатков по id выбранного наименования 
     */
    onFix() {
        if (!this.invoice.isFixed && this.registers.length > 0) {
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

    /** 
     * Получение остатков по id выбранного наименования 
    */
    getBatchesByItem() {
        this.apiService.getById<Batch[]>('register/item', this.register.itemId).subscribe(
            data => {
                // Получаем остатки по наименованию...
                this.batches = data;
                // ...и конвертируем их по шаблону в список для ng-select
                this.optionBatches = Converter.BatchToOptionArray(this.batches);
                // 
                //this.registerCost = this.register.cost.toString();
            },
            error => {
                this.alertService.serverError(error);
            });

    }

    /**
     * Изменение поставщика
     */
    onChangeVendor() {
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

    /**
     * Получения списка всех категорий
     */
    getAllCategories() {
        if (this.optionCategories.length == 0) {
            this.apiService.getAll<Category>('category').subscribe(
                data => {
                    this.optionCategories = Converter.ConvertToOptionArray(data);
                },
                error => {
                    this.alertService.serverError(error);
                }
            );
        }
    }

    /**
     * Получение наименований по выбранной категории
     */
    getItemsByCategory() {
        var params = 'categoryId=' + this.categoryId;
        this.apiService.get<Item[]>('item/filter', params).subscribe(
            data => {
                this.optionItems = Converter.ConvertToOptionArray(data);
            },
            error => {
                this.alertService.serverError(error);
            }
        );
    }

    /*
    * Загрузка всех наименований из справочника
    */
    getAllItems() {

        this.apiService.getAll<Item>('item').subscribe(
            data => {
                this.optionItems = Converter.ConvertToOptionArray(data);
            },
            error => {
                this.alertService.serverError(error);
            }
        );
    }

    /**
     * Событие: изменение категории
     * @param option 
     */
    onCategoryChanged(option: IOption) {
        this.categoryId = option.value;
        this.getItemsByCategory();
    }

    /**
     * Событие: изменение наименования
     * @param option 
     */
    onItemChanged(option: IOption) {
        this.register.itemId = option.value;
        this.getBatchesByItem();
    }

    onFormSubmit() {
        if (this.register.itemId != null && this.register.amount > 0 && this.register.cost >= 0) {
            switch (this.modal.type) {
                case 'new': this.onAdd(); break;
                case 'edit': this.onChange(); break;
            }
        } else {
            this.alertService.error('Невозможно добавить позицию. Проверьте правильность заполнения полей.');
        }
    }

    onBatchChanged(option: IOption) {
        this.register.cost = parseInt(option.value);
    }

}
