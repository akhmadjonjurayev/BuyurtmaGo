namespace BuyurtmaGo.Core.Authentications.Models
{
    public class ErrorModel
    {
        public ErrorModel(string code)
        {
            Code = code;
        }

        public ErrorModel(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public ErrorModel(string code, string[] errors)
        {
            Code = code;
            Errors = errors;
        }

        public string Code { get; set; }

        public string Message { get; set; }

        public string[] Errors { get; set; }
    }

    public class ErrorModel<E> : ErrorModel
        where E : Enum
    {
        public ErrorModel(E code) : base(code.ToString())
        {
        }

        public ErrorModel(E code, string message) : base(code.ToString(), message)
        {
        }

        public static implicit operator ErrorModel<E>(E code)
        {
            return new ErrorModel<E>(code);
        }
    }
}
