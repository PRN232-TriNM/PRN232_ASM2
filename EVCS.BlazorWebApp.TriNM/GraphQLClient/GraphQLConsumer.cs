using EVCS.BlazorWebApp.TriNM.Models;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace EVCS.BlazorWebApp.TriNM.GraphQLClient
{
    public class GraphQLConsumer
    {
        private readonly IGraphQLClient _graphQLClient;

        public GraphQLConsumer(IGraphQLClient graphQLClient)
        {
            _graphQLClient = graphQLClient;
        }

        public async Task<List<StationTriNM>> GetStationsAsync()
        {
            try
            {
                var query = @"
                    query {
                       getStations {
                           stationTriNmid
                           stationTriNmcode
                           stationTriNmname
                           address
                           city
                           province
                           latitude
                           longitude
                           capacity
                           currentAvailable
                           owner
                           contactPhone
                           contactEmail
                           description
                           createdDate
                           modifiedDate
                           isActive
                           imageUrl
                        }
                    }";

                var request = new GraphQLRequest { Query = query };
                
                var response = await _graphQLClient.SendQueryAsync<StationListResponse>(request);

                if (response.Errors != null && response.Errors.Any())
                {
                    var errorMessages = string.Join(", ", response.Errors.Select(e => e.Message));
                    throw new Exception($"GraphQL Error: {errorMessages}");
                }

                return response?.Data?.GetStations?.ToList() ?? new List<StationTriNM>();
            }
            catch (Exception ex)
            {
                return new List<StationTriNM>();
            }
        }

        public async Task<StationTriNM?> GetStationByIdAsync(int id)
        {
            try
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                        query ($id: Int!) {
                           getStationById(id: $id) {
                               stationTriNmid
                               stationTriNmcode
                               stationTriNmname
                               address
                               city
                               province
                               latitude
                               longitude
                               capacity
                               currentAvailable
                               owner
                               contactPhone
                               contactEmail
                               description
                               createdDate
                               modifiedDate
                               isActive
                               imageUrl
                            }
                        }",
                    Variables = new { id }
                };

                var response = await _graphQLClient.SendQueryAsync<StationByIdResponse>(request);

                if (response.Errors != null && response.Errors.Any())
                {
                    var errorMessages = string.Join(",", response.Errors.Select(e => e.Message));
                    throw new Exception($"GraphQL Error: {errorMessages}");
                }

                return response?.Data?.GetStationById;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<StationTriNM> CreateStationAsync(StationTriNM stationTriNM)
        {
            try
            {
                var mutation = @"
                    mutation CreateStation($input: CreateStationInput!) {
                        createStation(input: $input) {
                           stationTriNmid
                           stationTriNmcode
                           stationTriNmname
                           address
                           city
                           province
                           latitude
                           longitude
                           capacity
                           currentAvailable
                           owner
                           contactPhone
                           contactEmail
                           description
                           createdDate
                           isActive
                           imageUrl
                        }
                    }";

                var input = new
                {
                    StationCode = stationTriNM.StationTriNMCode,
                    StationName = stationTriNM.StationTriNMName,
                    address = stationTriNM.Address,
                    city = stationTriNM.City,
                    province = stationTriNM.Province,
                    latitude = stationTriNM.Latitude,
                    longitude = stationTriNM.Longitude,
                    capacity = stationTriNM.Capacity,
                    owner = stationTriNM.Owner,
                    contactPhone = stationTriNM.ContactPhone,
                    contactEmail = stationTriNM.ContactEmail,
                    description = stationTriNM.Description,
                    imageURL = stationTriNM.ImageURL
                };

                var request = new GraphQLRequest
                {
                    Query = mutation,
                    Variables = new { input }
                };

                var response = await _graphQLClient.SendMutationAsync<CreateStationResponse>(request);
                if (response.Errors != null && response.Errors.Any())
                {
                    var errorMessages = string.Join("; ", response.Errors.Select(e => e.Message));
                    throw new Exception($"GraphQL Error: {errorMessages}");
                }
                return response?.Data?.CreateStation ?? stationTriNM;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<StationTriNM?> UpdateStationAsync(StationTriNM stationTriNM)
        {
            try
            {
                var mutation = @"
                    mutation UpdateStation($input: UpdateStationInput!) {
                        updateStation(input: $input) {
                           stationTriNmid
                           stationTriNmcode
                           stationTriNmname
                           address
                           city
                           province
                           latitude
                           longitude
                           capacity
                           currentAvailable
                           owner
                           contactPhone
                           contactEmail
                           description
                           modifiedDate
                           isActive
                           imageUrl
                        }
                    }";

                var input = new
                {
                    StationId = stationTriNM.StationTriNMId,
                    StationCode = stationTriNM.StationTriNMCode,
                    StationName = stationTriNM.StationTriNMName,
                    address = stationTriNM.Address,
                    city = stationTriNM.City,
                    province = stationTriNM.Province,
                    latitude = stationTriNM.Latitude,
                    longitude = stationTriNM.Longitude,
                    capacity = stationTriNM.Capacity,
                    owner = stationTriNM.Owner,
                    contactPhone = stationTriNM.ContactPhone,
                    contactEmail = stationTriNM.ContactEmail,
                    description = stationTriNM.Description,
                    imageURL = stationTriNM.ImageURL,
                    isActive = (bool?)stationTriNM.IsActive
                };

                var request = new GraphQLRequest
                {
                    Query = mutation,
                    Variables = new { input }
                };

                var response = await _graphQLClient.SendMutationAsync<UpdateStationResponse>(request);
                
                if (response.Errors != null && response.Errors.Any())
                {
                    var errorMessages = string.Join("; ", response.Errors.Select(e => e.Message));
                    throw new Exception($"GraphQL Error: {errorMessages}");
                }
                
                return response?.Data?.UpdateStation;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteStationAsync(int id)
        {
            try
            {
                var mutation = @"
                    mutation DeleteStation($id: Int!) {
                        deleteStation(id: $id)
                    }";

                var request = new GraphQLRequest
                {
                    Query = mutation,
                    Variables = new { id }
                };

                var response = await _graphQLClient.SendMutationAsync<DeleteStationResponse>(request);
                return response?.Data?.DeleteStation ?? false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<StationTriNM>> SearchStationsAsync(string? name, string? location, bool? isActive)
        {
            try
            {
                var query = @"
                    query SearchStations($name: String, $location: String, $isActive: Boolean) {
                       searchStations(name: $name, location: $location, isActive: $isActive) {
                           stationTriNmid
                           stationTriNmcode
                           stationTriNmname
                           address
                           city
                           province
                           latitude
                           longitude
                           capacity
                           currentAvailable
                           owner
                           contactPhone
                           contactEmail
                           description
                           createdDate
                           modifiedDate
                           isActive
                           imageUrl
                        }
                    }";

                var request = new GraphQLRequest
                {
                    Query = query,
                    Variables = new { name, location, isActive }
                };

                var response = await _graphQLClient.SendQueryAsync<SearchStationsResponse>(request);

                if (response.Errors != null && response.Errors.Any())
                {
                    var errorMessages = string.Join(", ", response.Errors.Select(e => e.Message));
                    throw new Exception($"GraphQL Error: {errorMessages}");
                }

                return response?.Data?.SearchStations?.ToList() ?? new List<StationTriNM>();
            }
            catch (Exception ex)
            {
                return new List<StationTriNM>();
            }
        }

        public async Task<List<ChargerTriNM>> GetChargersAsync()
        {
            try
            {
                var query = @"
                    query {
                       getChargers {
                           chargerTriNmid
                           stationTriNmid
                           chargerTriNmtype
                           isAvailable
                           imageUrl
                           stationTriNm {
                               stationTriNmid
                               stationTriNmname
                               address
                            }
                        }
                    }";

                var response = await _graphQLClient.SendQueryAsync<ChargerListResponse>(
                    new GraphQLRequest { Query = query });

                return response?.Data?.GetChargers?.ToList() ?? new List<ChargerTriNM>();
            }
            catch (Exception ex)
            {
                return new List<ChargerTriNM>();
            }
        }

        public async Task<ChargerTriNM?> GetChargerByIdAsync(int id)
        {
            try
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                        query ($id: Int!) {
                           getChargerById(id: $id) {
                               chargerTriNmid
                               stationTriNmid
                               chargerTriNmtype
                               isAvailable
                               imageUrl
                               stationTriNm {
                                   stationTriNmid
                                   stationTriNmname
                                   address
                                }
                            }
                        }",
                    Variables = new { id }
                };

                var response = await _graphQLClient.SendQueryAsync<ChargerByIdResponse>(request);

                if (response.Errors != null && response.Errors.Any())
                {
                    var errorMessages = string.Join(",", response.Errors.Select(e => e.Message));
                    throw new Exception($"GraphQL Error: {errorMessages}");
                }

                return response?.Data?.GetChargerById;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ChargerTriNM>> GetChargersByStationIdAsync(int stationTriNMId)
        {
            try
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                        query ($stationId: Int!) {
                           getChargersByStationId(stationId: $stationId) {
                               chargerTriNmid
                               stationTriNmid
                               chargerTriNmtype
                               isAvailable
                               imageUrl
                            }
                        }",
                    Variables = new { stationId = stationTriNMId }
                };

                var response = await _graphQLClient.SendQueryAsync<ChargerListResponse>(request);
                return response?.Data?.GetChargers?.ToList() ?? new List<ChargerTriNM>();
            }
            catch (Exception ex)
            {
                return new List<ChargerTriNM>();
            }
        }

        // ==== Login ====
        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            try
            {
                var mutation = @"
                    mutation Login($input: LoginInput!) {
                        login(input: $input) {
                            token
                            error
                        }
                    }";

                var request = new GraphQLRequest
                {
                    Query = mutation,
                    Variables = new { input = new { email, password } }
                };

                var response = await _graphQLClient.SendMutationAsync<LoginResponse>(request);

                if (response.Errors != null && response.Errors.Any())
                {
                    var errorMessages = string.Join(", ", response.Errors.Select(e => e.Message));
                    return new LoginResult { Success = false, Error = errorMessages };
                }

                var loginPayload = response?.Data?.Login;
                if (loginPayload == null)
                {
                    return new LoginResult { Success = false, Error = "Invalid response from server" };
                }

                if (!string.IsNullOrEmpty(loginPayload.error))
                {
                    return new LoginResult { Success = false, Error = loginPayload.error };
                }

                return new LoginResult { Success = true, Token = loginPayload.token };
            }
            catch (Exception ex)
            {
                return new LoginResult { Success = false, Error = "Connection error. Please try again." };
            }
        }

        // ==== Google Login ====
        public async Task<LoginResult> GoogleLoginAsync(string credential)
        {
            try
            {
                var mutation = @"
                    mutation GoogleLogin($credential: String!) {
                        googleLogin(credential: $credential) {
                            token
                            error
                        }
                    }";

                var request = new GraphQLRequest
                {
                    Query = mutation,
                    Variables = new { credential }
                };

                var response = await _graphQLClient.SendMutationAsync<GoogleLoginResponse>(request);

                if (response.Errors != null && response.Errors.Any())
                {
                    var errorMessages = string.Join(", ", response.Errors.Select(e => e.Message));
                    return new LoginResult { Success = false, Error = errorMessages };
                }

                var loginPayload = response?.Data?.GoogleLogin;
                if (loginPayload == null)
                {
                    return new LoginResult { Success = false, Error = "Invalid response from server" };
                }

                if (!string.IsNullOrEmpty(loginPayload.error))
                {
                    return new LoginResult { Success = false, Error = loginPayload.error };
                }

                return new LoginResult { Success = true, Token = loginPayload.token };
            }
            catch (Exception ex)
            {
                return new LoginResult { Success = false, Error = "Google login error: " + ex.Message };
            }
        }

        public class StationListResponse
        {
            [Newtonsoft.Json.JsonProperty("getStations")]
            public List<StationTriNM> GetStations { get; set; } = new();
        }

        public class SearchStationsResponse
        {
            [Newtonsoft.Json.JsonProperty("searchStations")]
            public List<StationTriNM> SearchStations { get; set; } = new();
        }

        public class StationByIdResponse
        {
            [Newtonsoft.Json.JsonProperty("getStationById")]
            public StationTriNM? GetStationById { get; set; }
        }

        public class CreateStationResponse
        {
            [Newtonsoft.Json.JsonProperty("createStation")]
            public StationTriNM CreateStation { get; set; }
        }

        public class UpdateStationResponse
        {
            [Newtonsoft.Json.JsonProperty("updateStation")]
            public StationTriNM? UpdateStation { get; set; }
        }

        public class DeleteStationResponse
        {
            [Newtonsoft.Json.JsonProperty("deleteStation")]
            public bool DeleteStation { get; set; }
        }

        public class ChargerListResponse
        {
            [Newtonsoft.Json.JsonProperty("getChargers")]
            public List<ChargerTriNM> GetChargers { get; set; } = new();
        }

        public class ChargerByIdResponse
        {
            [Newtonsoft.Json.JsonProperty("getChargerById")]
            public ChargerTriNM? GetChargerById { get; set; }
        }

        public class LoginResponse
        {
            [Newtonsoft.Json.JsonProperty("login")]
            public LoginPayload Login { get; set; }
        }

        public class GoogleLoginResponse
        {
            [Newtonsoft.Json.JsonProperty("googleLogin")]
            public LoginPayload GoogleLogin { get; set; }
        }

        public class LoginPayload
        {
            public string? token { get; set; }
            public string? error { get; set; }
        }

        public class LoginResult
        {
            public bool Success { get; set; }
            public string? Token { get; set; }
            public string? Error { get; set; }
        }
    }
}
