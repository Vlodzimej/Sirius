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

import { AlertComponent } from './components/_directives/';
import { ApiService, UserService, AuthenticationService, AlertService } from './components/_services';

import { AuthGuard } from './components/_guards/auth.guard';

@NgModule({
    declarations: [
        AppComponent,
        AlertComponent,
        NavMenuComponent,
        HomeComponent,
        NomenclatureComponent,
        LoginComponent,
        RegisterComponent,
        UserListComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full', canActivate: [AuthGuard] },
            { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
            { path: 'nomenclature', component: NomenclatureComponent },
            { path: 'login', component: LoginComponent },
            { path: 'register', component: RegisterComponent },
            { path: 'userlist', component: UserListComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [ApiService, UserService, AuthenticationService, AlertService, AuthGuard]
})
export class AppModuleShared {
}
