export interface AdminStatsResponse {
  totalUsers: number;
  totalRegularUsers: number;
  totalEditors: number;
  totalAdmins: number;
  totalReviews: number;
  totalGames: number;
  averageReviewsPerUser: number;
  averageRating: number;
  reviewsByGenre: { [key: string]: number };
  usersByMonth: { [key: string]: number };
  reviewsByMonth: { [key: string]: number };
}