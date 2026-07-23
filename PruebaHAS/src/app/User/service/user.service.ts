import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginReq } from '../models/LoginReq';
import { LoginRes } from '../models/LoginRes';
import { User } from '../models/User';
import { Observable, throwError } from 'rxjs'; 
import { tap, catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  token:string = '';

  user!:User;

  // Only send appropriate request headers; CORS response headers must come from the server.
  headers:HttpHeaders = new HttpHeaders({
    'Content-Type':'application/json'
  })

  env = environment;

  constructor(private http:HttpClient) { 
    this.user = new User();
    // token is no longer read from localStorage (cookie-based auth)
    const savedUser = localStorage.getItem('user');
    if (savedUser) {
      try { this.user = JSON.parse(savedUser); } catch (e) { this.user = new User(); }
    }
  }

  login(data:LoginReq):Observable<HttpResponse<LoginRes>>{
    // Use relative URL so dev-server proxy can forward requests to backend and avoid CORS issues
    let url = '/api/v1/User/login';
    var body = JSON.stringify(data);
    console.debug('[UserService] POST', url, body);
    return this.http.post<LoginRes>(url,body,{headers:this.headers, observe: 'response', withCredentials: true})
      .pipe(
        tap(resp => console.debug('[UserService] login response', resp)),
        catchError(err => {
          console.error('[UserService] login error', err);
          return throwError(() => err);
        })
      );
  }

  getUsers():Observable<User[]>{
    let url = '/api/v1/User';
    console.debug('[UserService] GET', url);
    return this.http.get<User[]>(url,{headers:this.headers, withCredentials: true}).pipe(
      tap(resp => console.debug('[UserService] getUsers response', resp)),
      catchError(err => { console.error('[UserService] getUsers error', err); return throwError(() => err); })
    );
  }

  getUserById(id:number):Observable<User>{
    let url = `/api/v1/User/id=${id}`;
    console.debug('[UserService] GET', url);
    return this.http.get<User>(url,{headers:this.headers, withCredentials: true}).pipe(
      tap(resp => console.debug('[UserService] getUserById response', resp)),
      catchError(err => { console.error('[UserService] getUserById error', err); return throwError(() => err); })
    );
  }

  createUser(data:User):Observable<User>{
    let url = '/api/v1/User';
    var body = JSON.stringify(data);
    console.debug('[UserService] POST', url, body);
    return this.http.post<User>(url,body,{headers:this.headers, withCredentials: true}).pipe(
      tap(resp => console.debug('[UserService] createUser response', resp)),
      catchError(err => { console.error('[UserService] createUser error', err); return throwError(() => err); })
    );
  }

  updateUser(data:User):Observable<boolean>{
    let url = `/api/v1/User`;
    var body = JSON.stringify(data);
    console.debug('[UserService] PUT', url, body);
    return this.http.put<boolean>(url,body,{headers:this.headers, withCredentials: true}).pipe(
      tap(resp => console.debug('[UserService] updateUser response', resp)),
      catchError(err => { console.error('[UserService] updateUser error', err); return throwError(() => err); })
    );
  }

  deleteUser(id:number):Observable<boolean>{
    let url = `/api/v1/User/id=${id}`;
    console.debug('[UserService] DELETE', url);
    return this.http.delete<boolean>(url,{headers:this.headers, withCredentials: true}).pipe(
      tap(resp => console.debug('[UserService] deleteUser response', resp)),
      catchError(err => { console.error('[UserService] deleteUser error', err); return throwError(() => err); })
    );
  }

}