import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms'; 


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserService } from './User/service/user.service';
import { LogInComponent } from './User/Components/Login/login.component';
import { DashboardComponent } from './User/Components/Dashboard/dashboard.component';
import { UserFormComponent } from './User/Components/Form/user.form.component';
@NgModule({
  declarations: [
    AppComponent,
    LogInComponent,
    DashboardComponent,
    //UserFormComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [
    UserService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
