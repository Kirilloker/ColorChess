using Newtonsoft.Json;
using System.Net;
using System.Text.Json;

public static class DataDeliver
{
    public static ResponseContentStatus test() 
    {
        return GetReply($"{Config.ServerURL}/api/UserInfo/all_with_hash").Result;
    }

    public static ResponseContentStatus GetUserInfo(string userName) 
    {
        return GetReply($"{Config.ServerURL}/api/UserInfo/byName?userName={userName}").Result;
    }

    public static ResponseContentStatus GetEventTable(DateTime startPeriod, DateTime endPeriod, string listTypeEvent)
    {
        string formattedStartPeriod = startPeriod.ToString("yyyy-MM-dd HH:mm:ss");
        string formattedEndPeriod = endPeriod.ToString("yyyy-MM-dd HH:mm:ss");
        string formattedListTypeEvent = Uri.EscapeDataString(listTypeEvent);

        string url = $"{Config.ServerURL}/api/TableEvent/period_types?startPeriod={formattedStartPeriod}&endPeriod={formattedEndPeriod}&listTypeEvent={formattedListTypeEvent}";

        return GetReply(url).Result;
    }




    public static async Task<ResponseContentStatus> GetReply(string URL)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(URL);
                string responseBody = await response.Content.ReadAsStringAsync();

                return new ResponseContentStatus((ServerStatus)response.StatusCode, responseBody);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return new ResponseContentStatus(ServerStatus.NotFound, "Ошибка");
            }
        }
    }


}

