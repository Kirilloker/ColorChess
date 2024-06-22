using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GameServer.Tools
{
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

}
