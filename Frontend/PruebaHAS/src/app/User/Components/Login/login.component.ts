import { Component, Output, EventEmitter } from '@angular/core';
import { UserService } from '../../service/user.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginReq } from '../../models/LoginReq';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { animate, state, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-log-in',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  animations:[
    trigger('flyInOut', [
      state('in', style({ transform: 'translateX(0)' })),
      transition('void => *', [
        style({ transform: 'translateX(-100%)' }),
        animate(100)
      ]),
      transition('* => void', [
        animate(100, style({ transform: 'translateX(100%)' }))
      ])
    ])
  ]
})
export class LogInComponent {
  @Output() loginEvent = new EventEmitter<boolean>();

  frmLogIn: FormGroup;
  errPassLength: string = 'The password must be at least 8 characters long and contain at least one letter and one number.';

  constructor(private userService: UserService, private frmBuilder: FormBuilder, private route: Router, private tstService: ToastrService) {
    this.frmLogIn = this.frmBuilder.group({
      Email: ['', [Validators.required, Validators.email]],
      Password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$/)]]
    })
  }

  login() {
    if (this.frmLogIn.valid) {
      let data: LoginReq = this.frmLogIn.value;
      this.userService.login(data).subscribe({
        next: (resp) => {

          if (resp && resp.body) {
            const response = resp.body;
            // Server sets cookie via Set-Cookie; because we use withCredentials the browser will store it.
            // Do NOT attempt to read Set-Cookie from JS (browsers block it). If the server also returns a token in body,
            // keep it in memory but do not persist it to localStorage.
            if (response.token) { this.userService.token = response.token; }
            this.userService.user = response.user;
            this.loginEvent.emit(response.isAuthenticated);
            this.tstService.success('Logged in successfully', 'Success');
            this.route.navigate(['/dashboard']);
            this.restartForm();
          }
        },
        error: (err) => {
          if(err.status===401){
            this.tstService.error('Invalid password. Please try again.', 'Authentication Failed');
          }
          if(err.status===404){
            this.tstService.error(err.error, 'Error');
          }
          if(err.status===500){
            this.tstService.error('Server error. Please try again later.', 'Error');
          }
          this.restartForm();
        }
      });
    }
  }

  validateEmail() {
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

  ValidatePassword() {
    const passwordControl = this.frmLogIn.get('Password');
    const errors = passwordControl?.errors;
    const field = document.getElementById('pass');

    if (errors && (errors['required'] || errors['minlength'] || errors['pattern'])) {
      let field = document.getElementById('pass');
      field?.classList.add('invalid');
      if (field?.classList.contains('valid')) { field.classList.remove('valid'); }
    } else {
      if (field?.classList.contains('invalid')) { field.classList.remove('invalid'); }
      field?.classList.add('valid');
    }
  }

  restartForm() {
    this.frmLogIn = this.frmBuilder.group({
      Email: ['', [Validators.required, Validators.email]],
      Password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$/)]]
    });

    const emailControl = this.frmLogIn.get('Email');
    const field = document.getElementById('mail');
    if (emailControl?.value === '') {
      emailControl.markAsUntouched();
      field?.classList.remove('invalid');
      field?.classList.remove('valid');
    }

    const passwordControl = this.frmLogIn.get('Password');
    const passField = document.getElementById('pass');
    if (passwordControl?.value === '') {
      passwordControl.markAsUntouched();
      passField?.classList.remove('invalid');
      passField?.classList.remove('valid');
    }
  }
}