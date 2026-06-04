namespace GymManagement.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public string Error { get; }
    public string ErrorKey { get; }

    protected Result(
        bool isSuccess,
        string error,
        string errorKey)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorKey = errorKey;
    }

    public static Result Success()
        => new(true, string.Empty, string.Empty);

    public static Result Failure(
        string error,
        string errorKey)
        => new(false, error, errorKey);
}

public class Result<T> : Result
{
    private readonly T? _value;

    public T Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException(
                "The value of a failure result cannot be accessed.");

    protected Result(
        T? value,
        bool isSuccess,
        string error,
        string errorKey)
        : base(isSuccess, error, errorKey)
    {
        _value = value;
    }

    public static Result<T> Success(T value)
        => new(value, true, string.Empty, string.Empty);

    public new static Result<T> Failure(
        string error,
        string errorKey)
        => new(default, false, error, errorKey);
}