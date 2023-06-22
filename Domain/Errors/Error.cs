using FireplaceApi.Domain.Common;
using System.Text.Json.Serialization;

namespace FireplaceApi.Domain.Errors;

public class Error : BaseModel
{
    public int Code { get; set; }
    public ErrorType Type { get; set; }
    public FieldName Field { get; set; }
    public string ServerMessage { get; set; }
    public string ClientMessage { get; set; }
    public int HttpStatusCode { get; set; }
    public object Parameters { get; set; }
    [JsonIgnore]
    public Exception Exception { get; set; }

    public static Error InternalServerError { get; } = new Error
    (
        id: 37,
        code: 5000,
        type: ErrorType.INTERNAL_SERVER,
        field: FieldName.GENERAL,
        serverMessage: "Something went wrong on the server!",
        clientMessage: "Something went wrong on the server!",
        httpStatusCode: 500,
        creationDate: DateTime.UtcNow
    );

    public Error(ulong id, int code, ErrorType type,
        FieldName field, string clientMessage, int httpStatusCode,
        DateTime creationDate, DateTime? modifiedDate = null,
        string serverMessage = null, object parameters = null,
        Exception exception = null)
        : base(id, creationDate, modifiedDate)
    {
        Type = type;
        Code = code;
        Field = field;
        ClientMessage = clientMessage ?? throw new ArgumentNullException(nameof(clientMessage));
        HttpStatusCode = httpStatusCode;
        ServerMessage = serverMessage;
        Parameters = parameters;
        Exception = exception;
    }

    public Error PureCopy() => new(Id, Code, Type, Field,
        ClientMessage, HttpStatusCode, CreationDate, ModifiedDate,
        ServerMessage, Parameters, Exception);

    public void RemoveLoopReferencing()
    {

    }
}
