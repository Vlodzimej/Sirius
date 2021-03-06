﻿import { Injectable } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs/Subject';
import { GenerateErrorMessage } from '../_helpers/error-message';

@Injectable()
export class AlertService {
    private subject = new Subject<any>();
    private keepAfterNavigationChange = false;

    constructor(private router: Router) {
        // clear alert message on route change
        router.events.subscribe(event => {
            if (event instanceof NavigationStart) {
                if (this.keepAfterNavigationChange) {
                    // only keep for a single location change
                    this.keepAfterNavigationChange = false;
                } else {
                    // clear alert
                    this.subject.next();
                }
            }
        });
    }

    success(message: string, keepAfterNavigationChange = false) {
        message = message.replace(/['"]+/g, '');
        this.keepAfterNavigationChange = keepAfterNavigationChange;
        this.subject.next({ type: 'success', text: message });
    }

    error(message: string, keepAfterNavigationChange = false) {
        message = message.replace(/['"]+/g, '');
        this.keepAfterNavigationChange = keepAfterNavigationChange;
        this.subject.next({ type: 'error', text: message });
    }

    getMessage(): Observable<any> {
        return this.subject.asObservable();
    }

    serverError(error: Response) {
        /*
         * Через некоторое время после последнего захода 
         * Bearer перестаёт пускать и api выдаёт 401 
         * Пока не разберусь в чем дело, ставлю здесь переход на страницу входа
         */ 
        if(error.status == 401) {
            this.router.navigateByUrl('/login');
        } 
        this.error(GenerateErrorMessage(error));
    }
}