using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.Common.Interfaces.Communication
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

        //TODO:  Refactor this to support multi-dispatch messages.
    }
}
