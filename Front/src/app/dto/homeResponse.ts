export interface GameCard {
  id: string;
  title: string;
  genre: string;
  developer: string;
  coverImageUrl: string;
  averageRating: number;
  reviewCount: number;
  releaseYear: number;
}

export interface RecentReview {
  id: string;
  gameId: string;
  gameTitle: string;
  gameCoverUrl: string;
  userName: string;
  title: string;
  content: string;
  rating: number;
  createdAt: string;
}

export interface Stats {
  totalReviews: number;
  totalUsers: number;
  totalEditors: number;
}

export interface HomeResponse {
  topRatedGames: GameCard[];
  latestGames: GameCard[];
  recentReviews: RecentReview[];
  stats: Stats;
}