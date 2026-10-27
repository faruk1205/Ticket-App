using System;
using MediatR;

namespace TicketingApp.Application.Abstractions.Idempotency;

public interface IIdempotentCommand<TResponse> : IRequest<TResponse>   
{
    Guid RequestId { get; }
}
//IIdempotentCommand<TResponse> ifadesinde TResponse, komutun çalışması sonucunda dönecek veri tipidir. 
//MediatR kütüphanesinde, bir komutun çalışması sonucunda bir veri dönecekse IRequest<TResponse> interface'i kullanılır. 
//Bu interface, komutun çalışması sonucunda TResponse tipinde bir veri döneceğini belirtir. 
