using System;
using Domain.Entities;

namespace Infrastructure.Infrastructure;

public interface ITheaterService
{
    List<Theater> GetAll();
    Theater? GetById(int id);
    void Add(Theater theater);
    void Update(Theater theater);
    void Delete(int id);
}
