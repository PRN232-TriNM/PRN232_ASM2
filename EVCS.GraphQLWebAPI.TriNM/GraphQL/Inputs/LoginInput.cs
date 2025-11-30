namespace EVCS.GraphQLWebAPI.TriNM.GraphQL.Inputs
{
    public class LoginInput
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginPayload
    {
        public string? Token { get; set; }
        public string? Error { get; set; }
    }
}

