export interface AddGameRequest {
  title: string;
  genre: string;
  developer: string;
  description: string;
  releaseYear: number;
  coverImage?: File;
}