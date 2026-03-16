import { inject, Injectable } from '@angular/core';
import { userRole } from '../enums/userRols';
import { registerRequest } from '../dto/registerRequest';
import { HttpClient } from '@angular/common/http';
import { loginRequest } from '../dto/loginRequest';
import { HomeResponse } from '../dto/homeResponse';

@Injectable({
  providedIn: 'root',
})
export class Api {
  private url : string = 'http://localhost:5062/api/'
  private httpClient : HttpClient = inject(HttpClient)

  register(userName: string, email: string, password: string, role: userRole) {
    const payload: registerRequest = {
      userName: userName,
      email: email,
      password: password,
      role: role
    };

    return this.httpClient.post(`${this.url}auth/register`,payload,{responseType:"text"})
  }

  login(email:string,password:string)
  {
    const payload: loginRequest = {
      email: email,
      password: password
    };

    return this.httpClient.post(`${this.url}auth/login`,payload,{responseType:"text"})
  }

   getHomeData() {
    return this.httpClient.get<HomeResponse>(`${this.url}home`);
  }
}
