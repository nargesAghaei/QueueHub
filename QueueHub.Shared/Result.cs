namespace Shared;

public class Result
{
    protected Result(bool isSuccess, string message, Error? error)
    {
        if (isSuccess && error is not null)
            throw new InvalidOperationException("A successful result cannot have an error.");
        if (!isSuccess && error is null)
            throw new InvalidOperationException("A failed result must have an error.");

        IsSuccess = isSuccess;
        Message = message;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Message { get; }
    public Error? Error { get; }

    public static Result Success(string message = "") => new(true, message, null);

    public static Result Failed(Error error) => new(false, error.Message, error);
    public static Result Failed(string message) => Failed(Error.Failure(message));
}

public class Result<T> : Result
{
    private Result(bool isSuccess, string message, T? data, Error? error)
        : base(isSuccess, message, error)
    {
        Data = data;
    }

    public T? Data { get; }

    public static Result<T> Success(T data)
        => new(true, string.Empty, data, null);

    public static Result<T> Success(string message, T data)
        => new(true, message, data, null);

    public new static Result<T> Failed(Error error)
        => new(false, error.Message, default, error);

    public new static Result<T> Failed(string message)
        => Failed(Error.Failure(message));
}