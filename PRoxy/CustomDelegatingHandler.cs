using System.Text.RegularExpressions;
using System.Text;
using System.IO.Compression;
using HtmlAgilityPack;

namespace PRoxy;

public class CustomDelegatingHandler : DelegatingHandler
{
    private readonly BaseUrlProvider _baseUrlProvider;
    public CustomDelegatingHandler(BaseUrlProvider baseUrlProvider)
    {
        _baseUrlProvider = baseUrlProvider;

        InnerHandler = new HttpClientHandler();
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        if (response.Content.Headers.ContentType?.MediaType == "text/html" ||
            response.Content.Headers.ContentType?.MediaType == "text/plain; charset=utf-8")
        {
            // Read the content
            Stream responseStream = await response.Content.ReadAsStreamAsync();
            if (response.Content.Headers.ContentEncoding.Contains("gzip"))
            {
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                response.Content.Headers.ContentEncoding.Clear(); // Remove the Content-Encoding header
            }
            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(responseStream, Encoding.UTF8);
            foreach (var textNode in htmlDocument.DocumentNode.DescendantsAndSelf()
                         .Where(n => n.NodeType == HtmlNodeType.Text &&
                                     n.ParentNode.Name != "script" &&
                                     n.ParentNode.Name != "style"))
            {
                textNode.InnerHtml = Regex.Replace(textNode.InnerHtml, @"\b\w{6}\b", match => match.Value + "™");
            }

            foreach (var contentDiv in htmlDocument.DocumentNode.Descendants("div")
                         .Where(d => d.Id == "-post-rtjson-content"))
            {
                foreach (var paragraph in contentDiv.Descendants("p"))
                {
                    foreach (var textNode in paragraph.DescendantsAndSelf()
                                 .Where(n => n.NodeType == HtmlNodeType.Text))
                    {
                        textNode.InnerHtml = Regex.Replace(textNode.InnerHtml, @"\b\w{6}\b", match => match.Value + "™");
                    }
                }
            }

            // Replace the href attribute with proxy server URL
            foreach (var aTag in htmlDocument.DocumentNode.Descendants("a"))
            {
                var href = aTag.GetAttributeValue("href", "");
                if (Uri.TryCreate(href, UriKind.Absolute, out var uri))
                {
                    var newPathAndQuery = uri.PathAndQuery; // Keep the original path and query
                    var newUri = _baseUrlProvider.BaseUrl + newPathAndQuery;
                    aTag.SetAttributeValue("href", newUri);
                }
            }
            var modifiedContent = htmlDocument.DocumentNode.OuterHtml;
            var newContent = new StringContent(modifiedContent, Encoding.UTF8, "text/html");
            foreach (var header in response.Content.Headers)
            {
                newContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            response.Content = newContent;
        }
        return response;
    }
}