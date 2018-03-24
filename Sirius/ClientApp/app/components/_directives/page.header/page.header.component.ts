import { Component, OnInit } from '@angular/core';
import { PageHeaderService } from '../../_services';

@Component({
    selector: 'app-page-header',
    templateUrl: './page.header.component.html',
})
export class PageHeaderComponent implements OnInit {

    headerText: any;

    constructor(private pageHeaderService: PageHeaderService) { }

    ngOnInit() {
        this.pageHeaderService.headerText.subscribe(headerText => this.headerText = headerText);
    }

}
