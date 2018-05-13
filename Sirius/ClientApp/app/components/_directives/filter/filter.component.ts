import { Component, OnInit } from '@angular/core';
import { Filter, ISelectOption } from '../../_extends';
import { ApiService, AlertService } from '../../_services';
import { Item, Category, Vendor } from '../../_models';
import { Converter } from '../../_helpers';
import { FilterService } from '../../_services';

@Component({
    selector: 'app-filter',
    templateUrl: './filter.component.html'
})
export class FilterComponent implements OnInit {

    public optionItems: Array<ISelectOption> = [];
    public optionCategories: Array<ISelectOption> = [];
    public optionVendors: Array<ISelectOption> = [];

    constructor(
        private apiService: ApiService,
        private alertService: AlertService,
        public filterService: FilterService
    ) { }

    ngOnInit(): void {

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
                this.optionCategories = Converter.ConvertToSelectOptionArray(data);
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
                this.optionItems = Converter.ConvertToSelectOptionArray(data);
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
                this.optionItems = Converter.ConvertToSelectOptionArray(data);
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
                this.optionVendors = Converter.ConvertToSelectOptionArray(data);
            },
            error => {
                this.alertService.serverError(error);
            }
        );
    }


    onCategoryChanged() {
       // this.filterService.setCategoryId(option.value)
       this.getItemsByCategory();
    }

    onItemChanged(option: any) {
       // this.filterService.setItemId(option.value)
    }

    onVendorChanged(option: any) {
       // this.filterService.setVendorId(option.value)
    }

    onCleanFilter() {
        this.filterService.cleanFilter();
    }

    onBatchCalcMethodToggle() {
        if (this.filterService.filter.isDynamic) {
            this.filterService.params.vendor = false;
        } else {
            this.filterService.params.vendor = true;
        }
    }
}
