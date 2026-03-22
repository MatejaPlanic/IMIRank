import { inject, Injectable } from '@angular/core';
import { userRole } from '../enums/userRols';
import { registerRequest } from '../dto/registerRequest';
import { HttpClient } from '@angular/common/http';
import { loginRequest } from '../dto/loginRequest';
import { HomeResponse } from '../dto/homeResponse';
import { GameSearchResponse } from '../dto/gameSearch';
import { CreateReviewRequest } from '../dto/createReviewRequest';
import { ReviewListResponse, ReviewItem } from '../dto/reviewList';
import { ProfileResponse } from '../dto/profile';

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

  searchGames(query: string = '', page: number = 1, pageSize: number = 5) {
    return this.httpClient.get<GameSearchResponse>(
      `${this.url}game?query=${query}&page=${page}&pageSize=${pageSize}`
    );
  }

  createReview(payload: CreateReviewRequest) {
    const token = localStorage.getItem('token');
    return this.httpClient.post(`${this.url}review`, payload, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }

  getReviews(genre = '', minRating = 0, sort = 'newest', page = 1, pageSize = 6) {
    return this.httpClient.get<ReviewListResponse>(
      `${this.url}review?genre=${genre}&minRating=${minRating}&sort=${sort}&page=${page}&pageSize=${pageSize}`
    );
  }

  getReviewById(id: string) {
    return this.httpClient.get<ReviewItem>(`${this.url}review/${id}`);
  }

  getProfile() {
  const token = localStorage.getItem('token');
  return this.httpClient.get<ProfileResponse>(`${this.url}profile`, {
    headers: { Authorization: `Bearer ${token}` }
  });
}

updateUsername(newUserName: string) {
  const token = localStorage.getItem('token');
  return this.httpClient.put(`${this.url}profile/username`, { newUserName }, {
    headers: { Authorization: `Bearer ${token}` }
  });
}

updatePassword(oldPassword: string, newPassword: string, confirmPassword: string) {
  const token = localStorage.getItem('token');
  return this.httpClient.put(`${this.url}profile/password`,
    { oldPassword, newPassword, confirmPassword },
    { headers: { Authorization: `Bearer ${token}` } }
  );
}

updateProfilePicture(file: File) {
  const token = localStorage.getItem('token');
  const formData = new FormData();
  formData.append('file', file);
  return this.httpClient.put(`${this.url}profile/picture`, formData, {
    headers: { Authorization: `Bearer ${token}` }
  });
}
}

