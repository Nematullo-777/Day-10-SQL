using System;

namespace Domain.Entities;

public class Screening
{
    public int Id { get; set; }
    public int Movie_id { get; set; }
    public int Theater_id { get; set; }
    public DateTime Screening_time { get; set; }
    public decimal Ticket_price { get; set; }
    public int Available_seats { get; set; }
    public int MovieId { get; set; }
}
