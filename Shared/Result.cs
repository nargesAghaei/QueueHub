namespace Shared;

public class Result
{
    protected Result(bool isSuccess, string message)
    {
        Message = message;
        IsSuccess = isSuccess;
    }
    public string Message { get; private set; }
    public bool IsSuccess { get; private set; }

    public static Result Success(string msg = "")
    {
        return new Result(true, msg);
    }
    public static Result Failed(string msg)
    {
        return new Result(false, msg);
    }
}


public class Result<T> : Result
{
    private Result(bool isSuccess, string message, T data) : base(isSuccess, message)
    {
        Data = data;
    }
    public T Data { get; set; }

    public static Result<T> Success(string message, T data)
    {
        return new Result<T>(true, message, data);
    }
    public new static Result<T> Failed(string message)
    {
        return new Result<T>(false, message,default);
    }
}