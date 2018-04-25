import { Component, OnInit } from '@angular/core';
import { Item, ItemDetail, Dimension, Category /*Vendor*/ } from '../../_models';
import { ApiService, AlertService, PageHeaderService } from '../../_services'
import { ModalService, FilterService } from '../../_services';
import { Filter } from '../../_extends';
@Component({
    selector: 'app-item-dictionary',
    templateUrl: './item.dictionary.component.html',
    styleUrls: [
        '../../../assets/css/accordion.css',
        '../../../assets/css/modal.css'
    ]
})
export class ItemDictionaryComponent implements OnInit {
    public loading: boolean = true;
    public items: Item[] = [];
    public itemDetail: ItemDetail = new ItemDetail();
    public item: Item = new Item();
    public selectedItemId: string;

    public dimensions: Dimension[] = [];
    public categories: Category[] = [];

    constructor(
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService,
        private pageHeaderService: PageHeaderService,
        private filterService: FilterService) { }

    /**Инициализация
     * 
     */
    ngOnInit() {
        // Устанавливаем поля фильтра
        this.filterService.setFilter({ category: true, item: true });

        this.pageHeaderService.changeText("Наименования");

        this.getItemsByFilter();

        if (this.loading) {
            this.loadDictionaries();
        }
    }

    getItemsByFilter() {
        // Получение критериев фильтрации 
        var filter = this.filterService.getFilter();
        var params: string = "";
        params += filter.categoryId != null ? "categoryId=" + filter.categoryId : "";
        params += filter.itemId != null ? "itemId=" + filter.itemId : "";

        this.apiService.get<Item[]>('item', params).subscribe(
            data => {
                this.items = data;
                this.loading = false;
            },
            error => {
                this.alertService.serverError(error);
            });
    }

    /**
     * Открытие модального окна 
    */
    openModal(id: string) {
        this.modalService.open(id);
    }

    /**
     * Закрытие модального окна 
    */
    closeModal(id: string) {
        this.modalService.close(id);
    }

    /**
     * Открытие модального окна с формой добавления нового наименования 
    */
    onCreateItem() {
        this.item = new Item();
        this.modalService.open('modal-new-item');
    }

    /**
     * Выбор наименования
     */
    onSelectItem(itemId: string) {
        this.selectedItemId = itemId;
    }

    /**
     * Открытие модального окна с информацией о наименовании и возможностью редактирования 
    */
    onOpenItem() {
        if (this.selectedItemId != null && this.selectedItemId != "") {
            this.apiService.getById<ItemDetail>('item', this.selectedItemId).subscribe(
                data => {
                    this.itemDetail = data;
                    this.modalService.open('modal-edit-item');
                },
                error => {
                    this.alertService.error('Ошибка загрузки', true);
                });
        }
    }

    /**
     * Добавление нового наименования 
     */
    onAddItem() {
        this.apiService.create<Item>('item', this.item).subscribe(
            data => {
                this.modalService.close('modal-new-item');
                this.ngOnInit();
            },
            error => {
                this.alertService.error('Ошибка записи', true);
            });
    }

    /**
     * Обновление данных наименования 
     */
    onUpdateItem() {
        if (this.itemDetail.id != null || this.itemDetail.id != "") {
            var newItem: Item = new Item();
            newItem.id = this.itemDetail.id;
            newItem.name = this.itemDetail.name;
            newItem.dimensionId = this.itemDetail.dimension.id;
            newItem.categoryId = this.itemDetail.category.id;
            newItem.isCountless = this.itemDetail.isCountless;
            delete (newItem.category);
            delete (newItem.dimension);

            this.apiService.update<Item>('item', newItem.id, newItem).subscribe(
                data => {
                    this.modalService.close('modal-edit-item');
                    this.ngOnInit();
                },
                error => {
                    this.alertService.error('Ошибка записи', true);
                });
        } else {
            this.alertService.error('Отсутствуют данные', true);
        }
    }

    /**
     * Загрузка необходимых справичников
     */
    loadDictionaries() {
        this.apiService.getAll<Dimension>('dimension').subscribe(
            data => {
                this.dimensions = data;
            },
            error => {
                this.alertService.serverError(error);
            });

        this.apiService.getAll<Category>('category').subscribe(
            data => {
                this.categories = data;
            },
            error => {
                this.alertService.serverError(error);
            });
    }
}