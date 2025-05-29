namespace BuyurtmaGo.Core.Models
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }

        public ErrorModel? Error { get; set; }
    }

    public class OperationResult<T> : OperationResult
    {
        public T? Data { get; set; }

        public OperationResult(ErrorModel errorModel)
        {
            this.IsSuccess = false;
            this.Error = errorModel;
        }

        public OperationResult(T data)
        {
            this.IsSuccess = true;
            this.Data = data;
        }

        public static implicit operator OperationResult<T>(T data)
        {
            return new OperationResult<T>(data);
        }

        public static implicit operator OperationResult<T>(ErrorModel error)
        {
            return new OperationResult<T>(error);
        }
    }

    public class OperationResult<T, E> : OperationResult<T>
        where E : Enum
    {
        public OperationResult(E code) : base(new ErrorModel<E>(code))
        {
        }

        public OperationResult(T data) : base(data)
        {
        }

        public OperationResult(E code, string? message) : base(new ErrorModel<E>(code, message))
        {
        }

        public static implicit operator OperationResult<T, E>(E code)
        {
            return new OperationResult<T, E>(code);
        }

        public static implicit operator OperationResult<T, E>(T data)
        {
            return new OperationResult<T, E>(data);
        }
    }
}
