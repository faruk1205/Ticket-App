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

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);      //default kullanılan türün varsayılan değerini ifade eder.
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error)     //internal aynı assambly içerisinden erişilebilir demek yani Application içerisindeyse Domain içeririsinden erişilemez.
        : base(isSuccess, error)                                              //base(isSuccess, error), üst sınıf olan Result sınıfının constructor'ını çağırır. Böylece IsSuccess ve Error bilgileri üst sınıfta ayarlanır.
    {
        _value = value;                                                       //satırı, generic sonuç nesnesinin kendi değerini saklar.
    }                                                                         //Yani bir constructor hem üst sınıfın başlangıç işlemlerini yapıyor hem de kendi sınıfına özgü alanı dolduruyor.

    public TValue Value => IsSuccess                                          //Bu method değil, property (özellik).Sonuç başarılıysa _value döndürülür.Sonuç başarısızsa InvalidOperationException fırlatılır.
        ? _value!
        : throw new InvalidOperationException("Başarısız bir sonucun değeri okunamaz.");

    public static implicit operator Result<TValue>(TValue? value) =>             //Bir TValue değerini otomatik olarak Result<TValue> türüne dönüştürür.
        value is not null ? Success(value) : Failure<TValue>(Error.None);
    
    /*örneğin:
     public Result<TicketResponse> GetTicket()
    {
        TicketResponse ticket = GetTicketFromDatabase();
        return ticket;
    }
    Buradaki return ticket; ifadesi, implicit operator sayesinde otomatik olarak Result<TicketResponse> sonucuna dönüştürülebilir.*/
}