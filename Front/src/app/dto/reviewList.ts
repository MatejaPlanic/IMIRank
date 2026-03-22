export interface ReviewItem {
  id: string;
  gameId: string;
  gameTitle: string;
  gameCoverUrl: string;
  gameGenre: string;
  userId: string;
  userName: string;
  userProfilePictureUrl?: string;
  title: string;
  content: string;
  rating: number;
  createdAt: string;
}

export interface ReviewListResponse {
  reviews: ReviewItem[];
  total: number;
}