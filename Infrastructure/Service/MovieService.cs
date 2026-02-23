using System;

namespace Infrastructure.Service;

using Domain.Entities;
using global::Infrastructure.Infrastructure;
using Npgsql;

public class MovieService : IMovieService
{
    private readonly string _connectionString =
        @"Host=localhost;Port=5432;Username=postgres;Database=Cinema_db;Password=nemat1409";

    public List<Movie> GetAll()
    {
        var movies = new List<Movie>();

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var cmd = new NpgsqlCommand("SELECT * FROM movies", connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            movies.Add(new Movie
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Director = reader.GetString(2),
                Year = reader.GetInt32(3),
                Duration = reader.GetInt32(4),
                Genre = reader.GetString(5),
                Description = reader.GetString(6)
            });
        }

        return movies;
    }

    public void GetMovieWithMaxDuration()
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
        SELECT id, title, duration
        FROM movies
        WHERE duration = (SELECT MAX(duration) FROM movies)", con);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine();
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine("ID | Title | Duration (min)");
        Console.WriteLine("------------------------------------------------------------");

        while (reader.Read())
            Console.WriteLine($"{reader.GetInt32(0)} | {reader.GetString(1)} | {reader.GetInt32(2)}");

        Console.WriteLine("------------------------------------------------------------");
        con.Close();
    }

    public void GetMoviesInMultipleTheaters()
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
        SELECT m.title, COUNT(DISTINCT s.theater_id) AS theater_count
        FROM movies m
        JOIN screenings s ON m.id = s.movie_id
        GROUP BY m.title
        HAVING COUNT(DISTINCT s.theater_id) >= 2", con);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine();
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine("Movie Title | Theaters Count");
        Console.WriteLine("------------------------------------------------------------");

        while (reader.Read())
            Console.WriteLine($"{reader.GetString(0)} | {reader.GetInt64(1)}");

        Console.WriteLine("------------------------------------------------------------");
        con.Close();
    }

    public void GetTheatersWithSessionsMoreThan(int minSessions)
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
        SELECT th.name, COUNT(s.id) AS sessions_count
        FROM theaters th
        JOIN screenings s ON th.id = s.theater_id
        GROUP BY th.name
        HAVING COUNT(s.id) > @min", con);

        cmd.Parameters.AddWithValue("min", minSessions);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine();
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine("Theater Name | Sessions Count");
        Console.WriteLine("------------------------------------------------------------");

        while (reader.Read())
            Console.WriteLine($"{reader.GetString(0)} | {reader.GetInt64(1)}");

        Console.WriteLine("------------------------------------------------------------");
        con.Close();
    }

    public void GetRevenuePerMovie()
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
        SELECT m.title, COALESCE(SUM(t.price),0) AS total_revenue
        FROM movies m
        LEFT JOIN screenings s ON m.id = s.movie_id
        LEFT JOIN tickets t ON s.id = t.screening_id
        GROUP BY m.title", con);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine();
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine("Movie Title | Total Revenue");
        Console.WriteLine("------------------------------------------------------------");

        while (reader.Read())
            Console.WriteLine($"{reader.GetString(0)} | {reader.GetDecimal(1)}");

        Console.WriteLine("------------------------------------------------------------");
        con.Close();
    }

    public void GetScreeningsAboveAveragePrice()
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
        SELECT s.id, m.title, s.ticket_price, s.screening_time
        FROM screenings s
        JOIN movies m ON s.movie_id = m.id
        WHERE s.ticket_price > (SELECT AVG(ticket_price) FROM screenings)", con);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine();
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine("ID | Movie | Price | Screening Time");
        Console.WriteLine("------------------------------------------------------------");

        while (reader.Read())
            Console.WriteLine($"{reader.GetInt32(0)} | {reader.GetString(1)} | {reader.GetDecimal(2)} | {reader.GetDateTime(3):dd.MM.yyyy HH:mm}");

        Console.WriteLine("------------------------------------------------------------");
        con.Close();
    }

    public void GetTicketsCountPerScreening()
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
        SELECT s.id, m.title, COUNT(t.id)
        FROM screenings s
        JOIN movies m ON s.movie_id = m.id
        LEFT JOIN tickets t ON s.id = t.screening_id
        GROUP BY s.id, m.title", con);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine();
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine("Screening ID | Movie | Tickets Sold");
        Console.WriteLine("------------------------------------------------------------");

        while (reader.Read())
            Console.WriteLine($"{reader.GetInt32(0)} | {reader.GetString(1)} | {reader.GetInt64(2)}");

        Console.WriteLine("------------------------------------------------------------");
        con.Close();
    }

    public void GetAveragePricePerTheater()
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
        SELECT th.name, AVG(s.ticket_price)
        FROM theaters th
        JOIN screenings s ON th.id = s.theater_id
        GROUP BY th.name", con);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine();
        Console.WriteLine("------------------------------------------------------------");

        while (reader.Read())
            Console.WriteLine($"{reader.GetString(0)} | {Math.Round(reader.GetDecimal(1), 2)}");

        Console.WriteLine("------------------------------------------------------------");
        con.Close();
    }

    public void GetMoviesAboveAverageDuration()
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
        SELECT title, duration
        FROM movies
        WHERE duration > (SELECT AVG(duration) FROM movies)", con);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine();

        while (reader.Read())
            Console.WriteLine($"{reader.GetString(0)} | {reader.GetInt32(1)}");
        con.Close();
    }

    public void GetTheatersByMovie(string movieTitle)
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
        SELECT name
        FROM theaters
        WHERE id IN (
            SELECT theater_id
            FROM screenings
            WHERE movie_id = (
                SELECT id FROM movies WHERE title = @title
            )
        )", con);

        cmd.Parameters.AddWithValue("title", movieTitle);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine();

        while (reader.Read())
            Console.WriteLine(reader.GetString(0));
        con.Close();
    }

    public void GetSessionsCountPerTheater()
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
        SELECT th.name, COUNT(s.id)
        FROM theaters th
        LEFT JOIN screenings s ON th.id = s.theater_id
        GROUP BY th.name", con);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine();

        while (reader.Read())
            Console.WriteLine($"{reader.GetString(0)} | {reader.GetInt64(1)}");
        con.Close();
    }

    public void GetAverageTicketPricePerMovie()
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
        SELECT m.title, AVG(t.price)
        FROM movies m
        JOIN screenings s ON m.id = s.movie_id
        JOIN tickets t ON s.id = t.screening_id
        GROUP BY m.title", con);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine();

        while (reader.Read())
            Console.WriteLine($"{reader.GetString(0)} | {Math.Round(reader.GetDecimal(1), 2)}");
        con.Close();
    }

    public void GetAllTicketsDetailed()
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
        SELECT t.id, m.title, th.name, t.seat_number, t.customer_name, t.price
        FROM tickets t
        JOIN screenings s ON t.screening_id = s.id
        JOIN movies m ON s.movie_id = m.id
        JOIN theaters th ON s.theater_id = th.id", con);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine();

        while (reader.Read())
            Console.WriteLine($"{reader.GetInt32(0)} | {reader.GetString(1)} | {reader.GetString(2)} | {reader.GetString(3)} | {reader.GetString(4)} | {reader.GetDecimal(5)}");
        con.Close();
    }

    public void GetTheatersWithoutUpcomingSessions()
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
        SELECT name
        FROM theaters
        WHERE id NOT IN (
            SELECT theater_id
            FROM screenings
            WHERE screening_time BETWEEN NOW() AND NOW() + INTERVAL '7 days'
        )", con);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine();

        while (reader.Read())
            Console.WriteLine(reader.GetString(0));
        con.Close();
    }

    public void GetMoviesByYears(List<int> years)
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var yearsString = string.Join(",", years);

        var cmd = new NpgsqlCommand(
            $"SELECT title, year FROM movies WHERE year IN ({yearsString})", con);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine();

        while (reader.Read())
            Console.WriteLine($"{reader.GetString(0)} | {reader.GetInt32(1)}");
        con.Close();
    }

    public void GetScreeningsWithFreeSeats(int minSeats)
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
        SELECT s.id, m.title, s.available_seats, s.screening_time
        FROM screenings s
        JOIN movies m ON s.movie_id = m.id
        WHERE s.available_seats > @min", con);

        cmd.Parameters.AddWithValue("min", minSeats);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine();

        while (reader.Read())
            Console.WriteLine($"{reader.GetInt32(0)} | {reader.GetString(1)} | {reader.GetInt32(2)} | {reader.GetDateTime(3):dd.MM.yyyy HH:mm}");
        con.Close();
    }

    public void Add(Movie movie)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var sql = @"INSERT INTO movies 
                    (title, director, year, duration, genre, description)
                    VALUES (@title, @director, @year, @duration, @genre, @description)";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("title", movie.Title);
        cmd.Parameters.AddWithValue("director", movie.Director);
        cmd.Parameters.AddWithValue("year", movie.Year);
        cmd.Parameters.AddWithValue("duration", movie.Duration);
        cmd.Parameters.AddWithValue("genre", movie.Genre);
        cmd.Parameters.AddWithValue("description", movie.Description);

        cmd.ExecuteNonQuery();
    }

    public void Update(Movie movie)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var sql = @"UPDATE movies 
                    SET title=@title, director=@director, year=@year,
                        duration=@duration, genre=@genre, description=@description
                    WHERE id=@id";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("id", movie.Id);
        cmd.Parameters.AddWithValue("title", movie.Title);
        cmd.Parameters.AddWithValue("director", movie.Director);
        cmd.Parameters.AddWithValue("year", movie.Year);
        cmd.Parameters.AddWithValue("duration", movie.Duration);
        cmd.Parameters.AddWithValue("genre", movie.Genre);
        cmd.Parameters.AddWithValue("description", movie.Description);

        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var cmd = new NpgsqlCommand("DELETE FROM movies WHERE id=@id", connection);
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }

    public Movie? GetById(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var cmd = new NpgsqlCommand("SELECT * FROM movies WHERE id=@id", connection);
        cmd.Parameters.AddWithValue("id", id);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Movie
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Director = reader.GetString(2),
                Year = reader.GetInt32(3),
                Duration = reader.GetInt32(4),
                Genre = reader.GetString(5),
                Description = reader.GetString(6)
            };
        }

        return null;
    }

    public void AddMovie(Movie movie)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var sql = @"INSERT INTO movies 
                    (title, director, year, duration, genre, description)
                    VALUES (@title, @director, @year, @duration, @genre, @description)";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("title", movie.Title);
        cmd.Parameters.AddWithValue("director", movie.Director);
        cmd.Parameters.AddWithValue("year", movie.Year);
        cmd.Parameters.AddWithValue("duration", movie.Duration);
        cmd.Parameters.AddWithValue("genre", movie.Genre);
        cmd.Parameters.AddWithValue("description", movie.Description);

        cmd.ExecuteNonQuery();
    }
}