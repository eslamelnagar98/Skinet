import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ReplaySubject, map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ILoginDto, IRegisterDto, IUser } from '../shared/models/user';
import { IAddress } from '../shared/models/address';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  accountEndPoint = "Account";
  localStorageKey: string = 'token';
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  constructor(private httpClient: HttpClient, private router: Router) { }

  loadCurrentUser(token: string) {
    if (token === null) {
      this.currentUserSource.next(null);
      return of(null);
    }
    let headers = new HttpHeaders();
    headers = headers.set("Authorization", `Bearer ${token}`);
    return this.httpClient.get(`${this.baseUrl}${this.accountEndPoint}`, { headers }).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem(this.localStorageKey, user.token);
          this.currentUserSource.next(user);
        }
        return user;
      })
    )
  }

  login(loginDto: ILoginDto) {
    return this.httpClient.post(`${this.baseUrl}${this.accountEndPoint}/login`, loginDto).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem(this.localStorageKey, user.token)
          this.currentUserSource.next(user);
        }
      })
    )
  }

  register(registerDto: IRegisterDto) {
    return this.httpClient.post(`${this.baseUrl}${this.accountEndPoint}/register`, registerDto).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token)
        }
      })
    )
  }

  logout() {
    localStorage.removeItem(this.localStorageKey);
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }
  
  checkEmailExist(email: string) {
    let params: HttpParams = new HttpParams().append("email", email);
    return this.httpClient.get(`${this.baseUrl}${this.accountEndPoint}/emailexists`, { observe: 'response', params })
  }

  getUserAddress() {
    return this.httpClient.get<IAddress>(`${this.baseUrl}${this.accountEndPoint}/address`);
  }

  updateUserAddress(address: IAddress) {
    return this.httpClient.put<IAddress>(`${this.baseUrl}${this.accountEndPoint}/address`, address)
  }
}
