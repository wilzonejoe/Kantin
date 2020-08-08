using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Core.Helpers
{
    public static class HttpResponseHelper
    {
        public static HttpResponseMessage GetHttpResponseMessageForBytes(byte[] bytes, string contentType, string fileName, DispositionType dispositionType)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(bytes);

            if (!string.IsNullOrEmpty(contentType))
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            if (!string.IsNullOrEmpty(fileName))
                fileName = fileName.Replace(",", "");

            var dispositionTypeString = dispositionType == DispositionType.Inline ? "inline" : "attachment";
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(dispositionTypeString) { FileName = fileName };
            return result;
        }
    }

    public enum DispositionType
    {
        Inline,
        Attachment
    }
}
