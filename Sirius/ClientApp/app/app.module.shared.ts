import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { NomenclatureComponent } from './components/nomenclature/nomenclature.component';
import { LoginComponent } from "./components/login/login.component";
import { RegisterComponent } from './components/register';
import { UserListComponent } from "./components/userlist/userlist.component";

import { AlertComponent, ModalComponent } from './components/_directives/';
import { ApiService, UserService, AuthenticationService, AlertService, ModalService  } from './components/_services';

import { AuthGuard } from './components/_guards/auth.guard';
import { Angular2FontawesomeModule } from 'angular2-fontawesome/angular2-fontawesome'
import { DimensionListComponent } from './components/dimension.list/dimension.list.component';


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
        UserListComponent,
        DimensionListComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        Angular2FontawesomeModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full', canActivate: [AuthGuard] },
            { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
            { path: 'login', component: LoginComponent },
            { path: 'register', component: RegisterComponent },
            { path: 'nomenclature', component: NomenclatureComponent, canActivate: [AuthGuard] },
            { path: 'userlist', component: UserListComponent, canActivate: [AuthGuard] },
            { path: 'dimensionlist', component: DimensionListComponent, canActivate: [AuthGuard] },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [ApiService, UserService, AuthenticationService, AlertService, AuthGuard, ModalService ]
})
export class AppModuleShared {
}
