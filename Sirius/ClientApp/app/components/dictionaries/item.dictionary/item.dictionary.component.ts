import { Component, OnInit } from '@angular/core';
import { Item, ItemDetail, Dimension, Category /*Vendor*/ } from '../../_models';
import { ApiService, AlertService } from '../../_services'
import { ModalService } from '../../_services';
import { ItemFilter } from './item.filter';

@Component({
    selector: 'app-item-dictionary',
    styleUrls: ['../../_content/modal.css'],
    templateUrl: './item.dictionary.component.html'
})
export class ItemDictionaryComponent implements OnInit {
    public loading: boolean = true;
    public items: Item[] = [];
    public itemDetail: ItemDetail = new ItemDetail();
    public item: Item = new Item();

    public dimensions: Dimension[] = [];
    public categories: Category[] = [];
    //public vendors: Vendor[] = [];

    public itemFilter: ItemFilter = new ItemFilter();

    constructor(
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService) { }

    /**Инициализация
     * 
     */
    ngOnInit() {
        if (this.loading) {
            this.loadDictionaries();
        }

        this.apiService.getAll<Item>('item').subscribe(
            data => {
                this.items = data;
                console.log(this.items);
                this.loading = false;
            },
            error => {
                this.alertService.error('Ошибка вызова', true);
                this.loading = false;
            });
    }
    
    /**Открытие модального окна  */
    openModal(id: string) {
        this.modalService.open(id);
    }

    /**Закрытие модального окна */
    closeModal(id: string) {
        this.modalService.close(id);
    }

    /**Открытие модального окна с формой добавления нового наименования */
    openNewItem(){
        this.item = new Item();
        this.modalService.open('modal-new-item');
    }

    /**Открытие модального окна с информацией о наименовании и возможностью редактирования */
    openItem(id: string) {
        this.apiService.getById<ItemDetail>('item', id).subscribe(
            data => {
                this.itemDetail = data;
                console.log(this.itemDetail);
                this.modalService.open('modal-edit-item');
            },
            error => {
                this.alertService.error('Ошибка загрузки', true);
            });
    }

    /**Добавление нового наименования */
    addItem() {
        console.log(this.item);
        this.apiService.create<Item>('item', this.item).subscribe(
            data => {
                this.ngOnInit();
            },
            error => {
                this.alertService.error('Ошибка записи', true);
            });
    }

    Test(){
        console.log(this.item);
    }

    /**Обновление данных наименования */
    updateItem() {
        if (this.itemDetail.id != null || this.itemDetail.id != "") {

            var newItem: Item = new Item();
            newItem.id = this.itemDetail.id;
            newItem.name = this.itemDetail.name;
            newItem.dimensionId = this.itemDetail.dimension.id;
            newItem.categoryId = this.itemDetail.category.id;

            this.apiService.update<Item>('item', newItem.id, newItem).subscribe(
                data => {
                    this.ngOnInit();
                },
                error => {
                    this.alertService.error('Ошибка записи', true);
                });
        } else {
            this.alertService.error('Отсутствуют данные', true);
        }
    }

    /**Загрузка необходимых справичников*/
    loadDictionaries() {
        this.apiService.getAll<Dimension>('dimension').subscribe(
            data => {
                this.dimensions = data;
            },
            error => {
                this.alertService.error('Ошибка загрузки списка единици измерения', true);
                this.loading = false;
            });

        this.apiService.getAll<Category>('category').subscribe(
            data => {
                this.categories = data;
            },
            error => {
                this.alertService.error('Ошибка загрузки списка категорий', true);
                this.loading = false;
            });
        /*
        this.apiService.getAll<Vendor>('vendor').subscribe(
            data => {
                this.vendors = data;
            },
            error => {
                this.alertService.error('Ошибка загрузки списка поставщиков', true);
                this.loading = false;
            });
        */      
    }
}