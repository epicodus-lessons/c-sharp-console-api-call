using System;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiTest
{
  class Program
  {
    static void Main()
    {
      var apiCallTask = ApiHelper.ApiCall("YVCyI8rXwXtw0ii0ok294jqlBFWNPrMw");
      var result = apiCallTask.Result;

      JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(result);

      // Grabbing and saving the num_results property from the response object, while also grabbing and saving all nested objects defined in the Models/ directory: (1) the nested "results" key (corresponding to an array of Article objects) and within that (2) the nested "multimedia" key (corresponding to the array of Multimedia objects that belong to each Article object). 
      // I came up with the name "MetaData" for the model, because I thought it fit best. Ultimately, you can name your classes whatever you like, however, the C# class property names need to match the JSON key in the response object.  
      MetaData metaData = JsonConvert.DeserializeObject<MetaData>(jsonResponse.ToString()); 

      // displaying all of the Article and Multimedia objects from the API call
      foreach (Article article in metaData.Results)
      {
        Console.WriteLine($"Section: {article.Section}");
        Console.WriteLine($"Title: {article.Title}");
        Console.WriteLine($"Abstract: {article.Abstract}");
        Console.WriteLine($"Url: {article.Url}");
        Console.WriteLine($"Byline: {article.Byline}");
        foreach (Multimedia media in article.Multimedia)
        {
            Console.WriteLine("---------");
            Console.WriteLine($"MULTIMEDIA");
            Console.WriteLine($"Type: {media.Type}");
            Console.WriteLine($"SubType: {media.SubType}");
            Console.WriteLine($"Caption: {media.Caption}");
        }
        Console.WriteLine("______________________________________________________________________");
      } 
      // listing this at the bottom, because it gets lost in the console output if you leave it at the top!
      Console.WriteLine($"Num_results: {metaData.Num_Results}");
    }
  }

  class ApiHelper
  {
    public static async Task<string> ApiCall(string apiKey)
    {
      RestClient client = new RestClient("https://api.nytimes.com/svc/topstories/v2");
      RestRequest request = new RestRequest($"home.json?api-key={apiKey}", Method.GET);
      var response = await client.ExecuteTaskAsync(request);
      return response.Content;
    }
  }
}