namespace BuyurtmaGo.Core.Models
{
    public class ErrorModel
    {
        public ErrorModel(string code)
        {
            this.Code = code;
        }

        public ErrorModel(string code, string? message)
        {
            this.Code = code;
            this.Message = message;
        }

        public ErrorModel(string code, string[] errors)
        {
            this.Code = code;
            this.Errors = errors;
        }

        public string Code { get; set; }

        public string? Message { get; set; }

        public string[] Errors { get; set; }
    }

    public class ErrorModel<E> : ErrorModel 
        where E : Enum
    {
        public ErrorModel(E code) : base(code.ToString())
        {
        }

        public ErrorModel(E code, string? message) : base(code.ToString(), message)
        {
        }

        public static implicit operator ErrorModel<E>(E code)
        {
            return new ErrorModel<E>(code);
        }
    }
}
