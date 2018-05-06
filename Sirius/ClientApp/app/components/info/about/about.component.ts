import { Component, OnInit } from '@angular/core';
import { PageHeaderService } from '../../_services';

@Component({
    selector: 'app-about',
    templateUrl: './about.component.html'
})
export class AboutComponent implements OnInit {
    constructor(private pageHeaderService: PageHeaderService) { }

    ngOnInit(): void { 
        this.pageHeaderService.changeText("Информация");
    }
}
