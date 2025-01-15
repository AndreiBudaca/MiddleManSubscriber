using Microsoft.AspNetCore.SignalR.Client;
using MiddleManSubscriber.Cli.Client;

namespace MiddleManSubscriber.Cli
{
  internal class Program
  {
    public class Triangle
    {
      public Point Point1 { get; set; }
      public Point Point2 { get; set; }
      public Point Point3 { get; set; }
    }

    public class Point
    {
      public float X { get; set; }
      public float Y { get; set; }
    }

    static async Task Main(string[] args)
    {
      var jwt = "{{redacted}}";

      HubConnection connection = new HubConnectionBuilder()
        .WithUrl(new Uri("wss://localhost:7133/playground"), options =>
        {
          options.Headers.Add("Authorization", $"Bearer {jwt}");
        })
        .WithAutomaticReconnect()
        .Build();

      connection.On("MakeTriangle", (List<float> values) =>
      {
        return Task.FromResult<List<Triangle>>(
          [
            new Triangle 
            { 
              Point1 = new Point { X = values[0], Y = values[1] },
              Point2 = new Point { X = values[2], Y = values[3] },
              Point3 = new Point { X = values[4], Y = values[5] } 
            }, 
            new Triangle
            {
              Point1 = new Point { X = values[0], Y = values[1] },
              Point2 = new Point { X = values[2], Y = values[3] },
              Point3 = new Point { X = values[4], Y = values[5] }
            }
          ]);
      });

      var endProgram = false;
      connection.Closed += (ex) => { endProgram = true; return Task.CompletedTask; };

      await connection.StartAsync();

      await connection.InvokeAsync("AddMethodInfo", new WebSocketClientMethodDto[]
      {
        new WebSocketClientMethodDto
        {
          Name = "MakeTriangle",
          Arguments = new List<WebSocketClientMethodArgumentDto>
          {
            new WebSocketClientMethodArgumentDto { Name = "numbers", IsPrimitive = true, IsArray = true, Type = "float" },
          },
          Returns = new WebSocketClientMethodArgumentDto 
          {
            Name = "Triangle",
            IsArray = true,
            Components = 
            [
              new WebSocketClientMethodArgumentDto 
              {
                Name = "Point1",
                Components =
                [
                  new WebSocketClientMethodArgumentDto { Name = "X", IsPrimitive = true, Type = "float" },
                  new WebSocketClientMethodArgumentDto { Name = "Y", IsPrimitive = true, Type = "float" },
                ]
              },
              new WebSocketClientMethodArgumentDto
              {
                Name = "Point2",
                Components =
                [
                  new WebSocketClientMethodArgumentDto { Name = "X", IsPrimitive = true, Type = "float" },
                  new WebSocketClientMethodArgumentDto { Name = "Y", IsPrimitive = true, Type = "float" },
                ]
              },
              new WebSocketClientMethodArgumentDto
              {
                Name = "Point3",
                Components =
                [
                  new WebSocketClientMethodArgumentDto { Name = "X", IsPrimitive = true, Type = "float" },
                  new WebSocketClientMethodArgumentDto { Name = "Y", IsPrimitive = true, Type = "float" },
                ]
              },
            ]
          }

        },
      });

      while (!endProgram) { Thread.Sleep(1000); }
    }
  }
}
