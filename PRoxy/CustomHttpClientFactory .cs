using Yarp.ReverseProxy.Forwarder;

namespace PRoxy;

sealed class CustomHttpClientFactory : ForwarderHttpClientFactory
{
    private readonly BaseUrlProvider _baseUrlProvider;

    public CustomHttpClientFactory(BaseUrlProvider baseUrlProvider)
    {
        _baseUrlProvider = baseUrlProvider;
    }
    protected override HttpMessageHandler WrapHandler(ForwarderHttpClientContext context, HttpMessageHandler handler)
    {
        return base.WrapHandler(context, new CustomDelegatingHandler(_baseUrlProvider));
    }
}