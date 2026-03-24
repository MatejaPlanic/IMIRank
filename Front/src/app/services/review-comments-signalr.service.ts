import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { ReviewCommentResponse } from '../dto/reviewComment';

@Injectable({ providedIn: 'root' })
export class ReviewCommentsSignalRService {
  private hubConnection: HubConnection | null = null;

  private commentReceivedSubject = new Subject<ReviewCommentResponse>();
  private commentUpdatedSubject = new Subject<ReviewCommentResponse>();
  private commentDeletedSubject = new Subject<string>();

  public commentReceived$ = this.commentReceivedSubject.asObservable();
  public commentUpdated$ = this.commentUpdatedSubject.asObservable();
  public commentDeleted$ = this.commentDeletedSubject.asObservable();

  async joinReviewGroup(reviewId: string): Promise<void> {
    if (this.hubConnection) {
      await this.hubConnection.stop().catch(() => {});
      this.hubConnection = null;
    }

    const token = typeof window !== 'undefined' ? localStorage.getItem('token') ?? '' : '';

    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5062/reviewCommentsHub', {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Warning)
      .build();

    this.hubConnection.on('ReceiveComment', (comment: ReviewCommentResponse) => {
      this.commentReceivedSubject.next(comment);
    });

    this.hubConnection.on('UpdateComment', (comment: ReviewCommentResponse) => {
      this.commentUpdatedSubject.next(comment);
    });

    this.hubConnection.on('DeleteComment', (commentId: string) => {
      this.commentDeletedSubject.next(commentId);
    });

    await this.hubConnection.start();
    await this.hubConnection.invoke('JoinReviewGroup', reviewId);
  }

  async leaveReviewGroup(reviewId: string): Promise<void> {
    if (!this.hubConnection) return;
    try {
      await this.hubConnection.invoke('LeaveReviewGroup', reviewId);
      await this.hubConnection.stop();
    } catch { }
    this.hubConnection = null;
  }

  disconnect(): Promise<void> {
    if (!this.hubConnection) return Promise.resolve();
    return this.hubConnection.stop();
  }
}