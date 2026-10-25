using System;
using TicketingApp.Domain.Entities;
using Xunit;

namespace TicketingApp.Domain.UnitTests.Events;

public class EventTests
{
    [Fact]
    public void Create_Should_ReturnSuccess_When_ParametersAreValid()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Rock Konseri";
        var location = "İstanbul BJK Stadyumu";
        var startDate = DateTime.UtcNow.AddDays(7);
        var endDate = startDate.AddHours(3);

        // Act
        var result = Event.Create(id, name, location, startDate, endDate);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(name, result.Value.Name);
        Assert.Equal(location, result.Value.Location);
        Assert.True(result.Value.IsActive);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_EndDateIsBeforeStartDate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Tiyatro";
        var location = "Kadıköy Sahne";
        var startDate = DateTime.UtcNow.AddDays(5);
        var endDate = startDate.AddDays(-1); // Hatalı tarih

        // Act
        var result = Event.Create(id, name, location, startDate, endDate);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Event.InvalidDates", result.Error.Code);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_NameIsEmpty()
    {
        // Arrange
        var id = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(2);
        var endDate = startDate.AddHours(2);

        // Act
        var result = Event.Create(id, string.Empty, "Ankara Sahne", startDate, endDate);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Event.EmptyName", result.Error.Code);
    }

    [Fact]
    public void Cancel_Should_ReturnSuccess_When_EventIsActive()
    {
        // Arrange
        var eventItem = Event.Create(
            Guid.NewGuid(), 
            "Stand-up Gösterisi", 
            "Kültür Merkezi", 
            DateTime.UtcNow.AddDays(3), 
            DateTime.UtcNow.AddDays(3).AddHours(2)).Value;

        // Act
        var result = eventItem.Cancel();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(eventItem.IsActive);
    }

    [Fact]
    public void Cancel_Should_ReturnFailure_When_EventIsAlreadyCancelled()
    {
        // Arrange
        var eventItem = Event.Create(
            Guid.NewGuid(), 
            "Stand-up Gösterisi", 
            "Kültür Merkezi", 
            DateTime.UtcNow.AddDays(3), 
            DateTime.UtcNow.AddDays(3).AddHours(2)).Value;

        eventItem.Cancel(); // İlk iptal

        // Act
        var result = eventItem.Cancel(); // İkinci iptal denemesi

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Event.AlreadyCancelled", result.Error.Code);
    }
}
