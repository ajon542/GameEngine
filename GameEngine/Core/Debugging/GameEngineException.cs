using System;
using System.Runtime.Serialization;

namespace GameEngine.Core.Debugging
{
    /// <summary>
    /// Exception helper class to provide functionality for exceptions with
    /// variable length paramemters.
    /// </summary>
    [Serializable]
    public class GameEngineException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameEngineException"/> class.
        /// </summary>
        /// <remarks>Throw exception without message.</remarks>
        public GameEngineException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameEngineException"/> class.
        /// </summary>
        /// <remarks>Throw exception with message.</remarks>
        /// <param name="message">Exception information.</param>
        public GameEngineException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameEngineException"/> class.
        /// </summary>
        /// <remarks>Throw exception with message format and parameters.</remarks>
        /// <param name="format">Message format.</param>
        /// <param name="args">Message arguments.</param>
        public GameEngineException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameEngineException"/> class.
        /// </summary>
        /// <remarks>Throw exception with message and inner exception.</remarks>
        /// <param name="message">Exception information.</param>
        /// <param name="innerException">Inner exception.</param>
        public GameEngineException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameEngineException"/> class.
        /// </summary>
        /// <remarks>Throw exception with message format and inner exception.</remarks>
        /// <param name="format">Message format.</param>
        /// <param name="innerException">Inner exception.</param>
        /// <param name="args">Message arguments.</param>
        public GameEngineException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameEngineException"/> class.
        /// </summary>
        /// <remarks>Standard serialization constructor.</remarks>
        /// <param name="info">Data needed to serialize/deserialize the object.</param>
        /// <param name="context">Describes the source and destination of the given serialized stream.</param>
        protected GameEngineException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
