export interface GameItem {
  id: string;
  title: string;
  coverImageUrl: string;
  genre: string;
}

export interface GameSearchResponse {
  games: GameItem[];
  total: number;
}