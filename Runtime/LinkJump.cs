using System;
using System.Linq;
using UnityEngine;

#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace org.ftxtool.lib
{
    public class LinkJump
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern bool LinkJump_CanOpenUrl(string url); 
#endif

#if UNITY_ANDROID
        private static bool LinkJump_CanOpenUrl_JNI(string url)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrWhiteSpace(url))
                return false;

            using (var linkJumpClass = new AndroidJavaClass("org.ftxtool.lib.LinkJumpNative"))
            {
                return linkJumpClass.CallStatic<bool>("CanOpenUrl", url);
            }
        
            return false;
        }
#endif

        public static bool CanOpenUrl(string url)
        {
#if UNITY_IOS
            return LinkJump_CanOpenUrl(url);
#elif UNITY_ANDROID
            return LinkJump_CanOpenUrl_JNI(url);
#else
            return false;
#endif
        }

        private static bool TryOpen(string url)
        {
            var canOpen = CanOpenUrl(url);
            if (canOpen)
                Application.OpenURL(url);

            return canOpen;
        }

        private static bool TryOpen(UriBuilder builder, string tryScheme)
        {
            builder.Scheme = tryScheme;
            return TryOpen(builder.Uri.AbsoluteUri);
        }

        private static bool TryOpenWithSchemes(UriBuilder builder, string[] trySchemes)
        {
            if (builder == null || trySchemes == null || trySchemes.Length == 0)
                return false;

            var scheme = builder.Scheme;

            return (from tryScheme in trySchemes
                where !string.IsNullOrEmpty(tryScheme) && !string.IsNullOrWhiteSpace(tryScheme)
                where !string.Equals(tryScheme, scheme, StringComparison.OrdinalIgnoreCase)
                where !string.Equals(tryScheme, "https", StringComparison.OrdinalIgnoreCase)
                where !string.Equals(tryScheme, "http", StringComparison.OrdinalIgnoreCase)
                select tryScheme).Any(tryScheme => TryOpen(builder, tryScheme));
        }

        /// <summary>
        /// Try open the url with another app.
        /// </summary>
        /// <param name="url">A full url that you want to open. e.g.: taobao://tb.cn/xxxxxx</param>
        /// <param name="trySchemes">If your url cannot be open, try others app schemes. e.g.: ["tmall", "taobao"]</param>
        /// <param name="openBrowser">If your url cannot be open, and donot have available scheme, then open the address with browser.</param>
        /// <param name="https">Whether use https when open the address with browser. If not then use http.</param>
        public static void Jump(string url, string[] trySchemes = null, bool openBrowser = true, bool https = true)
        {
            var uri = new Uri(url);

            var scheme = uri.Scheme;
            var host = uri.Host;
            var path = uri.AbsolutePath;

            var builder = new UriBuilder
            {
                Scheme = scheme, 
                Host = host, 
                Path = path
            };

            var originUrl = builder.Uri.AbsoluteUri;
            if (TryOpen(originUrl))
            {
                return;
            }

            if (trySchemes != null && trySchemes.Length > 0)
            {
                if (TryOpenWithSchemes(builder, trySchemes))
                {
                    return;
                }
            }

            if (!openBrowser)
                return;

            builder.Scheme = https ? "https" : "http";
            var httpsUrl = builder.Uri.AbsoluteUri;
            Application.OpenURL(httpsUrl);
        }
    }
}
