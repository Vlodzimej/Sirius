import { Component, OnInit } from '@angular/core';
import { Vendor } from '../../_models';
import { ApiService, AlertService, ModalService, LoadingService, PageHeaderService } from '../../_services';

@Component({
    selector: 'app-vendor-dictionary',
    templateUrl: './vendor.dictionary.component.html',
    styleUrls: [
        '../../../assets/css/accordion.css',
        '../../../assets/css/modal.css'
    ]
})
export class VendorDictionaryComponent implements OnInit {
    public vendors: Vendor[] = [];
    public vendor: Vendor = new Vendor();

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
        this.pageHeaderService.changeText("Поставщики");
        this.apiService.getAll<Vendor>('vendor').subscribe(
            data => {
                this.vendors = data;
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

    openVendor(id: string) {
        this.apiService.getById<Vendor>('vendor', id).subscribe(
            data => {
                this.vendor = data;
                this.modalService.open('modal-edit-vendor');
            },
            error => {
                this.alertService.serverError(error);
            });
    }

    addVendor() {
        this.apiService.create<Vendor>('vendor', this.vendor).subscribe(
            data => {
                this.ngOnInit();
                this.modalService.close('modal-new-vendor');
            },
            error => {
                this.alertService.serverError(error);
            });
    }

    updateVendor() {
        this.apiService.update<Vendor>('vendor', this.vendor.id, this.vendor).subscribe(
            data => {
                this.ngOnInit();
                this.modalService.close('modal-edit-vendor');
            },
            error => {
                this.alertService.serverError(error);
            });
    }

    onCreateVendor() {
        this.vendor = new Vendor();
        this.modalService.open('modal-new-vendor');
    }
}