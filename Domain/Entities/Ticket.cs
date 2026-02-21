using System;

namespace Domain.Entities;

public class Ticket
{
    public int Id { get; set; }
    public int ScreeningId { get; set; }
    public string CustomerName { get; set; }="";
    public string SeatNumber { get; set; }= "";
    public decimal Price { get; set; }
}
