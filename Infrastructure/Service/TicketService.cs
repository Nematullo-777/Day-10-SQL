using System;
using Domain.Entities;
using Infrastructure.Infrastructure;
using Npgsql;

namespace Infrastructure.Service;

public class TicketService : ITicketService
{
    private readonly string _connectionString =
        @"Host=localhost;Port=5432;Username=postgres;Database=Cinema_db;Password=nemat1409";

    public List<Ticket> GetAll()
    {
        var tickets = new List<Ticket>();

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var cmd = new NpgsqlCommand("SELECT * FROM tickets", connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            tickets.Add(new Ticket
            {
                Id = reader.GetInt32(0),
                Screening_id = reader.GetInt32(1),
                SeatNumber = reader.GetString(2),
                Price = reader.GetDecimal(3)
            });
        }

        return tickets;
    }

    public void Add(Ticket ticket)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var sql = @"INSERT INTO tickets 
                    (screening_id, seat_number, price)
                    VALUES (@screening_id, @seat_number, @price)";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("screening_id", ticket.Screening_id);
        cmd.Parameters.AddWithValue("seat_number", ticket.SeatNumber);
        cmd.Parameters.AddWithValue("price", ticket.Price);

        cmd.ExecuteNonQuery();
    }

    public void Update(Ticket ticket)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var sql = @"UPDATE tickets 
                    SET screening_id=@screening_id, seat_number=@seat_number, price=@price
                    WHERE id=@id";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("id", ticket.Id);
        cmd.Parameters.AddWithValue("screening_id", ticket.Screening_id);
        cmd.Parameters.AddWithValue("seat_number", ticket.SeatNumber);
        cmd.Parameters.AddWithValue("price", ticket.Price);

        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var cmd = new NpgsqlCommand("DELETE FROM tickets WHERE id=@id", connection);
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }

    public Ticket? GetById(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var cmd = new NpgsqlCommand("SELECT * FROM movies WHERE id=@id", connection);
        cmd.Parameters.AddWithValue("id", id);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Ticket
            {
                Id = reader.GetInt32(0),
                Screening_id = reader.GetInt32(1),
                SeatNumber = reader.GetString(2),
                Price = reader.GetDecimal(3)
            };
        }

        return null;
    }
}
