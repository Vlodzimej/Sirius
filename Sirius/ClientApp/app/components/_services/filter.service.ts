import { Injectable } from '@angular/core';
import { Filter } from '../_extends';

@Injectable()
export class FilterService {
    private filter: Filter = new Filter();

    setCategoryId(categoryId: string) {
        this.filter.categoryId = categoryId;
    }

    setItemId(itemId: string) {
        this.filter.itemId = itemId;
    }

    setVendorId(vendorId: string) {
        this.filter.vendorId = vendorId;
    }

    getFilter() {
        return this.filter;
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