namespace MusicStoreApp.Messages;

public class NotificationMessage(string message)
{
    public string Message { get; } = message;
}