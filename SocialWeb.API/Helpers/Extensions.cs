using Microsoft.AspNetCore.Http;

namespace SocialWeb.API.Helpers {

    public static class Extensions {
        public static void AddApplicationError (this HttpResponse, string message) {
            response.Headers.Add ("Application-Error", message);
            response.Headers.Add ("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add ("Access-Control-Allow-Origin", "*");
        }
    }
}