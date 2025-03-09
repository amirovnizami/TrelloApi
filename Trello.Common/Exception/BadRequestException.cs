namespace Trello.Repository.Common.Exception;

public class BadRequestException:System.Exception
{
    public BadRequestException(string message):base(message)
    {
        
    }
}