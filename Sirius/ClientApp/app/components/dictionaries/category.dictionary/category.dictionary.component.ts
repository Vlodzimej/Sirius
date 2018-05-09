import { Component, OnInit } from '@angular/core';
import { Category } from '../../_models';
import { ApiService, AlertService, ModalService, LoadingService, PageHeaderService } from '../../_services';


@Component({
    selector: 'app-category-dictionary',
    templateUrl: './category.dictionary.component.html',
    styleUrls: [
        '../../../assets/css/accordion.css',
        '../../../assets/css/modal.css'
    ]
})
export class CategoryDictionaryComponent implements OnInit {

    public categories: Category[] = [];
    public category: Category = new Category();

    constructor(
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService,
        public loadingService: LoadingService,
        private pageHeaderService: PageHeaderService
    ) { }

    ngOnInit() {
        // Включаем визуализацию загрузки
        this.loadingService.showLoadingIcon();
        this.pageHeaderService.changeText("Категории");
        this.apiService.getAll<Category>('category').subscribe(
            data => {
                this.categories = data;
                this.loadingService.hideLoadingIcon();
            },
            error => {
                this.alertService.serverError(error);

            });
    }

    openModal(id: string) {
        this.modalService.open(id);
    }

    closeModal(id: string) {
        this.modalService.close(id);
    }

    openCategory(id: string) {
        this.apiService.getById<Category>('category', id).subscribe(
            data => {
                this.category = data;
                this.modalService.open('modal-edit-category');
            },
            error => {
                this.alertService.serverError(error);
            });
    }

    addCategory() {
        this.apiService.create<Category>('category', this.category).subscribe(
            data => {
                this.ngOnInit();
                this.modalService.close('modal-new-category');
            },
            error => {
                this.alertService.serverError(error);
            });
    }

    updateCategory() {
        this.apiService.update<Category>('category', this.category.id, this.category).subscribe(
            data => {
                this.ngOnInit();
                this.modalService.close('modal-edit-category');
            },
            error => {
                this.alertService.serverError(error);
            });
    }
    onCreateCategory() {
        this.category = new Category();
        this.modalService.open('modal-new-category');
    }
}