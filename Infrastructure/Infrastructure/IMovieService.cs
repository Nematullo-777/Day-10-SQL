using System;
using Domain.Entities;

namespace Infrastructure.Infrastructure;

public interface IMovieService
{
    public interface IMovieService
{
    List<Movie> GetAll();
    Movie GetById(int id);
    void Add(Movie movie);
    void Update(Movie movie);
    void Delete(int id);
}
}
