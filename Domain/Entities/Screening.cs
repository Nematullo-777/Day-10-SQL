using System;

namespace Domain.Entities;

public class Screening
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int TheaterId { get; set; }
    public DateTime ScreeningTime { get; set; }
    public decimal TicketPrice { get; set; }
    public int AvailableSeats { get; set; }
}
