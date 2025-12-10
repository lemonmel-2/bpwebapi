namespace webapi.Enum
{
    public class ErrorCode
    {
        public string Code { get; }
        public string Message { get; }

        private ErrorCode(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public static readonly ErrorCode SESSION_INVALID = new ErrorCode("SESSION_INVALID", "session is invalid. Please try log in again.");
        public static readonly ErrorCode PARAM_ILLEGAL = new ErrorCode("PARAM_ILLEGAL", "input parameter is invalid.");
        public static readonly ErrorCode USER_NOT_EXIST = new ErrorCode("USER_NOT_EXIST", "user not exist.");
        public static readonly ErrorCode USER_ID_EXIST = new ErrorCode("USER_ID_EXIST", "user ID used, change another one.");
        public static readonly ErrorCode INCORRECT_PASSWORD = new ErrorCode("INCORRECT_PASSWORD", "password is incorrect.");
        public static readonly ErrorCode INVALID_ITEM = new ErrorCode("INVALID_ITEM", "item not exist.");
        public static readonly ErrorCode DATABASE_ERROR = new ErrorCode("DATABASE_ERROR", "database error, data not saved.");
        public static readonly ErrorCode INSUFFICIENT_POINTS = new ErrorCode("INSUFFICIENT_POINTS", "Oh no! You don't have enough points, play more to earn!");
    }
}