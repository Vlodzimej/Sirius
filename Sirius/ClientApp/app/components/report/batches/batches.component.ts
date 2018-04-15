import { Component, OnInit } from '@angular/core';
import { Batch, BatchGroup } from '../../_models';
import { Router, ActivatedRoute } from '@angular/router';
import { CurrencyPipe } from '../../_pipes';
import {
    ApiService,
    AlertService,
    ModalService,
    PageHeaderService
} from '../../_services';

@Component({
    selector: 'app-batches',
    templateUrl: './batches.component.html'
})
export class BatchesComponent implements OnInit {

    public batchGroups: BatchGroup[] = [];
    public batches: Batch[] = [];
    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private apiService: ApiService,
        private alertService: AlertService,
        private modalService: ModalService,
        private pageHeaderService: PageHeaderService
    ) { }

    ngOnInit(): void {
        this.apiService.getAll<BatchGroup>('register/batches').subscribe(
            data => {
                // Получаем структурированные данные и создаём сквозной массив для формирования таблицы
                this.batchGroups = data;
                this.batchGroups.forEach(element => {
                    var entry: Batch = { name: element.name, cost: 0, amount: 0, sum: 0 };
                    this.batches.push(entry);
                    
                    var amount: number = 0; // Для подсчета общего кол-ва 
                    element.batches.forEach(batch => {
                        batch.name = "";
                        batch.sum = batch.cost * batch.amount;
                        amount += batch.amount;
                        this.batches.push(batch);
                    });
                    // Закрывающий элемент группы в таблице
                    var lastElement: Batch = { name: 'Всего: '+amount, cost: 0, amount: 0, sum: 0 };
                    this.batches.push(lastElement);
                });
                console.log(this.batches);
            },
            error => {
                this.alertService.serverError(error);
            });
    }
}

