using System;
using System.Runtime.Serialization;
using Tutorial.Logger;

namespace Tutorial.Global.Exceptions
{
    /// <summary>
    /// This exception is the base of all application exceptions in EburoFactory.
    /// Since exceptions without a message is of no value, any attempt to 
    /// create an exception wihtout a message will result in an exception
    /// being thrown.
    /// </summary>
    public class TutorialApplicationException : ApplicationException
    {
        public int StatusCode { get; set; }

        #region Constructors and Finalizers

        /// <summary>
        /// This zero-argument constructor must never be used.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Always thrown.</exception>
        public TutorialApplicationException()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Constructor for the EburoFactoryApplication taking a message
        /// </summary>
        /// <param name="message">A message stating what exception occurred</param>
        /// <exception cref="System.ArgumentNullException">Thrown when message is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when message is the empty string.</exception>
        public TutorialApplicationException(string message): base(message)
        {
            CheckMessage(message);
        }

        /// <summary>
        /// Constructor for the EburoFactoryApplication taking a message and a nested exception
        /// </summary>
        /// <param name="message">A message stating what exception occurred</param>
        /// <param name="inner">A nested inner exception</param>
        /// <exception cref="System.ArgumentNullException">Thrown when message is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when message is the empty string.</exception>
        public TutorialApplicationException(string message, int statusCode, Exception inner): base(message, inner)
        {
            StatusCode = statusCode;
            CheckMessage(message);
        }

        /// <summary>
        /// Construct an instance of this exception with serialized data
        /// </summary>
        public TutorialApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion Constructors and Finalizers

        #region Private and Protected Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void CheckMessage(string message)
        {
            if (message == null)
            {
                string ErrMessage = "The message parameter cannot be Null.";
                TutorialLogger.LogError(ErrMessage);
                throw new ArgumentNullException(ErrMessage);
            }

            if (string.Empty.Equals(message))
            {
                string ErrMessage1 = "The message parameter cannot be the empty string.";
                TutorialLogger.LogError(ErrMessage1);

                throw new ArgumentOutOfRangeException("message", string.Empty, ErrMessage1);
            }
        }

        #endregion Private and Protected Methods
    }
}
