public static class Utill 
{
    public static List<DateTime> GetDateTime(string date)
    {
        List<DateTime> result = new List<DateTime>();

        try
        {
            string[] dateStrings = date.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (string dateString in dateStrings)
            {
                if (DateTime.TryParse(dateString, out DateTime parsedDate))
                    result.Add(parsedDate);
                else
                    return new List<DateTime>();
            }

            if (result.Count == 1)
                result.Add(DateTime.Now);
        }
        catch (Exception)
        {
            return new List<DateTime>();
        }

        return result;
    }
}