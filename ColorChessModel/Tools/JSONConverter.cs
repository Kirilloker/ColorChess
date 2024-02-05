using Newtonsoft.Json;

namespace ColorChessModel
{
    public class JSONConverter
    {
        public static Map ConvertJSONtoMap(string JSON)
        {
            Map gameState = JsonConvert.DeserializeObject<Map>(JSON,
                new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

            return gameState;
        }

        public static Step ConvertJSONtoSTEP(string JSON)
        {
            Step step = JsonConvert.DeserializeObject<Step>(JSON,
            new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

            return step;
        }

        public static string ConvertToJSON(Map map)
        {
            return JsonConvert.SerializeObject(map, Formatting.Indented,
            new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
        }

        public static string ConvertToJSON(Step step)
        {
            return JsonConvert.SerializeObject(step, Formatting.Indented,
            new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
        }
    }
}
