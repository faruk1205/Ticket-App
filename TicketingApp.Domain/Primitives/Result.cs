namespace TicketingApp.Domain.Primitives;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    // Başarılı bir sonuç yaratmak için
    public static Result Success() => new(true, Error.None);
    
    // Başarısız bir sonuç yaratmak için
    public static Result Failure(Error error) => new(false, error);
}