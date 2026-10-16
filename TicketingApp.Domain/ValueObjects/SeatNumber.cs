using System;

namespace TicketingApp.Domain.ValueObjects;

// C#'ta 'record' tipleri Value Object oluşturmak için biçilmiş kaftandır.
public record SeatNumber
{
    public string Value { get; init; }

    // Dışarıdan "new SeatNumber()" ile kafasına göre oluşturulmasını engelliyoruz
    private SeatNumber(string value)
    {
        Value = value;
    }

    // Nesnenin yaratılabileceği TEK kapı burası (Factory Method)
    public static SeatNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Koltuk numarası boş olamaz!");

        if (value.Length > 5)
            throw new ArgumentException("Koltuk numarası 5 karakterden uzun olamaz!");

        return new SeatNumber(value);
    }
}