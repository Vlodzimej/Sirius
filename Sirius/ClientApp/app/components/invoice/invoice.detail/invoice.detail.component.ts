import { Component, OnInit } from '@angular/core';

import { FormControl } from '@angular/forms';

import { ActivatedRoute } from '@angular/router';
import { Invoice, InvoiceUpdate, Register, Item, InvoiceType, Batch, Vendor, Category, InvoiceListItem } from '../../_models';
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

    public optionItems: Array<IOption> = [];
    public optionCategories: Array<IOption> = [];
    public optionBatches: Array<IOption> = [];
    public optionTemplates: Array<IOption> = [];

    public form: FormGroup;

    // Категория добавляемой/изменяемой позиции
    public categoryId: string = "";

    // Идентификатор шаблона накладной. Используется при добавлении регистров из выбранного шаблона
    public templateInvoiceId: string = "";

    /**
     *  Костыль для привязки значения цены регистра (для определения остатка) к списку ng-select, так как number принимать мы не хотим, приходится перед
     * открытием регистра на редактирование переводить значение цены в строку и назначать переменной registerCost
    */
    public registerCost: string = "";

    public modal: ModalType = new ModalType();
    private newRegisterModal: ModalType;
    private editRegisterModal: ModalType;
    private templateModal: ModalType;

    public isValid: boolean = true;

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

        this.templateModal = {
            type: 'template',
            title: 'Шаблон услуги',
            submit: 'Вставить'
        }

        // Устанавливаем значения переменных до загрузки накладной, что бы у парсера не возникало вопросов
        this.invoice.userFullName = "";
        this.invoice.createDate = "";
        this.invoice.isFixed = true;
        this.invoice.vendorName = "";
        this.invoice.vendorId = "";
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

                this.registers = this.invoice.registers;

                this.generatePageHeader()

                //Вычисление суммы для каждого регистра
                this.registers.map(r => {
                    r.sum = r.amount * r.cost
                    this.sum += r.sum;
                });

                //this.registers.forEach(element => {
                //this.sum += element.sum;
                //});

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
            delete (this.register);
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
            this.optionBatches = [];
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
            switch (this.invoiceType.alias) {
                case 'consumption':
                    /**
                     * РАСХОД
                     */
                    var params = 'itemid=' + this.register.itemId + '&cost=' + this.register.cost;
                    this.apiService.get<Batch>('register/batch', params).subscribe(
                        data => {
                            // Производим сравнение остатка и предполагаемого расхода добавляемой позиции по накладной
                            // Получение данных об остатке
                            var relativeBatch: Batch = data;
                            // Вычисление существующих позиций в накладной соответвующих выбранному остатку
                            var sumAmount: number = 0;
                            var existRegs = this.registers.filter(r => r.itemId == this.register.itemId && r.cost == this.register.cost);
                            existRegs.forEach(r => { sumAmount += r.amount });
                            // Прибавляем к вычисленному количеству расхода по выбранному остатку количество, которое указано в окне добавления позиции
                            sumAmount += this.register.amount;
                            // Сравнения общего расхода накладной по конкретному остатку
                            if (sumAmount <= relativeBatch.amount) {
                                this.addRegister();
                            } else {
                                this.alertService.error('Расход превышает остаток! Расход: ' + sumAmount + ' ед., остаток: ' + relativeBatch.amount + ' ед.');
                            }
                        },
                        error => {
                            this.alertService.serverError(error);
                        }
                    );
                    break;
                case 'template':
                    /**
                     * ШАБЛОН
                     */
                    // Фильтруем существующие регистры в поисках позиции, которая уже возможно была добавлена в шаблон
                    var filteredRegisters = this.registers.filter(r => r.itemId == this.register.itemId);
                    if (filteredRegisters.length > 0) {
                        // Если позиция уже существует - находим её в списке регистров и увеличиваем кол-во
                        var index = this.registers.indexOf(filteredRegisters[0]);
                        // Вычисляем новое кол-во в позиции
                        var sum = this.registers[index].amount + this.register.amount;
                        // Отправляем кол-во в добавляемый регистр
                        this.register.amount = sum;
                        // Присваиваем добавляемому регистру Id существующего регистра, таким образом, в базе данных произойдёт обновление после отправки запроса
                        this.register.id = this.registers[index].id;
                        this.register.invoiceId = this.invoice.id;
                        this.apiService.update('register', this.register.id, this.register).subscribe(
                            data => {
                                // Отправляем это кол-во в список отображения
                                this.registers[index].amount = sum;
                                this.modalService.close('modal-register');
                                this.alertService.success('Добавляемая позиция ' + filteredRegisters[0].name + ' уже существует в шаблоне услуги! Количество увеличено на ' + this.register.amount + ' ' + filteredRegisters[0].dimension);
                            }, error => {
                                this.alertService.serverError(error);
                            });

                    } else {
                        this.addRegister();
                    }
                    break;
                case 'arrival':
                    /**
                    * ПРИХОД 
                    */
                    this.addRegister();
                    break;
            }
        }
    }

    addRegister() {
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

                        delete (this.registerCost);
                        delete (this.optionBatches);

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
            //Проверка соответствия расходов к остаткам и вычисление суммы для каждого регистра
            this.isValid = true;
            this.registers.map(r => {
                // Проверяем цены. Если есть отрицательные значения, значит где-то нехватает остатков
                if (r.cost < 0) {
                    // Если отрицательные значения найдены
                    this.isValid = false;
                }
            });
            if (this.isValid) {
                this.apiService.update<string>('invoice/fix', this.invoice.id).subscribe(
                    data => {
                        this.invoice.isFixed = true;
                        this.alertService.success(data);
                    },
                    error => {
                        this.alertService.serverError(error);
                    });
            } else {
                this.alertService.error('Расход некоторых позиций документа превышает фактический остаток!');
            }
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
    onVendorChanged() {
        if (this.invoice.vendorId != "") {
            this.apiService.update<string>('invoice', this.invoice.id + '/vendor?value=' + this.invoice.vendorId, "").subscribe(
                data => {
                    this.alertService.success(data);
                },
                error => {
                    this.alertService.serverError(error);
                });
        }
    }

    /**
     * Изменение названия накладной
     */
    onNameChanged() {
        this.generatePageHeader();
        if (this.invoice.name != "") {
            this.apiService.update<string>('invoice', this.invoice.id + '/name?value=' + this.invoice.name, "").subscribe(
                data => {
                    this.alertService.success(data);
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
        if (this.register.itemId != null && this.register.amount > 0) {
            switch (this.modal.type) {
                case 'new': this.onAdd(); break;
                case 'edit': this.onChange(); break;
            }
        } else {
            this.alertService.error('Невозможно добавить позицию. Проверьте правильность заполнения полей.');
        }
    }

    onBatchChanged(option: IOption) {
        this.register.cost = parseFloat(option.value);
    }

    generatePageHeader() {
        var headerText = this.invoice.name + " от " + this.invoice.createDate;
        this.pageHeaderService.changeText(headerText);
    }

    onOpenTemplates() {
        if (this.registers.length == 0) {
            // Загрузка списка накладных-шаблонов
            // Получение данных о типе 'Шаблон'
            this.apiService.getById<InvoiceType>('invoice/type/alias', 'template').subscribe(
                data => {
                    var type = data;
                    // Параметр для отбора по типу накладной
                    var params = "typeid=" + type.id;
                    // Получение списка накладных
                    this.apiService.get<InvoiceListItem[]>('invoice', params).subscribe(
                        data => {
                            // Конвертация полученного списка в массив для выпадающего списка ng-select
                            this.optionTemplates = Converter.ConvertToOptionArray(data);
                            // Назначение свойств модали 'Шаблон' для модального окна
                            this.modal = this.templateModal;
                            // Открытие модального окна
                            this.modalService.open('modal-template');
                        },
                        error => {
                            this.alertService.serverError(error);
                        });
                },
                error => {
                    this.alertService.serverError(error);
                });
        } else {
            this.alertService.error('Нельзя добавить услугу из шаблона, так как документ содержит позиции. Пожалуйста, удалите все позиции и попробуйте снова.');
        }
    }

    addRegistersFromTemplate() {
        var params = 'sourceId=' + this.templateInvoiceId + '&destinationId=' + this.invoice.id;
        // Метод делает копию регистров шаблона и назначаем им Id текущего документа
        this.apiService.create<Register[]>('register/copy?' + params, new Array<Register>()).subscribe(
            data => {
                //Таким образом, после повторной инициализации в списке появляются скопированные регистры
                this.ngOnInit();
                this.modalService.close('modal-template');
            },
            error => {
                this.alertService.serverError(error);
            }
        );
    }

}
