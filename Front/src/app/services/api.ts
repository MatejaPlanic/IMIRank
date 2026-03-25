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
import { ReviewCommentResponse, ReviewCommentsListResponse, CreateReviewCommentRequest, UpdateReviewCommentRequest } from '../dto/reviewComment';

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

  getReviewsByGame(gameId: string) {
    return this.httpClient.get<ReviewListResponse>(`${this.url}review/byGame/${gameId}`);
  }

  getReviewsByUser(userId: string) {
    return this.httpClient.get<ReviewListResponse>(`${this.url}review/byUser/${userId}`);
  }

  getGameById(id: string) {
    return this.httpClient.get<any>(`${this.url}game/${id}`);
  }

  searchUsers(query: string = '', page: number = 1, pageSize: number = 10) {
    return this.httpClient.get<any>(`${this.url}user?query=${encodeURIComponent(query)}&page=${page}&pageSize=${pageSize}`);
  }

  getUserById(id: string) {
    return this.httpClient.get<any>(`${this.url}user/${id}`);
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

// Review Comments
createReviewComment(payload: CreateReviewCommentRequest) {
  const token = localStorage.getItem('token');
  return this.httpClient.post<ReviewCommentResponse>(`${this.url}reviewcomment/create`, payload, {
    headers: { Authorization: `Bearer ${token}` }
  });
}

getReviewComments(reviewId: string, page: number = 1, pageSize: number = 10) {
  return this.httpClient.get<ReviewCommentsListResponse>(
    `${this.url}reviewcomment/review/${reviewId}?page=${page}&pageSize=${pageSize}`
  );
}

updateReviewComment(commentId: string, payload: UpdateReviewCommentRequest) {
  const token = localStorage.getItem('token');
  return this.httpClient.put<boolean>(`${this.url}reviewcomment/${commentId}`, payload, {
    headers: { Authorization: `Bearer ${token}` }
  });
}

deleteReviewComment(commentId: string) {
  const token = localStorage.getItem('token');
  return this.httpClient.delete<boolean>(`${this.url}reviewcomment/${commentId}`, {
    headers: { Authorization: `Bearer ${token}` }
  });
}

  getNotifications(page: number = 1, pageSize: number = 50) {
    const token = localStorage.getItem('token');
    return this.httpClient.get(`${this.url}notification/me?page=${page}&pageSize=${pageSize}`, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }

  markNotificationAsRead(notificationId: string) {
    const token = localStorage.getItem('token');
    return this.httpClient.put(`${this.url}notification/${notificationId}/read`, {}, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }
}
