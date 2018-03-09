import { Component, OnInit } from '@angular/core';
import { Category } from '../../_models';
import { ApiService, AlertService } from '../../_services';
import { ModalService } from '../../_services';

@Component({
    selector: 'app-category-dictionary',
    styleUrls: ['../../_content/modal.css'],
    templateUrl: './category.dictionary.component.html'
})
export class CategoryDictionaryComponent implements OnInit {
    public loading: boolean = true;
    public categories: Category[] = [];
    public category: Category = new Category();

    constructor(
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService) { }

    ngOnInit() {

        this.apiService.getAll<Category>('category').subscribe(
            data => {
                this.categories = data;
                this.loading = false;
            },
            error => {
                this.alertService.error('Ошибка вызова', true);
                this.loading = false;
            });
    }

    openModal(id: string) {
        this.modalService.open(id);
    }

    closeModal(id: string) {
        this.modalService.close(id);
    }

    openCategory(id: string) {
        console.log(id);
        this.apiService.getById<Category>('category', id).subscribe(
            data => {
                this.category = data;
                this.modalService.open('modal-edit-category');
            },
            error => {
                this.alertService.error('Ошибка загрузки', true);
            });
    }

    addCategory() {
        this.apiService.create<Category>('category', this.category).subscribe(
            data => {
                this.ngOnInit();
            },
            error => {
                this.alertService.error('Ошибка записи', true);
            });
    }

    updateCategory() {
        this.apiService.update<Category>('category', this.category.id, this.category).subscribe(
            data => {
                this.ngOnInit();
            },
            error => {
                this.alertService.error('Ошибка записи', true);
            });
    }
}