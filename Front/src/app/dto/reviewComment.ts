export interface ReviewCommentResponse {
  id: string;
  reviewId: string;
  userId: string;
  userName: string;
  userProfilePictureUrl?: string;
  content: string;
  createdAt: string;
  updatedAt: string;
}

export interface ReviewCommentsListResponse {
  comments: ReviewCommentResponse[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface CreateReviewCommentRequest {
  reviewId: string;
  content: string;
}

export interface UpdateReviewCommentRequest {
  content: string;
}
