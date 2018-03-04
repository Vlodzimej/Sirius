import { Component, OnInit } from '@angular/core';
import { Dimension } from '../_models';
import { ApiService, AlertService } from '../_services';

@Component({
    selector: 'app-dimension-list',
    templateUrl: './dimension.list.component.html'
})
export class DimensionListComponent implements OnInit {

    public dimensions: Dimension[] = [];

    constructor(private apiService: ApiService, private alertService: AlertService) { }

    ngOnInit() { 
        
        this.apiService.getAll<Dimension>('dimension').subscribe(
            data => {
                console.log(data);
                this.dimensions = data;
                console.log(this.dimensions);
            },
            error => {
                this.alertService.error('Ошибка вызова', true);
            });
    }
}