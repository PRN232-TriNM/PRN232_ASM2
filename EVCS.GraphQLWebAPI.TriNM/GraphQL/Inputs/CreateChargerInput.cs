namespace EVCS.GraphQLWebAPI.TriNM.GraphQL.Inputs
{
    public class CreateChargerInput
    {
        public int StationTriNMId { get; set; }
        public string ChargerTriNMType { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public string? ImageURL { get; set; }
    }
}

