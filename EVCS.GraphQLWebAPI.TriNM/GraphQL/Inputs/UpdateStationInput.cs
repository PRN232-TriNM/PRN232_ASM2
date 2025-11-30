namespace EVCS.GraphQLWebAPI.TriNM.GraphQL.Inputs
{
    public class UpdateStationInput
    {
        public int StationId { get; set; }
        public string? StationCode { get; set; }
        public string? StationName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? Capacity { get; set; }
        public string? Owner { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        public bool? IsActive { get; set; }
    }
}
