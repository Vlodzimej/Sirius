import { Component, OnInit } from '@angular/core';
import { LoadingService } from '../../_services';
@Component({
    selector: 'app-loading-icon',
    templateUrl: './loading.icon.component.html'
})
export class LoadingIconComponent implements OnInit {
    constructor(private loadingService: LoadingService) { }

    ngOnInit(): void { }
}
