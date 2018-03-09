import { Component, OnInit } from '@angular/core';
import { Vendor } from '../../_models';
import { ApiService, AlertService, ModalService } from '../../_services';

@Component({
    selector: 'app-vendor-dictionary',
    styleUrls: ['../../_content/modal.css'],
    templateUrl: './vendor.dictionary.component.html'
})
export class VendorDictionaryComponent implements OnInit {
    public loading: boolean = true;
    public vendors: Vendor[] = [];
    public vendor: Vendor = new Vendor();

    constructor(
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService) { }

    ngOnInit() {
        this.apiService.getAll<Vendor>('vendor').subscribe(
            data => {
                this.vendors = data;
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

    openVendor(id: string) {
        this.apiService.getById<Vendor>('vendor', id).subscribe(
            data => {
                this.vendor = data;
                this.modalService.open('modal-edit-vendor');
            },
            error => {
                this.alertService.error('Ошибка загрузки', true);
            });
    }

    addVendor() {
        this.apiService.create<Vendor>('vendor', this.vendor).subscribe(
            data => {
                this.ngOnInit();
            },
            error => {
                this.alertService.error('Ошибка записи', true);
            });
    }

    updateVendor() {
        this.apiService.update<Vendor>('vendor', this.vendor.id, this.vendor).subscribe(
            data => {
                this.ngOnInit();
            },
            error => {
                this.alertService.error('Ошибка записи', true);
            });
    }
}