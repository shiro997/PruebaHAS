import { Component, Output, EventEmitter } from '@angular/core';
import { UserService } from '../../service/user.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginReq } from '../../models/LoginReq';
import { Router } from '@angular/router';

@Component({
  selector: 'app-log-in',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LogInComponent {
  @Output() loginEvent = new EventEmitter<boolean>();

  frmLogIn: FormGroup;
  errPassLength: string = 'The password must be at least 8 characters long and contain at least one letter and one number.';

  constructor(private userService: UserService, private frmBuilder: FormBuilder, private route: Router) {
    this.frmLogIn = this.frmBuilder.group({
      Email: ['', [Validators.required ,Validators.email]],
      Password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$/)]]
    })
  }

  login() {
    if (this.frmLogIn.valid) {
      let data: LoginReq = this.frmLogIn.value;
      this.userService.login(data).subscribe(resp => {
        if (resp && resp.body) {
          const response = resp.body;
          // Server sets cookie via Set-Cookie; because we use withCredentials the browser will store it.
          // Do NOT attempt to read Set-Cookie from JS (browsers block it). If the server also returns a token in body,
          // keep it in memory but do not persist it to localStorage.
          if (response.token) { this.userService.token = response.token; }
          this.userService.user = response.user;
          this.loginEvent.emit(response.isAuthenticated);
          this.route.navigate(['/dashboard']);
          this.frmLogIn = this.frmBuilder.group({
            Email: ['', [Validators.required, Validators.email]],
            Password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$/)]]
          })
        }
      })
    }
  }

  validateEmail()
  {
    const emailControl = this.frmLogIn.get('Email');
    const field = document.getElementById('mail');

    const touchedOrDirty = !!(emailControl?.touched || emailControl?.dirty);

    if (!touchedOrDirty) {
      field?.classList.remove('invalid');
      field?.classList.remove('valid');
      return;
    }

    if (emailControl?.invalid) {
      field?.classList.add('invalid');
      if (field?.classList.contains('valid')) { field.classList.remove('valid'); }
    } else {
      if (field?.classList.contains('invalid')) { field.classList.remove('invalid'); }
      field?.classList.add('valid');
    }
  }

  ValidatePassword(){
    const passwordControl = this.frmLogIn.get('Password');
    const errors = passwordControl?.errors;
    const field = document.getElementById('pass');

    if(errors && (errors['required'] || errors['minlength'] || errors['pattern'])){
      let field = document.getElementById('pass');
      field?.classList.add('invalid');
      if (field?.classList.contains('valid')) { field.classList.remove('valid'); }
    } else {
      if (field?.classList.contains('invalid')) { field.classList.remove('invalid'); }
      field?.classList.add('valid');
    }
  }
}