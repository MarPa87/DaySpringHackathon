using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace DaySpringApp.Controllers
{
  public class GeoDataController : ApiController
  {
    // GET: GeoData
    public HttpResponseMessage Get(string postCode)
    {
      var client = new WebClient();
      var text = client.DownloadString($"http://maps.googleapis.com/maps/api/geocode/json?address={postCode}");
      var data = JsonConvert.DeserializeObject<ResponseWrapper>(text);
      if (data.Results.Length == 0) return Request.CreateResponse(HttpStatusCode.NotFound, "Invalid post code");
      var lat = data.Results[0].Geometry.Location.Lat;
      var lng = data.Results[0].Geometry.Location.Lng;
      text = client.DownloadString($"http://maps.googleapis.com/maps/api/geocode/json?latlng={lat},{lng}&sensor=false");
      var resultData = JsonConvert.DeserializeObject<SecondRequestResponseWrapper>(text);
      if (resultData.Results.Length == 0) return Request.CreateResponse(HttpStatusCode.NotFound, "Invalid post code");
      var numberPart = resultData.Results[0].AdressComponents.First(c => c.Types.Contains("street_number"));
      var streetPart = resultData.Results[0].AdressComponents.First(c => c.Types.Contains("route"));
  return Request.CreateResponse(HttpStatusCode.OK, $"{numberPart.LongName} {streetPart.LongName}");
    }

    internal struct ResponseWrapper
    {
      public ResponseData[] Results { get; set; }
    }

    internal struct ResponseData
    {
      public Geometry Geometry { get; set; }
    }
    internal struct Geometry
    {
      public Coordinates Location { get; set; }
    }

    internal struct Coordinates
    {
      public float Lat { get; set; }
      public float Lng { get; set; }
    }

    internal struct SecondRequestResponseWrapper
    {
      public SecondRequestResponse[] Results { get; set; }
    }
    internal struct SecondRequestResponse
    {
      [JsonProperty(PropertyName = "address_components")]
      public AdressComponent[] AdressComponents { get; set; }
    }
    internal struct AdressComponent
    {
      [JsonProperty(PropertyName = "long_name")]
      public string LongName { get; set; }
      [JsonProperty(PropertyName = "short_name")]
      public string ShortName { get; set; }
      [JsonProperty(PropertyName = "types")]
      public string[] Types { get; set; }
    }
  }


}
