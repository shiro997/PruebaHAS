import { Component, OnInit } from '@angular/core';
import { UserService } from '../../service/user.service';
import { Router } from '@angular/router';
import { User } from '../../models/User';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  userList:User[]=[];
  constructor(public userService:UserService, private route:Router){
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
}