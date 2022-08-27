namespace Dynastic.Common;

public class ErrorDetails
{
    public List<Error> Errors { get; set; }
}

public class Error
{
    public string Message { get; set; }
}