using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;

namespace DaySpringApp.Helpers
{
    public class GoogleGeocoder
    {
        public string[] Geocode(string rawAddress)
        {
            const string UNKNOWN_ERROR = "UNKNOWN_ERROR";
            //const string URL = "https://maps.googleapis.com/maps/api/geocode/json?{0}&key=AIzaSyC5kdKI7-ZdfRKJ_9eAYMKxnlPK7dG0iQM";
            const string URL = "https://maps.googleapis.com/maps/api/geocode/json?{0}&key=AIzaSyDE6iuJIs4cVE3LNfRvEKSKv9eVyTRVJ0Y";

            var url = string.Format(URL, "address=" + rawAddress);

            string[] addresses = new string[4];

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET4.0C; .NET4.0E)";
                var jsonString = client.DownloadString(url);
                dynamic jsonObject = JsonConvert.DeserializeObject(jsonString);

                if (jsonObject.status == UNKNOWN_ERROR)
                {
                    throw new Exception(UNKNOWN_ERROR);
                }

                if (jsonObject.status == HttpStatusCode.OK
                    && (jsonObject.status != "ZERO_RESULTS"
                        || jsonObject.status != "OVER_QUERY_LIMIT"
                        || jsonObject.status != "REQUEST_DENIED"
                        || jsonObject.status != "INVALID_REQUEST"
                        || jsonObject.status != "UNKNOWN_ERROR"))
                // "ZERO_RESULTS" indicates that the geocode was successful but returned no results. This may occur if the geocoder was passed a non-existent address.
                // "OVER_QUERY_LIMIT" indicates that you are over your quota.
                // "REQUEST_DENIED" indicates that your request was denied.
                // "INVALID_REQUEST" generally indicates that the query (address, components or latlng) is missing.
                // "UNKNOWN_ERROR" indicates that the request could not be processed due to a server error. The request may succeed if you try again. 
                {
                    var result = jsonObject.results[0];

                    foreach (var component in result.address_components)
                    {
                        if (component.types.Count == 0)
                        {
                            continue;
                        }

                        switch (component.types[0].Value as string)
                        {
                            case "street_number":
                                addresses[0] = component.long_name;
                                break;
                            case "route":
                                addresses[0] += " " + component.long_name;
                                break;
                            case "country":
                                addresses[2] = component.long_name;
                                break;
                            case "postal_code":
                                addresses[3] = component.long_name;
                                break;
                        }
                    }
                }
            }

            int pos = rawAddress.IndexOf('#');
            if (pos > -1)
            {
                addresses[1] = rawAddress.Substring(pos, rawAddress.IndexOf(' ', pos) - pos);
            }

            return addresses;
        }
    }
}