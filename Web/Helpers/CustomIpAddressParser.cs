using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using WebApiThrottle.Net;

namespace Web.Helpers
{
    public class CustomIpAddressParser : DefaultIpAddressParser
    {
        public override IPAddress GetClientIp(HttpRequestMessage request)
        {
            const string customHeaderName = "true-client-ip";

            if (request.Headers.Contains(customHeaderName))
            {
                IEnumerable<string> headerValues;

                if (request.Headers.TryGetValues(customHeaderName, out headerValues))
                {
                    if (headerValues.Any())
                    {
                        return ParseIp(headerValues.FirstOrDefault().Trim());
                    }
                }
            }

            return base.GetClientIp(request);
        }
    }
}