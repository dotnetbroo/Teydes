using System.Text.RegularExpressions;

namespace Teydes.Web.Models;

public class ConfigureApiUrlName : IOutboundParameterTransformer
{
    public string TransformOutbound(Object value)
        => value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
}
