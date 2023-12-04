public static class DataDeliver
{
    public static string test() 
    {
        return GetReply($"{Config.ServerURL}/api/UserInfo/all_with_hash").Result;
    }

    public static string GetUserInfo(string userName) 
    {
        return GetReply($"{Config.ServerURL}/api/UserInfo/byName?userName={userName}").Result;
    }


    public static async Task<string> GetReply(string URL)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(URL);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else
                {
                    Console.WriteLine($"Ошибка: {response.StatusCode} - {response.ReasonPhrase}");
                    return "error";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return "error";
            }
        }
    }
}

