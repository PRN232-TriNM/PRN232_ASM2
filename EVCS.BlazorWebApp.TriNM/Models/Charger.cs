using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace EVCS.BlazorWebApp.TriNM.Models
{
    public partial class ChargerTriNM
    {
        [JsonProperty("chargerTriNmid")]
        public int ChargerTriNMId { get; set; }

        [Required(ErrorMessage = "Station ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Station ID must be greater than 0.")]
        [JsonProperty("stationTriNmid")]
        public int StationTriNMId { get; set; }

        [Required(ErrorMessage = "Charger Type is required.")]
        [StringLength(50, ErrorMessage = "Charger Type cannot exceed 50 characters.")]
        [JsonProperty("chargerTriNmtype")]
        public string ChargerTriNMType { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }

        [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters.")]
        [JsonProperty("imageUrl")]
        public string? ImageURL { get; set; }

        public virtual StationTriNM? StationTriNM { get; set; }
    }
}
