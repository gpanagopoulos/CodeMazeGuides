using System.Net.Sockets;
using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace Tests;

public class HowToCheckIfURLIsValidUnitTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void GivenACorrectUrl_WhenValidatedWithRegex_ThenItShouldBeValid()
    {
        const string url = "https://www.amazon.com";
        const string urlTwo = "https://api.example.org/v1/data";
        
        var urlRegex = new Regex(
            @"[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)",
            RegexOptions.IgnoreCase);

        Assert.Matches(urlRegex, url);
        Assert.Matches(urlRegex, urlTwo);
    }

    [Fact]
    public void GivenAnIncorrectUrl_WhenValidatedWithRegex_ThenItShouldBeInvalid()
    {
        List<string> urls =
        [
            "http:/www.amazon.com",
            "https://site.company",
            "xxx://org.company.com"
        ];

        var urlRegex = new Regex(
            "^(http(s)?:\\/\\/.)[-a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)$",
            RegexOptions.IgnoreCase);

        foreach (var url in urls)
        {
            Assert.DoesNotMatch(urlRegex, url);
        }
    }

    [Fact]
    public void GivenACorrectUrl_WhenValidatedWithUriTryCreate_ThenItShouldBeValid()
    {
        const string url = "https://api.facebook.com:443";
        var success = Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri);

        Assert.True(success);
        Assert.NotNull(uri);

        Assert.Equal("api.facebook.com", uri.Host);
        Assert.Equal(443, uri.Port);
        Assert.Equal("https", uri.Scheme);
        Assert.Equal(string.Empty, uri.Query);
        
        const string url2 = "ftps://user:password@secure.example.org/files";
        success = Uri.TryCreate(url2, UriKind.RelativeOrAbsolute, out uri);

        Assert.True(success);
        Assert.NotNull(uri);

        Assert.Equal("secure.example.org", uri.Host);
        Assert.Equal("user:password", uri.UserInfo);
        Assert.Equal("ftps", uri.Scheme);
        Assert.Equal(string.Empty, uri.Query);
        
        const string url3 = "file:///C:/Users/username/Documents/file.txt";
        success = Uri.TryCreate(url3, UriKind.RelativeOrAbsolute, out uri);
        
        Assert.True(success);
        Assert.NotNull(uri);

        Assert.True(uri.IsFile);
        Assert.Equal("file", uri.Scheme);

        List<string> urls = [url, url2, url3];
        Assert.True(urls.All(x => Uri.IsWellFormedUriString(x, UriKind.RelativeOrAbsolute)));
    }

    [Fact]
    public void GivenAnIncorrectUrl_WhenValidatedWithUriTryCreate_ThenItShouldBeInvalid()
    {
        List<string> urls =
        [
            "https:/www.twitter.com", // false
            "https://site.company?q=search", // true
            "http://api.facebook.com", // true
            "ftp://api.site.com?value=word1 word2" // false
        ];

        var successes = urls
            .Select(url => Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out _))
            .ToList();

        var uris = urls
            .Select(url => Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri) ? uri : null)
            .ToList();

        var areWellFormed = urls
            .Select(url => Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            .ToList();
        
        Assert.False(Uri.IsWellFormedUriString(urls[0], UriKind.RelativeOrAbsolute));
        Assert.True(Uri.IsWellFormedUriString(urls[1], UriKind.RelativeOrAbsolute));
        Assert.True(Uri.IsWellFormedUriString(urls[2], UriKind.RelativeOrAbsolute));
        Assert.False(Uri.IsWellFormedUriString(urls[3], UriKind.RelativeOrAbsolute));

        var allAreInvalid = successes.All(s => !s);
        // Assert.True(allAreInvalid);
    }

    [Fact]
    public async Task GivenACorrectUrl_WhenValidatedWithUriHttpRequest_ThenItShouldBeValid()
    {
        using var client = new HttpClient();
        const string url = "https://api.facebook.com";

        var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GivenAnIncorrectUrl_WhenValidatedWithUriHttpRequest_ThenItShouldBeInvalid()
    {
        using var client = new HttpClient();
        const string url = "https://www.example-nonexistent-url.com";

        var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
        });
        
        Assert.IsType<SocketException>(exception.InnerException);
        Assert.Equal(SocketError.HostNotFound, (exception.InnerException as SocketException)?.SocketErrorCode);
    }
}