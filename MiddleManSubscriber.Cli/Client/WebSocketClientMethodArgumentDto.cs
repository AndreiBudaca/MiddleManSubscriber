﻿namespace MiddleManSubscriber.Cli.Client
{
  public class WebSocketClientMethodArgumentDto
  {
    public string? Name { get; set; }

    public bool IsPrimitive { get; set; }

    public bool IsArray {  get; set; }

    public bool IsNullable {  get; set; }

    public string? Type { get; set; }

    public List<WebSocketClientMethodArgumentDto> Components { get; set; } = [];
  }
}
