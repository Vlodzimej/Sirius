import { Injectable } from '@angular/core';
import { Filter, FilterParameters } from '../_extends';

@Injectable()
export class FilterService {
    private filter: Filter = new Filter();
    private params: FilterParameters = new FilterParameters();

    setFilter(params: FilterParameters) {
        this.params = params;
    }

    getFilter() {
        return this.filter;
    }

    setName(name: string) {
        this.filter.name = name;
    }

    setCategoryId(categoryId: string) {
        this.filter.categoryId = categoryId;
    }

    setItemId(itemId: string) {
        this.filter.itemId = itemId;
    }

    setVendorId(vendorId: string) {
        this.filter.vendorId = vendorId;
    }

    getName() {
        return this.filter.name;
    }

    getCategoryId() {
        return this.filter.categoryId;
    }

    getItemId() {
        return this.filter.itemId;
    }

    getVendorId() {
        return this.filter.vendorId;
    }
}