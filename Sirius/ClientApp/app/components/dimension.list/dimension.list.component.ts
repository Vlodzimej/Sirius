import { Component, OnInit } from '@angular/core';
import { Dimension } from '../_models';
import { ApiService, AlertService } from '../_services';
import { ModalService } from '../_services';

@Component({
    selector: 'app-dimension-list',
    styleUrls: ['./../_content/modal.css'],
    templateUrl: './dimension.list.component.html'
})
export class DimensionListComponent implements OnInit {
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
                console.log(data);
                this.dimensions = data;
                console.log(this.dimensions);
                this.loading = false;
            },
            error => {
                this.alertService.error('Ошибка вызова', true);
            });
    }

    addDimension(){
        this.apiService.create<Dimension>('dimension', this.dimension).subscribe(
            data => {
                this.ngOnInit();
            },
            error => {
                this.alertService.error('Ошибка записи', true);
            });
    }

    openModal(id: string) {
        this.modalService.open(id);
    }

    closeModal(id: string) {
        this.modalService.close(id);
    }
}