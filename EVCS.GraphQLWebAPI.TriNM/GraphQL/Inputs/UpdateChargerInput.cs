namespace EVCS.GraphQLWebAPI.TriNM.GraphQL.Inputs
{
    public class UpdateChargerInput
    {
        public int ChargerTriNMId { get; set; }
        public int? StationTriNMId { get; set; }
        public string? ChargerTriNMType { get; set; }
        public bool? IsAvailable { get; set; }
        public string? ImageURL { get; set; }
    }
}

