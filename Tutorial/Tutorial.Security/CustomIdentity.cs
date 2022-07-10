using System;
using System.Security.Principal;

namespace Tutorial.Security
{
    /// <summary>
    /// Represents the identity of a user of the application
    /// </summary>
    [Serializable()]
    public class CustomIdentity : GenericIdentity
    {
        private string _userName;
        private string _fullName;
        private string _emailAddress;

        /// <summary>
        /// Initializes a new instance of the CustomIdentity class representing the user with the specified User Name.
        /// </summary>
        /// <param name="userName">The UserName of the user on whose behalf the code is running.</param>
        /// <param name="fullName"></param>
        /// <param name="emailAddress"></param>
        public CustomIdentity(string userName, string fullName, string emailAddress) : base(userName)
        {
            _userName = userName;
            _fullName = fullName;
            _emailAddress = emailAddress;
        }

        /// <summary>
        /// The User Name of the user on whose behalf the code is running.
        /// </summary>
        public string UserName
        {
            get { return _userName; }
        }

        /// <summary>
        /// The Full Name of the user on whose behalf the code is running.
        /// </summary>
        public string FullName
        {
            get { return _fullName; }
        }

        /// <summary>
        /// The Email Address of the user on whose behalf the code is running.
        /// </summary>
        public string EmailAddress
        {
            get { return _emailAddress; }
        }
    }
}
