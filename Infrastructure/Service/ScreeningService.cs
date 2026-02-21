using System;
using Domain.Entities;
using Infrastructure.Infrastructure;
using Npgsql;

namespace Infrastructure.Service;

public class ScreeningService : IScreeningService
{
    private readonly string _connectionString =
        @"Host=localhost;Port=5432;Username=postgres;Database=Cinema_db;Password=nemat1409";

    public List<Screening> GetAll()
    {
        var screenings = new List<Screening>();

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var cmd = new NpgsqlCommand("SELECT * FROM screenings", connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            screenings.Add(new Screening
            {
                Id = reader.GetInt32(0),
                Movie_id = reader.GetInt32(1),
                Theater_id = reader.GetInt32(2),
                Screening_time = reader.GetDateTime(3),
                Ticket_price = reader.GetDecimal(4),
                Available_seats = reader.GetInt32(5)
            });
        }

        return screenings;
    }

    public void Add(Screening screening)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var sql = @"INSERT INTO screenings 
                    (movie_id, theater_id, screening_time, ticket_price, available_seats)
                    VALUES (@movie_id, @theater_id, @screening_time, @ticket_price, @available_seats)";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("movie_id", screening.Movie_id);
        cmd.Parameters.AddWithValue("theater_id", screening.Theater_id);
        cmd.Parameters.AddWithValue("screening_time", screening.Screening_time);
        cmd.Parameters.AddWithValue("ticket_price", screening.Ticket_price);
        cmd.Parameters.AddWithValue("available_seats", screening.Available_seats);

        cmd.ExecuteNonQuery();
    }

    public void Update(Screening screening)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var sql = @"UPDATE screenings 
                    SET movie_id=@movie_id, theater_id=@theater_id, screening_time=@screening_time,
                        ticket_price=@ticket_price, available_seats=@available_seats
                    WHERE id=@id";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("id", screening.Id);
        cmd.Parameters.AddWithValue("movie_id", screening.Movie_id);
        cmd.Parameters.AddWithValue("theater_id", screening.Theater_id);
        cmd.Parameters.AddWithValue("screening_time", screening.Screening_time);
        cmd.Parameters.AddWithValue("ticket_price", screening.Ticket_price);
        cmd.Parameters.AddWithValue("available_seats", screening.Available_seats);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var cmd = new NpgsqlCommand("DELETE FROM screenings WHERE id=@id", connection);
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }

    public Screening? GetById(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var cmd = new NpgsqlCommand("SELECT * FROM screenings WHERE id=@id", connection);
        cmd.Parameters.AddWithValue("id", id);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Screening
            {
                Id = reader.GetInt32(0),
                Movie_id = reader.GetInt32(1),
                Theater_id = reader.GetInt32(2),
                Screening_time = reader.GetDateTime(3),
                Ticket_price = reader.GetDecimal(4),
                Available_seats = reader.GetInt32(5)
            };
        }

        return null;
    }
}
