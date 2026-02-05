import { Component } from '@angular/core';
import { Router } from '@angular/router';
import * as bootstrap from 'bootstrap';
import { User } from './User/models/User';
import { UserService } from './User/service/user.service';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'PruebaHAS';

  isAuth: boolean = false;

  constructor(private userService: UserService, private route: Router, private cookieStore: CookieService) {

  }

  cerrarModalLogin($event: boolean) {
    if ($event) {
      let modal = document.getElementById('loginModal');
      modal?.setAttribute('aria-hidden', 'true');
      modal?.classList.remove('show');
      modal?.setAttribute('style', 'display:none');

      document.querySelectorAll('.modal-backdrop').forEach(el => el.remove());
      document.body.classList.remove('modal-open');
      document.body.style.paddingRight = '0';
      
      this.isAuth = true;
    }
  }

  cerrarModalRegister($event: any) {
    if ($event != null) {
      let modal = document.getElementById('registerModal');
      let bsModal = new bootstrap.Modal(<HTMLElement>modal);
      bsModal?.hide();
    }
  }

  logOut() {
    this.isAuth = false;
    this.userService.token = '';
    this.userService.user = new User();
    this.cookieStore.deleteAll();
    localStorage.clear(); 
    this.route.navigate(['/']);//Navegar al login
  }
}
