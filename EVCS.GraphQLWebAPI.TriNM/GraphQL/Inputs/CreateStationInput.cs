namespace EVCS.GraphQLWebAPI.TriNM.GraphQL.Inputs
{
    public class CreateStationInput
    {
        public string StationCode { get; set; } = string.Empty;
        public string StationName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? City { get; set; }
        public string? Province { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int Capacity { get; set; }
        public string Owner { get; set; } = string.Empty;
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
    }
}
