using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AspNetWebApiWithDbContext
{
    public class Result
    {
        public Result(bool isSuccess, Error error,string traceId = "")
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException();
            }

            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException();
            }

            IsSuccess = isSuccess;
            Error = error;
            TraceId = string.IsNullOrEmpty(traceId?.Trim()) ? DateTime.Now.Ticks.ToString() : traceId;
        }

        public string TraceId { get; set; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        public static Result Success(string traceId = "") => new Result(true, Error.None,traceId);
        public static Result Failure(Error error, string traceId = "") => new Result(false, error,traceId);
        public static Result<TValue> Success<TValue>(TValue value,string traceId = "") => new Result<TValue>(value, true, Error.None,traceId);
        public static Result<TValue> Failure<TValue>(Error error, string traceId = "") => new Result<TValue>(default, false, error,traceId);

        public static Result<TValue> Create<TValue>(TValue? value, string traceId = "") =>
            value is not null ? Success(value,traceId) : Failure<TValue>(Error.NullValue,traceId);


    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;
        public Result(TValue? value, bool isSuccess, Error error, string traceId = "") : base(isSuccess, error,traceId)
        {
            _value = value;
        }
        [NotNull]
        public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("The value of a failure result can not be accessed.");

        public static implicit operator Result<TValue>(TValue? value) => Create(value);



    }

    public record Error(string Code, string Description)
    {
        public override string ToString()
        {
            return $"{Code}:{Description}";
        }
        public static Error None = new Error(string.Empty, string.Empty);
        public static Error NullValue = new Error("Error.NullValue", "Null value was provided");
        public static Error ArgumentValidationError = new Error("Error.ArgumentValidationError", "Argument validation error");
        public static Error NotFoundError = new Error("Error.NotFoundError", "Not Found");
        public static Error OperationError = new Error("Error.OperationError", "Operation error occured");

    }
}
