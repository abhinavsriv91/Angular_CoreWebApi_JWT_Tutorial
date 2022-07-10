using System;
using System.Runtime.Serialization;

namespace Tutorial.Global.Exceptions
{
    /// <summary>
    /// Ldap Authentication Exception
    /// </summary>
    public class LdapAuthenticationException: TutorialApplicationException
    {
        #region Constructors and Finalizers

        /// <summary>
        /// This zero-argument constructor must never be used.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Always thrown.</exception>
        public LdapAuthenticationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Constructor for the InvalidInputException taking a message
        /// </summary>
        /// <param name="message">A message stating what exception occurred</param>
        /// <exception cref="System.ArgumentNullException">Thrown when message is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when message is the empty string.</exception>
        public LdapAuthenticationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor for the InvalidInputException taking a message and a nested exception
        /// </summary>
        /// <param name="message">A message stating what exception occurred</param>
        /// <param name="inner">A nested inner exception</param>
        /// <exception cref="System.ArgumentNullException">Thrown when message is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when message is the empty string.</exception>
        public LdapAuthenticationException(string message, int statusCode, Exception inner) : base(message, statusCode, inner)
        {
        }

        #endregion Constructors and Finalizers
    }
}
