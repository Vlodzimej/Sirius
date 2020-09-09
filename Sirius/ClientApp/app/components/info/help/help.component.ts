import { Component, OnInit } from '@angular/core';
import { PageHeaderService } from '../../_services';
import { HelpData } from './help.data';
import { HelpItem } from './help';

@Component({
    selector: 'app-help',
    templateUrl: './help.component.html',
    styleUrls: ['help.component.css'],
})
export class HelpComponent implements OnInit {
    helpItems = HelpData;
    selectedIndex: number = 0;

    selectedItem: HelpItem | undefined;
    constructor(private pageHeaderService: PageHeaderService) {}

    ngOnInit(): void {
        this.pageHeaderService.changeText('Помощь');
    }

    selectItem(index: number) {
        this.selectedItem = HelpData[index];
    }
}
