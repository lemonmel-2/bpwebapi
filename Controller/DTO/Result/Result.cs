using webapi.Enum;

namespace webapi.Controller.DTO.Result
{
    public class Result<T>{
        public bool Success { get; set; }
        public string? Message { get; set; }

        public ErrorCode? Code { get; set; }
        public T? Data { get; set; }

        public static Result<T> Ok(T data, string? message = "success")
        {
            return new() {Success = true, Data = data, Message = message};
        }

        public static Result<T> Fail(ErrorCode code)
        {
            return new() {Success = false, Message = "error", Code = code};
        }

    }
}