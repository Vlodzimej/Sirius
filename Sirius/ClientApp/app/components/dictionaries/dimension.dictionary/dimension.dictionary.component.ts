import { Component, OnInit } from '@angular/core';
import { Dimension } from '../../_models';
import { ApiService, AlertService, LoadingService, PageHeaderService, ModalService } from '../../_services';

@Component({
    selector: 'app-dimension-dictionary',
    templateUrl: './dimension.dictionary.component.html',
    styleUrls: [
        '../../../assets/css/accordion.css',
        '../../../assets/css/modal.css'
    ]
})
export class DimensionDictionaryComponent implements OnInit {
    public dimensions: Dimension[] = [];
    public dimension: Dimension = new Dimension();

    constructor(
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService,
        private loadingService: LoadingService,
        private pageHeaderService: PageHeaderService
    ) { }

    ngOnInit() {
        // Включаем визуализацию загрузки
        this.loadingService.showLoadingIcon();
        this.pageHeaderService.changeText("Единицы измерения");
        this.apiService.getAll<Dimension>('dimension').subscribe(
            data => {
                this.dimensions = data;
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

    openDimension(id: string) {
        this.apiService.getById<Dimension>('dimension', id).subscribe(
            data => {
                this.dimension = data;
                this.modalService.open('modal-edit-dimension');
            },
            error => {
                this.alertService.serverError(error);
            });
    }

    addDimension() {
        this.apiService.create<Dimension>('dimension', this.dimension).subscribe(
            data => {
                this.ngOnInit();
                this.modalService.close('modal-new-dimension');
            },
            error => {
                this.alertService.serverError(error);
            });
    }

    updateDimension() {
        this.apiService.update<Dimension>('dimension', this.dimension.id, this.dimension).subscribe(
            data => {
                this.ngOnInit();
                this.modalService.close('modal-edit-dimension');
            },
            error => {
                this.alertService.serverError(error);
            });
    }
    onCreateDimension() {
        this.dimension = new Dimension();
        this.openModal('modal-new-dimension');
    }
}