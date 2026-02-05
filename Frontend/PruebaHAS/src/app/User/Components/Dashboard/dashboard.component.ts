import { Component, OnInit } from '@angular/core';
import { UserService } from '../../service/user.service';
import { Router } from '@angular/router';
import { User } from '../../models/User';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  userList:User[]=[];
  isCreateMode: boolean = false;
  isEditMode: boolean = false;
  userIdToEdit: number = 0;
  constructor(public userService:UserService, private route:Router, private tstService:ToastrService){
  } 

  ngOnInit(): void {
    // Check session by presence of user object (cookie-based auth stores session server-side)
    if (!this.userService.user || !this.userService.user.nombreUsuario) {
      this.route.navigate(['/']);
      return;
    }
    this.userService.getUsers().subscribe(response=>{
      if(response){
        this.userList=response;
      }
    });
  }

  refreshUsers(){
    this.userService.getUsers().subscribe(response=>{
      if(response){
        this.userList=response;
        this.isCreateMode = false;
        this.isEditMode = false;
      }
    });
  }

  enterCreateMode(){
    this.isCreateMode = true;
  }

  enterEditMode(userId: number){
    this.isEditMode = true;
    this.userIdToEdit = userId;
  }

  deleteUser(userId: number){
    this.userService.deleteUser(userId).subscribe({
      next: (response) => {
        if(response){
          this.refreshUsers();
          this.tstService.success('User deleted successfully', 'Success');
        }
      },
      error: (err) => {
        console.error('Error deleting user', err);
        if(err.status===400){
          this.tstService.error('Cannot delete user due to existing dependencies.', 'Error');
        }
        if(err.status===404){
          this.tstService.error('User not found.', 'Error');
        }
        if(err.status===500){
          this.tstService.error('Server error. Please try again later.', 'Error');
        }
      }
    });
  }
}