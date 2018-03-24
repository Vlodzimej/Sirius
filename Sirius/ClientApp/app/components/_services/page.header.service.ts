import { Component, Injectable, EventEmitter, Input, Output } from '@angular/core';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class PageHeaderService {
    public headerText: Subject<any>=new Subject();

    constructor() { }

	public changeText(text: string) {
		this.headerText.next(text);
	}
}