using System;
using Domain.Entities;

namespace Infrastructure.Infrastructure;

public interface IReportService
{
    List<Movie> GetMoviesByGenre(string genre);
    List<string> GetUniqueDirectors();
    List<Screening> GetScreeningsSortedByTime();
    List<Movie> GetMoviesSortedByYearDesc();
    List<Screening> GetFirst5Screenings();
    void GetScreeningCountPerMovie();
    void GetTicketsByTheater(string theaterName);
    void GetTotalTicketsPerMovie();
    void GetTicketsByMovie(string movieTitle);
    void GetFullScreeningInfo();
}
