using System;
using Domain.Entities;
using Infrastructure.Infrastructure;
using Npgsql;

namespace Infrastructure.Service;

public class TheaterService : ITheaterService
{
    private readonly string _connectionString =
        @"Host=localhost;Port=5432;Username=postgres;Database=Cinema_db;Password=nemat1409";

    public List<Theater> GetAll()
    {
        var theaters = new List<Theater>();

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var cmd = new NpgsqlCommand("SELECT * FROM theaters", connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            theaters.Add(new Theater
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Location = reader.GetString(2),
                Capacity = reader.GetInt32(3)
            });
        }

        return theaters;
    }

    public void Add(Theater theater)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var sql = @"INSERT INTO theaters 
                    (name, location, capacity)
                    VALUES (@name, @location, @capacity)";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("name", theater.Name);
        cmd.Parameters.AddWithValue("location", theater.Location);
        cmd.Parameters.AddWithValue("capacity", theater.Capacity);

        cmd.ExecuteNonQuery();
    }

    public void Update(Theater theater)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var sql = @"UPDATE theaters 
                    SET name=@name, location=@location, capacity=@capacity
                    WHERE id=@id";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("id", theater.Id);
        cmd.Parameters.AddWithValue("name", theater.Name);
        cmd.Parameters.AddWithValue("location", theater.Location);
        cmd.Parameters.AddWithValue("capacity", theater.Capacity);
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

    public Theater? GetById(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var cmd = new NpgsqlCommand("SELECT * FROM theaters WHERE id=@id", connection);
        cmd.Parameters.AddWithValue("id", id);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Theater
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Location = reader.GetString(2),
                Capacity = reader.GetInt32(3)
            };
        }

        return null;
    }
}
