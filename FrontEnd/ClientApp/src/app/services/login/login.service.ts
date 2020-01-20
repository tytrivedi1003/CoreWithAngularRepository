import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable,fromEventPattern } from 'rxjs';
import { UserDetails } from '../../models/userdetails/user-details';
import { map } from 'rxjs/operators';
import { Constants } from'../../models/constants/constants'

import { environment } from './../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class LoginService {
  
  private currentUserSubject: BehaviorSubject<UserDetails>;
  public currentUser: Observable<UserDetails>;
  public userDetails = new UserDetails();
  url:string;
  constructor(private http: HttpClient) {
   }
   loginUser(username: string, password: string)
   {
     debugger;
    this.url = environment.endpoints + Constants.login; 
    this.userDetails.EmailId = username;
    this.userDetails.StrPass = password;

    return this.http.post<any>(this.url,  this.userDetails )
            .pipe(map(user => {
                debugger;
                // login successful if there's a jwt token in the response
                if (user && user.token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', JSON.stringify(user));
                }
                return user;
            }));

   }

   registerUser(userdetails : UserDetails)
   {
    this.url = environment.endpoints + Constants.register; 
    this.userDetails.FirstName = userdetails.FirstName;
    this.userDetails.LastName = userdetails.LastName;
    this.userDetails.EmailId = userdetails.EmailId;
    this.userDetails.StrPass = userdetails.StrPass;
    
    return this.http.post<any>(this.url,  this.userDetails )
            .pipe(map(user => {
                debugger;
                // login successful if there's a jwt token in the response
                if (user && user.token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', JSON.stringify(user));
                }
                return user;
            }));

   }
}
