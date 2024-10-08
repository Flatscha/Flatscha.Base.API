namespace Flatscha.Base.API.Client.Extensions
{
    public static class UriExtension
    {
        public static Uri Append(this Uri uri, params string[] paths)
            => new(paths.Aggregate(uri.AbsoluteUri, (current, path) => string.Format("{0}/{1}", current.TrimEnd('/'), path.TrimStart('/'))));

        public static string GetUrlParameterString(params (string Name, object Value)[] parameter)
            => $"?{string.Join("&", parameter.Select(x => $"{x.Name}={GetString(x.Value)}"))}";

        private static string? GetString(object value) => value switch
        {
            DateTime x => x.ToString("yyyy-MM-dd HH:mm:ss"),
            _ => value.ToString(),
        };

        public static string SetUrlParameters(this string route, params (string Name, object Value)[] parameter) => $"{route}{GetUrlParameterString(parameter)}";
    }
}
