using System;
using Domain.Entities;
using Infrastructure.Infrastructure;
using Npgsql;

namespace Infrastructure.Service;

public class ReportService : IReportService
{
    private readonly string _connectionString =
        @"Host=localhost;Port=5432;Username=postgres;Database=Cinema_db;Password=nemat1409";
    public List<Movie> GetMoviesByGenre(string genre)
    {
        var list = new List<Movie>();

        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(
            "SELECT * FROM movies WHERE genre = @genre", con);
        cmd.Parameters.AddWithValue("genre", genre);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Movie
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

        return list;
    }

    public List<string> GetUniqueDirectors()
    {
        var list = new List<string>();

        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(
            "SELECT DISTINCT director FROM movies ORDER BY director", con);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(reader.GetString(0));

        return list;
    }

    public List<Screening> GetScreeningsSortedByTime()
    {
        var list = new List<Screening>();

        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(
            "SELECT * FROM screenings ORDER BY screening_time", con);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Screening
            {
                Id = reader.GetInt32(0),
                Movie_id = reader.GetInt32(1),
                Theater_id = reader.GetInt32(2),
                Screening_time = reader.GetDateTime(3),
                Ticket_price = reader.GetDecimal(4),
                Available_seats = reader.GetInt32(5)
            });
        }

        return list;
    }

    public List<Movie> GetMoviesSortedByYearDesc()
    {
        var list = new List<Movie>();

        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(
            "SELECT * FROM movies ORDER BY year DESC", con);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Movie
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

        return list;
    }

    public List<Screening> GetFirst5Screenings()
    {
        var list = new List<Screening>();

        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(
            "SELECT * FROM screenings ORDER BY screening_time LIMIT 5", con);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Screening
            {
                Id = reader.GetInt32(0),
                Movie_id = reader.GetInt32(1),
                Theater_id = reader.GetInt32(2),
                Screening_time = reader.GetDateTime(3),
                Ticket_price = reader.GetDecimal(4),
                Available_seats = reader.GetInt32(5)
            });
        }

        return list;
    }

    public void GetScreeningCountPerMovie()
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
            SELECT m.title, COUNT(s.id)
            FROM movies m
            LEFT JOIN screenings s ON m.id = s.movie_id
            GROUP BY m.title", con);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"{reader.GetString(0)} — {reader.GetInt64(1)} сеансов");
    }

    public void GetTicketsByTheater(string theaterName)
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
            SELECT t.customer_name, t.seat_number
            FROM tickets t
            JOIN screenings s ON t.screening_id = s.id
            JOIN theaters th ON s.theater_id = th.id
            WHERE th.name = @name", con);

        cmd.Parameters.AddWithValue("name", theaterName);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"{reader.GetString(0)} — Место: {reader.GetString(1)}");
    }

    public void GetTotalTicketsPerMovie()
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
            SELECT m.title, COUNT(t.id)
            FROM movies m
            LEFT JOIN screenings s ON m.id = s.movie_id
            LEFT JOIN tickets t ON s.id = t.screening_id
            GROUP BY m.title", con);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"{reader.GetString(0)} — {reader.GetInt64(1)} билетов");
    }

    public void GetTicketsByMovie(string movieTitle)
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
            SELECT t.customer_name, t.seat_number
            FROM tickets t
            JOIN screenings s ON t.screening_id = s.id
            JOIN movies m ON s.movie_id = m.id
            WHERE m.title = @title", con);

        cmd.Parameters.AddWithValue("title", movieTitle);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"{reader.GetString(0)} — {reader.GetString(1)}");
    }

    public void GetFullScreeningInfo()
    {
        using var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand(@"
            SELECT m.title, s.screening_time, th.name
            FROM screenings s
            JOIN movies m ON s.movie_id = m.id
            JOIN theaters th ON s.theater_id = th.id
            ORDER BY s.screening_time", con);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"{reader.GetString(0)} | {reader.GetDateTime(1)} | {reader.GetString(2)}");
    }
}
