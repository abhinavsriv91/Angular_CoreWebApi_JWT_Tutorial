using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tutorial.Business.Models
{
    /// <summary>
    /// User Model
    /// </summary>
    public class User
    {
        /// <summary>
        /// User Name
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Password
        /// </summary>

        public string EmailAddress { get; set; }

        /// <summary>
        /// Password
        /// </summary>

        public List<string> Roles { get; set; }
    }
}
