using System;

namespace StreamCipher.Common.Communication
{
    public interface IMessageWrapper
    {
        /// <summary>
        /// Correlation id that links all dispatches for a single, logical message.
        /// </summary>
        String CorrelationId { get; }

        /// <summary>
        /// Sequence of dispatches that constitute an individual message.
        /// </summary>
        Object Content { get; }
    }
}
