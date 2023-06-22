using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Identifiers;
using FireplaceApi.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Interfaces;

public interface IErrorRepository
{
    public Task<List<Error>> ListErrorsAsync();
    public Task<Error> GetErrorAsync(ErrorIdentifier identifier);
    public Task<Error> CreateErrorAsync(ulong id, int code, ErrorType type,
        FieldName field, string clientMessage, int httpStatusCode);
    public Task<Error> UpdateErrorAsync(Error error);
    public Task DeleteErrorAsync(ErrorIdentifier identifier);
    public Task<bool> DoesErrorExistAsync(ErrorIdentifier identifier);
}
