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
import { RegistrationComponent } from './components/registration/registration.component';
import { UserListComponent } from "./components/userlist/userlist.component";

import { ApiService } from './components/api/api.service';
import { UserService } from './components/user/user.service';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        NomenclatureComponent,
        LoginComponent,
        RegistrationComponent,
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
            { path: 'registration', component: RegistrationComponent},
            { path: 'userlist', component: UserListComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [ HttpModule, ApiService, UserService ]
})
export class AppModuleShared {
}
