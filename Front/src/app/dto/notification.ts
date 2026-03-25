export interface NotificationItem {
  id: string;
  recipientUserId: string;
  actorUserId: string;
  actorUserName: string;
  actorProfilePictureUrl?: string;
  reviewId: string;
  reviewCommentId?: string;
  message: string;
  isRead: boolean;
  createdAt: string;
}

export interface NotificationListResponse {
  notifications: NotificationItem[];
  totalCount: number;
}
