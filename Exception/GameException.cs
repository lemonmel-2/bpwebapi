using webapi.Enum;

namespace webapi.Exception
{
    public class GameException : System.Exception
    {
        public ErrorCode Code { get; }

        public GameException(ErrorCode errorCode) : base(errorCode.Message)
        {
            Code = errorCode;
        }
    }
}