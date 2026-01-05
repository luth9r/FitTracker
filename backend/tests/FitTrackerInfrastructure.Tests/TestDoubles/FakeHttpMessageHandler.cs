using System.Net;
using System.Text;

namespace FitTrackerInfrastructure.Tests.TestDoubles;

public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<HttpResponseMessage> _responses = new();

    public List<HttpRequestMessage> Requests { get; } = new();

    public void AddResponse(HttpResponseMessage response)
    {
        _responses.Enqueue(response);
    }

    public void AddResponse(HttpStatusCode statusCode, string content)
    {
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(content, Encoding.UTF8, "application/json"),
        };
        _responses.Enqueue(response);
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        Requests.Add(request);

        if (_responses.Count == 0)
        {
            return Task.FromResult(
                new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("No response configured"),
                });
        }

        return Task.FromResult(_responses.Dequeue());
    }
}
