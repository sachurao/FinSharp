using System;

namespace StreamCipher.Common.Communication
{
    public interface IProcessResponses:IProcessAnyMessage
    {
        void AddSpecificResponseHandler(String correlationId, Action<IIncomingMessage> handler);
    }
}
