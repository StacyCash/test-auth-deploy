using Microsoft.Owin;
using System.Text;
using System.Text.Json;

namespace SwaAuth.Api.Api;

public static class StaticWebAppApiAuthorization
{
    private static readonly JsonSerializerOptions options = new ()
	{
		PropertyNameCaseInsensitive = true
	};

	public static ClientPrincipal? ParseHttpHeaderForClientPrinciple(IHeaderDictionary headers)
    {
        ArgumentNullException.ThrowIfNull(headers);

        if (!headers.TryGetValue("x-ms-client-principal", out var header))
        {
            return null;
        }

        var data = header[0];
        if (data is null)
        {
            return null;
        }

        var decoded = Convert.FromBase64String(data);
        var json = Encoding.UTF8.GetString(decoded);
		var principal = JsonSerializer.Deserialize<ClientPrincipal>(
            json,
			options);

        return principal ?? null;
    }
}