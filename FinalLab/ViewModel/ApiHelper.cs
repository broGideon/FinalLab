using System.Net;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Windows;
using System.Security.Policy;
using Newtonsoft.Json;
namespace FinalLab.ViewModel;

public static class ApiHelper
{
    private static string _url = "http://93.185.159.39:5000/api";
    
    public static T? Get<T>(string model, long id = 0) 
    {
        HttpClient client = new HttpClient();
        string request = id == 0 ? $"{model}" : $"{model}/{id}";
        HttpResponseMessage response = client.GetAsync($"{_url}/{model}/{(id != 0 ? id : string.Empty)}").Result;
        if (response.StatusCode != HttpStatusCode.OK) return default;
        return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
    }
    
    public static bool Put(string json, string model, int id)
    {
        HttpClient client = new HttpClient();
        HttpContent body = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = client.PutAsync($"{_url}/{model}/{id}", body).Result;
        if (response.StatusCode == HttpStatusCode.Created) return true;
        else return false;
    }

    public static bool Post(string json, string model)
    {
        HttpClient client = new HttpClient();
        HttpContent body = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = client.PostAsync($"{_url}/{model}", body).Result;
        if (response.StatusCode == HttpStatusCode.NoContent) return true;
        else return false;
    }

    public static bool Delete(string model, int id)
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = client.DeleteAsync($"{_url}/{model}/{id}").Result;
        if (response.StatusCode == HttpStatusCode.NoContent) return true;
        else return false;
    }
}
