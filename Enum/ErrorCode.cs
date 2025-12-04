namespace webapi.Enum
{
    public class ErrorCode
    {
        public string Message { get; }

        private ErrorCode(string message)
        {
            this.Message = message;
        }

        public static readonly ErrorCode PARAM_ILLEGAL = new ErrorCode("input parameter is invalid.");
        public static readonly ErrorCode USER_NOT_EXIST = new ErrorCode("user not exist.");
        public static readonly ErrorCode USER_ID_EXIST = new ErrorCode("user ID used, change another one.");
        public static readonly ErrorCode INCORRECT_PASSWORD = new ErrorCode("password is incorrect.");
        public static readonly ErrorCode INVALID_ITEM = new ErrorCode("item not exist.");
        public static readonly ErrorCode DATABASE_ERROR = new ErrorCode("database error, data not saved.");
    }
}