import { Injectable } from '@angular/core';
import { Filter, FilterParameters } from '../_extends';

@Injectable()
export class FilterService {
    public filter: Filter = new Filter();
    public params: FilterParameters = new FilterParameters();

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

    setStartDate(date: Date) {
        this.filter.startDate = date;
    }

    setFinishDate(date: Date) {
        this.filter.finishDate = date;
    }

    setFixedOnly(fixedOnly: boolean) {
        this.filter.fixedOnly = fixedOnly;
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

    getStartDate() {
        return this.filter.startDate;
    }

    getFinishDate() {
        return this.filter.finishDate;
    }

    getIsFixed() {
        return this.filter.fixedOnly;   
    }

    cleanFilter() {
        this.filter = new Filter();
    }

}