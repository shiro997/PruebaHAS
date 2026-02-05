import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { UserService } from '../../service/user.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from '../../models/User';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-form',
  templateUrl: './user.form.component.html',
  styleUrls: ['./user.form.component.css']
})
export class UserFormComponent implements OnInit {
  @Output() submitEvent = new EventEmitter<boolean>();
  @Input() isEditMode: boolean = false;
  @Input() userId: number = 0;

  frmUser: FormGroup;
  constructor(private userService: UserService, private frmBuilder: FormBuilder, private tstService: ToastrService) {
    this.frmUser = this.frmBuilder.group({
      nombreUsuario: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      usrPassword: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$/)]]
    });
  }

  ngOnInit(): void {
    if (this.isEditMode) {
      this.userService.getUserById(this.userId).subscribe(response => {
        if (response) {
          this.frmUser.patchValue({
            nombreUsuario: response.nombreUsuario,
            email: response.email,
            usrPassword: '' // Passwords are usually not pre-filled for security reasons
          });
        }
      });
    }
  }

  restartForm() {
    this.frmUser = this.frmBuilder.group({
      nombreUsuario: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      usrPassword: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$/)]]
    });

    const emailControl = this.frmUser.get('email');
    const field = document.getElementById('usrMail');
    if (emailControl?.value === '') {
      emailControl.markAsUntouched();
      field?.classList.remove('invalid');
      field?.classList.remove('valid');
    }

    const passwordControl = this.frmUser.get('usrPassword');
    const passField = document.getElementById('usrPass');
    if (passwordControl?.value === '') {
      passwordControl.markAsUntouched();
      passField?.classList.remove('invalid');
      passField?.classList.remove('valid');
    }
  }

  validateEmail() {
    const emailControl = this.frmUser.get('email');
    const field = document.getElementById('usrMail');

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
    const passwordControl = this.frmUser.get('usrPassword');
    const errors = passwordControl?.errors;
    const field = document.getElementById('usrPass');

    if (errors && (errors['required'] || errors['minlength'] || errors['pattern'])) {
      field?.classList.add('invalid');
      if (field?.classList.contains('valid')) { field.classList.remove('valid'); }
    } else {
      if (field?.classList.contains('invalid')) { field.classList.remove('invalid'); }
      field?.classList.add('valid');
    }
  }

  updateUser() {
    if (this.frmUser.valid) {
      //Lógica para actualizar el usuario
      let updatedUsr = this.frmUser.value as User;
      updatedUsr.idUsuario = this.userId;
      this.userService.updateUser(updatedUsr).subscribe({
        next: (response) => {
          if (response) {
            this.tstService.success('User updated successfully', 'Success');
            this.submitEvent.emit(response);
            this.restartForm();
          }
        },
        error: (err) => {
          if(err.status===400){
            this.tstService.error('Cannot update user due to existing dependencies.', 'Error');
          }
          if(err.status===404){
            this.tstService.error('User not found.', 'Error');
          }
          if(err.status===500){
            this.tstService.error('Server error. Please try again later.', 'Error');
          }
          this.restartForm(); 
        }
      });

    }
  }

  createUser() {
    if (this.frmUser.valid) {
      //Lógica para crear el usuario
      let newUsr = this.frmUser.value as User;
      this.userService.createUser(newUsr).subscribe(response => {
        if (response) {
          this.tstService.success('User created successfully', 'Success');
          this.submitEvent.emit(true);
          this.restartForm();
        }
      });
    }
  }
}