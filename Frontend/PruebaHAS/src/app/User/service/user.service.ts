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

  token: string = '';

  user!: User;

  // Only send appropriate request headers; CORS response headers must come from the server.
  headers: HttpHeaders = new HttpHeaders({
    'Content-Type': 'application/json',
    'Access-Control-Allow-Credentials': 'true'
  })

  env = environment;

  constructor(private http: HttpClient) {
    this.user = new User();
  }

  login(data: LoginReq): Observable<HttpResponse<LoginRes>> {
    // Use relative URL so dev-server proxy can forward requests to backend and avoid CORS issues
    let url = this.env.urlSecurity + '/api/v1/User/login';
    var body = JSON.stringify(data);
    console.debug('[UserService] POST', url, body);
    return this.http.post<LoginRes>(url, body, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Credentials': 'true'
      }), observe: 'response'
    })
      .pipe(
        tap(resp => console.debug('[UserService] login response', resp)),
        catchError(err => {
          console.error('[UserService] login error', err);
          return throwError(() => err);
        })
      );
  }

  getUsers(): Observable<User[]> {
    let url = this.env.urlSecurity + '/api/v1/User';
    console.debug('[UserService] GET', url);
    return this.http.get<User[]>(url, { headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Credentials': 'true',
      'Access-Control-Allow-Origin': this.env.urlSecurity
    })}).pipe(
      tap(resp => console.debug('[UserService] getUsers response', resp)),
      catchError(err => { console.error('[UserService] getUsers error', err); return throwError(() => err); })
    );
  }

  getUserById(id: number): Observable<User> {
    let url = `${this.env.urlSecurity}/api/v1/User/id=${id}`;
    console.debug('[UserService] GET', url);
    return this.http.get<User>(url, { headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Credentials': 'true',
      'Access-Control-Allow-Origin': this.env.urlSecurity
    })}).pipe(
      tap(resp => console.debug('[UserService] getUserById response', resp)),
      catchError(err => { console.error('[UserService] getUserById error', err); return throwError(() => err); })
    );
  }

  createUser(data: User): Observable<User> {
    let url = this.env.urlSecurity + '/api/v1/User';
    var body = JSON.stringify(data);
    console.debug('[UserService] POST', url, body);
    return this.http.post<User>(url, body, { headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Credentials': 'true',
      'Access-Control-Allow-Origin': this.env.urlSecurity
    })}).pipe(
      tap(resp => console.debug('[UserService] createUser response', resp)),
      catchError(err => { console.error('[UserService] createUser error', err); return throwError(() => err); })
    );
  }

  updateUser(data: User): Observable<boolean> {
    let url = `${this.env.urlSecurity}/api/v1/User`;
    var body = JSON.stringify(data);
    console.debug('[UserService] PUT', url, body);
    return this.http.put<boolean>(url, body, { headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Credentials': 'true',
      'Access-Control-Allow-Origin': this.env.urlSecurity
    }) }).pipe(
      tap(resp => console.debug('[UserService] updateUser response', resp)),
      catchError(err => { console.error('[UserService] updateUser error', err); return throwError(() => err); })
    );
  }

  deleteUser(id: number): Observable<boolean> {
    let url = `${this.env.urlSecurity }/api/v1/User/id=${id}`;
    console.debug('[UserService] DELETE', url);
    return this.http.delete<boolean>(url, { headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Credentials': 'true',
      'Access-Control-Allow-Origin': this.env.urlSecurity
    }) }).pipe(
      tap(resp => console.debug('[UserService] deleteUser response', resp)),
      catchError(err => { console.error('[UserService] deleteUser error', err); return throwError(() => err); })
    );
  }

}