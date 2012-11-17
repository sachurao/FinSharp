using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.Common.Interfaces.Communication
{
    /// <summary>
    /// Represents a simple wrapper around the contents of each message packet sent across the wire.
    /// A dispatch represents a logical packet of communication.
    /// </summary>
    public interface IDispatchWrapper
    {
        /// <summary>
        /// Cargo of each dispatch.
        /// </summary>
        Object Payload { get; }

        /// <summary>
        /// Total dispatches in the message.
        /// </summary>
        int DispatchCardinality { get; set; }
        
        /// <summary>
        /// Position of current dispatch in the message
        /// </summary>
        int DispatchIndex { get; set; }
    }
}
