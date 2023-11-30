using GameServer.Database.DTO;

public static class Converter
{
    public static LogEventDTO EntityToDTO(LogEvent resource) 
    {
        return new() 
        {
            Date = resource.Date,
            Type_Event = resource.Type_Event,
            UsersId = resource.UsersId,
            Description = resource.Description
        };
    }

    public static List<LogEventDTO> List_EntityToDTO(List<LogEvent> resource) 
    {
        List<LogEventDTO> resourceDTO = new(resource.Count);
        foreach (var item in resource)
            resourceDTO.Add(EntityToDTO(item));
        return resourceDTO;
    }

}
