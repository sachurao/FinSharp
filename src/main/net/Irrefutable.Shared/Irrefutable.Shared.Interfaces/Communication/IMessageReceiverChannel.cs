﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irrefutable.Shared.Interfaces.Communication
{
    public interface IMessageReceiverChannel:ICommunicationChannel
    {
        void Subscribe(IMessageDestination topicOrQueue, IMessageHandler messageHandler);
        void Unsubscribe(IMessageDestination topicOrQueue);
        void UnsubscribeAll();
        IEnumerable<IMessageDestination> Subscriptions { get; }
    }
}