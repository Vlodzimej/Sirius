import { Component, OnInit } from '@angular/core';
import { Item, Dimension, Category /*Vendor*/ } from '../../_models';
import {
    ApiService,
    AlertService,
    PageHeaderService,
    LoadingService,
    ModalService,
    FilterService,
} from '../../_services';
import { Filter } from '../../_extends';
import * as _ from 'lodash';
import { GenerateErrorMessage } from '../../_helpers/error-message';

@Component({
    selector: 'app-item-dictionary',
    templateUrl: './item.dictionary.component.html',
    styleUrls: [
        '../../../assets/css/accordion.css',
        '../../../assets/css/modal.css',
        'item.dictionary.component.css',
    ],
})
export class ItemDictionaryComponent implements OnInit {
    public loading: boolean = true;
    public items: Item[] = [];
    public item: Item = new Item();
    public selectedItemId: string = '';

    public dimensions: Dimension[] = [];
    public categories: Category[] = [];

    constructor(
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService,
        private pageHeaderService: PageHeaderService,
        public loadingService: LoadingService,
        private filterService: FilterService
    ) {}

    /**
     * Инициализация
     */
    ngOnInit() {
        // Включаем визуализацию загрузки
        this.loadingService.showLoadingIcon();
        // Устанавливаем поля фильтра
        this.filterService.setFilter({ category: true, item: true });
        // Очистка полей фильтра
        this.filterService.cleanFilter();
        // Установка заголовка раздела
        this.pageHeaderService.changeText('Наименования');
        this.getItemsByFilter();
        this.loadDictionaries();
    }

    getItemsByFilter() {
        // Получение критериев фильтрации
        var filter = this.filterService.getFilter();
        var params: string = '';
        params +=
            filter.categoryId != null ? 'categoryId=' + filter.categoryId : '';
        params += filter.itemId != null ? '&itemId=' + filter.itemId : '';

        this.apiService.get<Item[]>('item', params).subscribe(
            (data) => {
                const arr = data.map((x) => {
                    return { ...x, sortName: x.name.trim().toLowerCase() };
                });
                this.items = _.sortBy(arr, 'sortName');
                this.loadingService.hideLoadingIcon();
            },
            (error) => {
                this.alertService.serverError(error);
            }
        );
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
        if (this.selectedItemId != null && this.selectedItemId != '') {
            this.apiService
                .getById<Item>('item', this.selectedItemId)
                .subscribe(
                    (data) => {
                        this.item = data;
                        this.modalService.open('modal-edit-item');
                        console.log(this.item);
                    },
                    (error) => {
                        this.alertService.error('Ошибка загрузки', true);
                    }
                );
        }
    }

    /**
     * Добавление нового наименования
     */
    onAddItem() {
        this.apiService.create<Item>('item', this.item).subscribe(
            (data) => {
                this.modalService.close('modal-new-item');
                this.ngOnInit();
            },
            (error) => {
                this.alertService.error(error._body, true);
            }
        );
    }

    /**
     * Обновление данных наименования
     */
    onUpdateItem() {
        if (this.item.id != null || this.item.id != '') {
            let newItem: Item = this.item;
            newItem.dimensionId = this.item.dimension.id;
            newItem.categoryId = this.item.category.id;
            delete newItem.category;
            delete newItem.dimension;

            console.log(newItem);

            this.apiService.update<Item>('item', newItem.id, newItem).subscribe(
                (data) => {
                    this.modalService.close('modal-edit-item');
                    this.ngOnInit();
                    console.log(this.item);
                },
                (error) => {
                    this.alertService.error(GenerateErrorMessage(error), true);
                }
            );
        } else {
            this.alertService.error('Отсутствуют данные', true);
        }
    }

    /**
     * Загрузка необходимых справочников
     */
    loadDictionaries() {
        this.apiService.getAll<Dimension>('dimension').subscribe(
            (data) => {
                this.dimensions = data;
            },
            (error) => {
                this.alertService.serverError(error);
            }
        );

        this.apiService.getAll<Category>('category').subscribe(
            (data) => {
                this.categories = data;
            },
            (error) => {
                this.alertService.serverError(error);
            }
        );
    }

    onDeleteItem() {
        this.apiService.delete('item', this.selectedItemId).subscribe(
            (data) => {
                this.closeModal('modal-edit-item');

                var deletedItem = this.items.find(
                    (i) => i.id == this.selectedItemId
                ) as Item;
                const i = this.items.indexOf(deletedItem);
                this.items.splice(i, 1);
                delete this.selectedItemId;
            },
            (error) => {
                this.alertService.error(error._body);
            }
        );
    }
}
