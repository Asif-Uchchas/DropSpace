using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text;

namespace DropSpace.Helpers.LinkEncrypt
{
    public class EncryptDecryptQueryStringsMiddleware
    {
        private const string PARAMETER_NAME = "_mh";
        private const string ENCRYPTION_KEY = "key";
        private static ASCIIEncoding encoding;
        private static IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;
        private readonly RequestDelegate _next;
        public EncryptDecryptQueryStringsMiddleware(IHttpContextAccessor httpContextAccessor, IDataProtectionProvider provider, RequestDelegate next)
        {
            encoding = new ASCIIEncoding();
            _httpContextAccessor = httpContextAccessor;
            _protector = provider.CreateProtector("OMPEUHSESDeIcHrAeStAKNeSy");
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if (UriHelper.GetEncodedUrl(context.Request).Contains("?"))
            {
                string contextQuery = GetAbsoluteUri().Query.ToString();
                //
                if (contextQuery.Contains(PARAMETER_NAME))
                {
                    var enc = context.Request.Query[PARAMETER_NAME];
                    enc = _protector.Unprotect(enc);
                    QueryString queryString = new QueryString(enc);
                    context.Request.QueryString = queryString;
                }
                else if (context.Request.Method == "GET")
                {
                    if (contextQuery != "")
                    {
                        string encryptedQuery = _protector.Protect(contextQuery);
                        string redirectToPagePath = context.Request.Path.Value + "?" + PARAMETER_NAME + "=" + encryptedQuery;
                        context.Response.Redirect(redirectToPagePath);
                    }
                }
            }
            await _next.Invoke(context);
        }
        #region Utils
        private static Uri GetAbsoluteUri()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;
            uriBuilder.Path = request.Path.ToString();
            uriBuilder.Query = request.QueryString.ToString();
            return uriBuilder.Uri;
        }

        #endregion
    }
    public static class QueryStringMiddlewareExtensions
    {
        public static IApplicationBuilder UseEncryptDecryptQueryStringsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EncryptDecryptQueryStringsMiddleware>();
        }
    }

}
