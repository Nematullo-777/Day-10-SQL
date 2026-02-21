using Infrastructure;
using Domain;
using Infrastructure.Service;
using Domain.Entities;
var reportService = new ReportService();

var movies1 = reportService.GetMoviesByGenre("Drama");
foreach (var m in movies1)
    Console.WriteLine(m.Title);

reportService.GetScreeningCountPerMovie();
reportService.GetFullScreeningInfo();
var movieService = new MovieService();

while (true)
{
    Console.WriteLine("1. Показать фильмы");
    Console.WriteLine("2. Добавить фильм");
    Console.WriteLine("0. Выход");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            var movies = movieService.GetAll();
            foreach (var m in movies)
                Console.WriteLine($"{m.Id} | {m.Title} | {m.Year}");
            break;

        case "2":
            Console.Write("Название: ");
            var title = Console.ReadLine();

            movieService.Add(new Movie
            {
                Title = title,
                Director = "Unknown",
                Year = 2025,
                Duration = 120,
                Genre = "Drama",
                Description = "Test"
            });

            Console.WriteLine("Фильм добавлен!");
            break;

        case "0":
            return;
    }
}
