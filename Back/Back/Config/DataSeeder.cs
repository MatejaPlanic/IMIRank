using Back.Models.Game;
using Back.Models.User;
using Back.Models.Enums;
using MongoDB.Driver;

namespace Back.Config
{
    public class DataSeeder
    {
        private readonly IMongoCollection<Game> _games;
        private readonly IMongoCollection<User> _users;

        public DataSeeder(MongoDBContext context)
        {
            _games = context.GetCollection<Game>("Game");
            _users = context.GetCollection<User>("User");
        }

        public async Task SeedAsync()
        {
            var adminCount = await _users.CountDocumentsAsync(u => u.Role == UserRole.Admin);
            if (adminCount == 0)
            {
                var adminUser = new User
                {
                    UserName = "admin",
                    Email = "admin@imirank.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow
                };
                await _users.InsertOneAsync(adminUser);
            }

            var gamesCount = await _games.CountDocumentsAsync(_ => true);
            if (gamesCount > 0) return;

            var games = new List<Game>
            {
                new() {
                    Title = "The Witcher 3: Wild Hunt",
                    Genre = "RPG",
                    Developer = "CD Projekt Red",
                    CoverImageUrl = "https://upload.wikimedia.org/wikipedia/en/0/0c/Witcher_3_cover_art.jpg",
                    Description = "Epska RPG avantura u otvorenom svijetu sa bogatom pričom i nezaboravnim likovima.",
                    ReleaseYear = 2015
                },
                new() {
                    Title = "Red Dead Redemption 2",
                    Genre = "Action-Adventure",
                    Developer = "Rockstar Games",
                    CoverImageUrl = "https://upload.wikimedia.org/wikipedia/en/4/44/Red_Dead_Redemption_II.jpg",
                    Description = "Priča o Arthuru Morganu i životu na izmaku ere Divljeg zapada.",
                    ReleaseYear = 2018
                },
                new() {
                    Title = "Elden Ring",
                    Genre = "Action RPG",
                    Developer = "FromSoftware",
                    CoverImageUrl = "https://upload.wikimedia.org/wikipedia/en/b/b9/Elden_Ring_Box_art.jpg",
                    Description = "Mračni akcijski RPG smješten u svijet koji su stvorili Hidetaka Miyazaki i George R.R. Martin.",
                    ReleaseYear = 2022
                },
                new() {
                    Title = "God of War Ragnarök",
                    Genre = "Action-Adventure",
                    Developer = "Santa Monica Studio",
                    CoverImageUrl = "https://upload.wikimedia.org/wikipedia/en/e/e8/God_of_War_Ragnar%C3%B6k_cover.jpg",
                    Description = "Kratos i Atreus suočavaju se sa nordijskim bogovima u epskoj borbi za preživljavanje.",
                    ReleaseYear = 2022
                },
                new() {
                    Title = "Cyberpunk 2077",
                    Genre = "Action RPG",
                    Developer = "CD Projekt Red",
                    CoverImageUrl = "https://upload.wikimedia.org/wikipedia/en/9/9f/Cyberpunk_2077_box_art.jpg",
                    Description = "Otvoreni svijet budućnosti u megagradу Night City, gdje tehnologija i kriminal vladaju svime.",
                    ReleaseYear = 2020
                }
            };
            await _games.InsertManyAsync(games);
        }
    }
}
