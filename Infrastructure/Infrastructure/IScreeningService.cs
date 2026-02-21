using System;
using Domain.Entities;

namespace Infrastructure.Infrastructure;

public interface IScreeningService
{
    List<Screening> GetAll();
    Screening? GetById(int id);
    void Add(Screening screening);
    void Update(Screening screening);
    void Delete(int id);
}
