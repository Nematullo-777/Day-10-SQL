using System;
using Domain.Entities;

namespace Infrastructure.Infrastructure;

public interface ITicketService
{
    List<Ticket> GetAll();
    Ticket? GetById(int id);
    void Add(Ticket ticket);
    void Update(Ticket ticket);
    void Delete(int id);
}
