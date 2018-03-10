import { Component, OnInit } from '@angular/core';
import { Dimension } from '../../_models';
import { ApiService, AlertService } from '../../_services';
import { ModalService } from '../../_services';

@Component({
    selector: 'app-dimension-dictionary',
    styleUrls: ['../../_content/modal.css'],
    templateUrl: './dimension.dictionary.component.html'
})
export class DimensionDictionaryComponent implements OnInit {
    public loading: boolean = true;
    public dimensions: Dimension[] = [];
    public dimension: Dimension = new Dimension();

    constructor(
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService) { }

    ngOnInit() {

        this.apiService.getAll<Dimension>('dimension').subscribe(
            data => {
                this.dimensions = data;
                this.loading = false;
            },
            error => {
                this.alertService.error('Ошибка вызова', true);
            });
    }

    openModal(id: string) {
        this.modalService.open(id);
    }

    closeModal(id: string) {
        this.modalService.close(id);
    }

    openDimension(id: string) {
        console.log(id);
        this.apiService.getById<Dimension>('dimension', id).subscribe(
            data => {
                this.dimension = data;
                this.modalService.open('modal-edit-dimension');
            },
            error => {
                this.alertService.error('Ошибка загрузки', true);
            });
    }

    addDimension() {
        this.apiService.create<Dimension>('dimension', this.dimension).subscribe(
            data => {
                this.ngOnInit();
            },
            error => {
                this.alertService.error('Ошибка записи', true);
            });
    }

    updateDimension() {
        this.apiService.update<Dimension>('dimension', this.dimension.id, this.dimension).subscribe(
            data => {
                this.ngOnInit();
            },
            error => {
                this.alertService.error('Ошибка записи', true);
            });
    }
}