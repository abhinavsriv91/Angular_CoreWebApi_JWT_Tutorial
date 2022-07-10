using System.Collections.Generic;

namespace Tutorial.Global.DTO
{
    /// <summary>
    /// Authenticated User
    /// </summary>
    public class AuthenticatedUserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public List<string> Roles { get; set; }
    }
}
