using System.Net;

public class ResponseContentStatus 
{
    public readonly ServerStatus statusCode;
    public readonly string content;

    public ResponseContentStatus(ServerStatus statusCode, string content) 
    {
        this.statusCode = statusCode;
        this.content = content;
    }
}