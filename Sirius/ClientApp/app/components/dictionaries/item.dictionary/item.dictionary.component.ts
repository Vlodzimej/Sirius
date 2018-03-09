import { Component, OnInit } from '@angular/core';
import { Item } from '../../_models';
import { ApiService, AlertService } from '../../_services';
import { ModalService } from '../../_services';

@Component({
    selector: 'app-item-dictionary',
    styleUrls: ['../../_content/modal.css'],
    templateUrl: './item.dictionary.component.html'
})
export class ItemDictionaryComponent implements OnInit {
    public loading: boolean = true;
    public items: Item[] = [];
    public item: Item = new Item();

    constructor(
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService) { }

    ngOnInit() {

        this.apiService.getAll<Item>('item').subscribe(
            data => {
                this.items = data;
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

    openItem(id: string) {
        console.log(id);
        this.apiService.getById<Item>('item', id).subscribe(
            data => {
                this.item = data;
                this.modalService.open('modal-edit-item');
            },
            error => {
                this.alertService.error('Ошибка загрузки', true);
            });
    }

    addItem() {
        this.apiService.create<Item>('item', this.item).subscribe(
            data => {
                this.ngOnInit();
            },
            error => {
                this.alertService.error('Ошибка записи', true);
            });
    }

    updateItem() {
        this.apiService.update<Item>('item', this.item.id, this.item).subscribe(
            data => {
                this.ngOnInit();
            },
            error => {
                this.alertService.error('Ошибка записи', true);
            });
    }
}