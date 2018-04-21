import { Injectable } from '@angular/core';

@Injectable()
export class LoadingService {
    private isProgress = false;

    getLoadingStatus() : boolean{
        return this.isProgress;
    }

    hideLoadingIcon(){
        this.isProgress = false;
    }
    
    showLoadingIcon(){
        this.isProgress = true;
    }
}