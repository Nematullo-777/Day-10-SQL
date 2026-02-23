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
movieService.GetMovieWithMaxDuration();
movieService.AddMovie(new Movie { Title = "New Movie", Director = "Unknown", Year = 2025, Duration = 120, Genre = "Action", Description = "A new action movie." });
movieService.Delete(1);
movieService.Update(new Movie { Id = 2, Title = "Updated Movie", Director = "Updated Director", Year = 2024, Duration = 130, Genre = "Thriller", Description = "An updated thriller movie." });
movieService.GetAll();
movieService.GetById(2);
movieService.GetAllTicketsDetailed();
movieService.GetAveragePricePerTheater();
movieService.GetAverageTicketPricePerMovie();
movieService.GetMoviesAboveAverageDuration();
movieService.GetMoviesByYears(new List<int> { 1994, 1999, 2008, 2014 });
movieService.GetMoviesInMultipleTheaters();
