namespace Trello.Common.Response;

public class Result
{
    public List<string> Messages { get; set; }

    public Result(List<string> messages)
    {
        Messages = messages;
    }
}