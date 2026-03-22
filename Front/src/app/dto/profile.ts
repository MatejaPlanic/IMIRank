export interface ProfileReview {
  id: string;
  title: string;
  gameId: string;
  rating: number;
  createdAt: string;
}

export interface ProfileResponse {
  id: string;
  userName: string;
  email: string;
  role: string;
  createdAt: string;
  totalReviews: number;
  profilePictureUrl: string | null;
  recentReviews: ProfileReview[];
}