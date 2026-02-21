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
}