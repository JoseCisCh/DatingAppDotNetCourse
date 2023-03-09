import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + "users");
  }

  getMember(username: string) {
    return this.http.get<Member>(this.baseUrl + "users/" + username);
  }

  // Creating options including the token to access the server

  // This now can be removed as we have already implemented the JwtInterceptor
  // getHttpOptions() {
  //   const userString = localStorage.getItem('user');
  //   if (!userString) return;
  //   const user = JSON.parse(userString);
  //   return {
  //     headers: new HttpHeaders({
  //       Authorization: "Bearer " + user.token
        
  //     })
  //   }
  // }
}
