namespace MiddleManSubscriber.Cli.Client
{
  public class WebSocketClientDataDto
  {
    public string? ConnectionId { get; set; }

    public List<WebSocketClientMethodDto> Methods { get; set; } = [];
  }
}
