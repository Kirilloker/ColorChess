using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class GameStatistic
{
    public int Id { get; set; }
    public List<int>? PlayerScore { get; set; }
    public GameMode GameMode { get; set; }
    public List<int>? UsersId { get; set; }
}
public class IntListToStringConverter : ValueConverter<List<int>, string>
    {
        public IntListToStringConverter(ConverterMappingHints mappingHints = null)
            : base(
                  list => ConvertListToString(list),
                  str => ConvertStringToList(str),
                  mappingHints)
        {
        }

        private static string ConvertListToString(List<int> list)
        {
            return string.Join(",", list);
        }

        private static List<int> ConvertStringToList(string str)
        {
            return str.Split(',').Select(int.Parse).ToList();
        }
    }

