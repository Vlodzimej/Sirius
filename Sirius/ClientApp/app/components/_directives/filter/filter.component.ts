import { Component, OnInit } from '@angular/core';
import { Filter } from '../../_extends';
import { ApiService, AlertService } from '../../_services';
import { Item, Category, Vendor } from '../../_models';
import { Converter } from '../../_helpers';
import { FilterService } from '../../_services';

@Component({
    selector: 'app-filter',
    templateUrl: './filter.component.html'
})
export class FilterComponent implements OnInit {

    public optionItems: Array<string> = [];
    public optionCategories: Array<string> = [];
    public optionVendors: Array<string> = [];

    constructor(
        private apiService: ApiService,
        private alertService: AlertService,
        public filterService: FilterService
    ) { }

    ngOnInit(): void {

        this.optionVendors = ['one', 'two'];
        // Предварительная загрузка списка категорий из справочника
        this.getAllCategories();

        // Предварительная загрузка наименований из справочника
        this.getAllItems();

        // Предварительная загрузка списка поставщиков из справочника
        this.getAllVendors();
    }

    getAllCategories() {
        this.apiService.getAll<Category>('category').subscribe(
            data => {
                //this.optionCategories = Converter.ConvertToOptionArray(data);
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
                //this.optionItems = Converter.ConvertToOptionArray(data);
            },
            error => {
                this.alertService.serverError(error);
            }
        );
    }

    /*
    * Загрузка наименований из справочника согласно категории
    */
    getItemsByCategory() {
        this.apiService.get<Item[]>('item', 'categoryId=' + this.filterService.getCategoryId()).subscribe(
            data => {
                //this.optionItems = Converter.ConvertToOptionArray(data);
            },
            error => {
                this.alertService.serverError(error);
            }
        );
    }

    /*
    * Загрузка списка поставщиков из справочника
    */
    getAllVendors() {
        this.apiService.getAll<Vendor>('vendor').subscribe(
            data => {
                //this.optionVendors = Converter.ConvertToOptionArray(data);
            },
            error => {
                this.alertService.serverError(error);
            }
        );
    }


    onCategoryChanged(option: any) {
       // this.filterService.setCategoryId(option.value)
       // this.getItemsByCategory();
    }

    onItemChanged(option: any) {
       // this.filterService.setItemId(option.value)
    }

    onVendorChanged(option: any) {
       // this.filterService.setVendorId(option.value)
    }

    onCleanFilter() {
       // this.filterService.cleanFilter();
    }
}
