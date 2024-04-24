﻿namespace TH.Services.Models;

public class Teacher
{
    public double Rate { get; set; }
    public TotalHours AmountHoursBudget = new TotalHours();
    public TotalHours AmountHoursCommercial = new TotalHours();

    public Teacher() { }
    public Teacher(double rate)
    {
        Rate = rate;
    }
    public void AddAmountHoursBudget(TotalHours totalHours)
    {
        AmountHoursBudget.Add(totalHours);
    }
    public void AddAmountHoursCommercial(TotalHours totalHours)
    {
        AmountHoursCommercial.Add(totalHours);
    }
}
