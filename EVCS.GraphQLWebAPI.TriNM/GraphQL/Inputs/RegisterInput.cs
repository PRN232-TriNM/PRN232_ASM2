namespace EVCS.GraphQLWebAPI.TriNM.GraphQL.Inputs
{
    public class RegisterInput
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? EmployeeCode { get; set; }
        public int RoleId { get; set; } = 2;
    }
}

