using System;

namespace Domain.Entities;

public class Theater
{
    public int Id { get; set; }
    public string Name { get; set; }="";
    public string Location { get; set; }="";
    public string Manager { get; set; }="";
    public string Phone { get; set; }= "";
    public int Capacity { get; set; }
}
