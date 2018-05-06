import { Component, OnInit } from '@angular/core';
import { PageHeaderService } from '../../_services';

@Component({
    selector: 'app-help',
    templateUrl: './help.component.html'
})
export class HelpComponent implements OnInit {
    constructor(private pageHeaderService: PageHeaderService) { }

    ngOnInit(): void { 
        this.pageHeaderService.changeText("Помощь");
    }
}
