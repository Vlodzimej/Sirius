import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { NomenclatureComponent } from './components/nomenclature/nomenclature.component';
import { LoginComponent } from "./components/login/login.component";
import { RegisterComponent } from './components/register';

import { AlertComponent, ModalComponent } from './components/_directives/';
import { 
    ApiService, 
    UserService, 
    AuthenticationService, 
    AlertService, 
    ModalService  
} from './components/_services';

import { AuthGuard } from './components/_guards/auth.guard';
//import { Angular2FontawesomeModule } from 'angular2-fontawesome/angular2-fontawesome'

// Импорт компонентов со списками
import { 
    UserDictionaryComponent, 
    DimensionDictionaryComponent, 
    CategoryDictionaryComponent,
    VendorDictionaryComponent,
    ItemDictionaryComponent
} from './components/dictionaries';

// Импорт компонентов с накладными
import { ArrivalInvoiceComponent } from './components/invoces';

@NgModule({
    declarations: [
        AppComponent,
        AlertComponent,
        ModalComponent,
        NavMenuComponent,
        HomeComponent,
        NomenclatureComponent,
        LoginComponent,
        RegisterComponent,
        UserDictionaryComponent,
        DimensionDictionaryComponent,
        CategoryDictionaryComponent,
        VendorDictionaryComponent,
        ItemDictionaryComponent,
        ArrivalInvoiceComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full', canActivate: [AuthGuard] },
            { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
            { path: 'login', component: LoginComponent },
            { path: 'register', component: RegisterComponent },
            { path: 'nomenclature', component: NomenclatureComponent, canActivate: [AuthGuard] },
            { path: 'users', component: UserDictionaryComponent, canActivate: [AuthGuard] },
            { path: 'dimensions', component: DimensionDictionaryComponent, canActivate: [AuthGuard] },
            { path: 'categories', component: CategoryDictionaryComponent, canActivate: [AuthGuard] },
            { path: 'vendors', component: VendorDictionaryComponent, canActivate: [AuthGuard] },
            { path: 'items', component: ItemDictionaryComponent, canActivate: [AuthGuard] },
            { path: 'arrivalinvoice', component: ArrivalInvoiceComponent, canActivate: [AuthGuard] },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [ApiService, UserService, AuthenticationService, AlertService, AuthGuard, ModalService ]
})
export class AppModuleShared {
}
