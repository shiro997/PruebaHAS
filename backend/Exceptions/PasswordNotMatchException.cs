namespace Exceptions
{
    public class PasswordNotMatchException : Exception
    {
        public string Message { get; set; }
        public PasswordNotMatchException(string message)
        {
            Message = message;
        }

    }
}
