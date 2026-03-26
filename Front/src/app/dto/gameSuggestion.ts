export interface CreateGameSuggestionRequest {
  title: string;
  genre: string;
  developer: string;
  note: string;
}

export interface GameSuggestionResponse {
  id: string;
  userId: string;
  userName: string;
  title: string;
  genre: string;
  developer: string;
  note: string;
  isReviewed: boolean;
  createdAt: string;
}

export interface GameSuggestionListResponse {
  suggestions: GameSuggestionResponse[];
  total: number;
}