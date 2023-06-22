namespace FireplaceApi.Domain.Errors;

public abstract class ApiException : Exception
{
    public ErrorType ErrorType { get; private set; }
    public FieldName ErrorField { get; private set; }
    public string ErrorServerMessage { get; set; }
    public object Parameters { get; set; }
    public Exception Exception { get; set; }
    public ErrorIdentifier ErrorIdentifier { get; private set; }

    public ApiException(ErrorType errorType, FieldName errorField,
        string errorServerMessage = null, object parameters = null,
        Exception systemException = null)
        : base(errorServerMessage, systemException)
    {
        ErrorType = errorType;
        ErrorField = errorField;
        ErrorIdentifier = ErrorIdentifier.OfTypeAndField(ErrorType, ErrorField);
        ErrorServerMessage = errorServerMessage;
        Parameters = parameters;
        Exception = systemException ?? this;
    }
}
