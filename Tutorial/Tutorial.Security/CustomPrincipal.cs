using Tutorial.Global;
using Tutorial.Global.DTO;
using System;
using System.Security;
using System.Security.Principal;
using System.Threading;
using Tutorial.Logger;

namespace Tutorial.Security
{
    /// <summary>
    /// Represents the identity and security information of an application user.
    /// </summary>
    public class CustomPrincipal : GenericPrincipal
    {
        public string UserName { get; }

        public string FullName { get; }

        public string EmailAddress { get; }

        /// <summary>
        /// Initialize user name and domain
        /// </summary>
        /// <param name="user"></param>
        public CustomPrincipal(AuthenticatedUserDTO user) : base(new CustomIdentity(user.UserName, user.FullName, user.EmailAddress), new string[] { })
        {
            UserName = user.UserName;
            FullName = user.FullName;
            EmailAddress = user.EmailAddress;
        }

        /// <summary>
        /// Get the security principal associated with the current thread
        /// </summary>
        /// <returns>the security principal</returns>
        public static CustomPrincipal GetCurrentPrincipal()
        {
            // Try to get a CustomPrincipal from the current thread
            try
            {
                return (CustomPrincipal)Thread.CurrentPrincipal;
            }
            catch (InvalidCastException castException)
            {
                // The current identity is not a EburoFactory CustomIdentity
                SecurityException securityException = new SecurityException(GlobalConstants.MessageNoPrincipal, castException);
                TutorialLogger.LogError(GlobalConstants.MessageNoPrincipal);
                throw securityException;
            }
        }

        /// <summary>
        /// Returns the userid of the current thread's identity
        /// </summary>
        /// <returns>Userid of the current user</returns>
        public static string GetCurrentUserName()
        {
            // Return the userid
            return GetCurrentPrincipal()?.CustomIdentity?.UserName;
        }

        /// <summary>
        /// Returns the FullName of the current thread's identity
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentUserFullName()
        {
            return GetCurrentPrincipal()?.CustomIdentity?.FullName;
        }

        /// <summary>
        /// Returns the Email Address of the current thread's identity
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentUserEmailAddress()
        {
            return GetCurrentPrincipal()?.CustomIdentity?.EmailAddress;
        }

        /// <summary>
        /// Indicates whether the current user is authenticated
        /// </summary>
        /// <returns>True if the current thread has a CustomPrincipal object assigned to it, false otherwise</returns>
        public static bool IsAuthenticated()
        {
            return Thread.CurrentPrincipal is CustomPrincipal;
        }

        /// <summary>
        /// Gets the identity of this principal typed as a CustomIdentity
        /// </summary>
        public CustomIdentity CustomIdentity
        {
            get { return (CustomIdentity)base.Identity; }
        }
    }
}
