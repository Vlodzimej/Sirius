import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { SelectModule } from 'ng-select';

import { AppComponent } from './components/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { NomenclatureComponent } from './components/nomenclature/nomenclature.component';
import { LoginComponent } from "./components/login/login.component";
import { RegisterComponent } from './components/register';
import { SettingsComponent } from './components/settings';

import {
    AlertComponent,
    ModalComponent,
    PageHeaderComponent,
    LoadingIconComponent,
    FilterComponent
} from './components/_directives/';

import {
    ApiService,
    UserService,
    AuthenticationService,
    AlertService,
    ModalService,
    PageHeaderService,
    LoadingService,
    FilterService
} from './components/_services';

import { AuthGuard } from './components/_guards/auth.guard';

// Импорт компонентов со списками
import {
    UserDictionaryComponent,
    DimensionDictionaryComponent,
    CategoryDictionaryComponent,
    VendorDictionaryComponent,
    ItemDictionaryComponent
} from './components/dictionaries';

// Импорт компонентов относящихся к накладным
import {
    InvoiceListComponent, InvoiceDetailComponent
} from './components/invoice';

// Импорт компонентов отчетов
import {
    ReportComponent,
    BatchesComponent
} from './components/report'

// Импорт пайпов
import {
    FullDatePipe,
    CurrencyPipe
} from './components/_pipes';


@NgModule({
    declarations: [
        AppComponent,
        AlertComponent,
        ModalComponent,
        LoadingIconComponent,
        FilterComponent,
        NavMenuComponent,
        HomeComponent,
        PageHeaderComponent,
        NomenclatureComponent,
        LoginComponent,
        RegisterComponent,
        SettingsComponent,
        UserDictionaryComponent,
        DimensionDictionaryComponent,
        CategoryDictionaryComponent,
        VendorDictionaryComponent,
        ItemDictionaryComponent,
        InvoiceDetailComponent,
        InvoiceListComponent,
        ReportComponent,
        BatchesComponent,
        FullDatePipe,
        CurrencyPipe
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        SelectModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full', canActivate: [AuthGuard] },
            { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
            { path: 'login', component: LoginComponent },
            { path: 'register', component: RegisterComponent },
            { path: 'settings', component: SettingsComponent },
            { path: 'nomenclature', component: NomenclatureComponent, canActivate: [AuthGuard] },
            { path: 'users', component: UserDictionaryComponent, canActivate: [AuthGuard] },
            { path: 'dimensions', component: DimensionDictionaryComponent, canActivate: [AuthGuard] },
            { path: 'categories', component: CategoryDictionaryComponent, canActivate: [AuthGuard] },
            { path: 'vendors', component: VendorDictionaryComponent, canActivate: [AuthGuard] },
            { path: 'items', component: ItemDictionaryComponent, canActivate: [AuthGuard] },
            { path: 'invoices/:typealias', component: InvoiceListComponent, canActivate: [AuthGuard] },
            { path: 'invoice/:id', component: InvoiceDetailComponent, canActivate: [AuthGuard] },
            { path: 'report/:typealias', component: ReportComponent, canActivate: [AuthGuard] },
            { path: 'batches', component: BatchesComponent, canActivate: [AuthGuard] },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
        ApiService, 
        UserService, 
        AuthenticationService, 
        AlertService, 
        AuthGuard, 
        ModalService, 
        PageHeaderService, 
        LoadingService, 
        FilterService
    ],
    exports: [
        FullDatePipe, 
        CurrencyPipe
    ]
})
export class AppModuleShared {
}
