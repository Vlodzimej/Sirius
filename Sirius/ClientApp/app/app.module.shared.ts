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

import { ApiService, UserService, AuthenticationService, AlertService } from './components/_services';

import { AuthGuard } from './components/_guards/auth.guard';

@NgModule({
    declarations: [
        AppComponent,
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
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'nomenclature', component: NomenclatureComponent },
            { path: 'login', component: LoginComponent },
            { path: 'register', component: RegisterComponent},
            { path: 'userlist', component: UserListComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [ HttpModule, ApiService, UserService, AuthenticationService, AlertService ]
})
export class AppModuleShared {
}
