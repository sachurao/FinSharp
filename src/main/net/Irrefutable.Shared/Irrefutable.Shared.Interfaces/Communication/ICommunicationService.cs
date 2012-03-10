﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.Shared.Interfaces.ActivityControl;

namespace Irrefutable.Shared.Interfaces.Communication
{
    public interface ICommunicationService: IDisposable
    {
        void AddSenderChannel(CommunicationMode communicationMode, IMessageSenderChannel senderChannel);
        void AddReceiverChannel(CommunicationMode communicationMode, IMessageReceiverChannel receiverChannel);


        void Send(CommunicationMode communicationMode, IMessageDestination destination, IMessageWrapper message);
        IMessageWrapper SendRpc(CommunicationMode communicationMode, IMessageDestination destination, IMessageWrapper message);

        void Subscribe(CommunicationMode communicationMode, IMessageDestination topic, IMessageHandler messageHandler);
        void Unsubscribe(CommunicationMode communicationMode, IMessageDestination topic);        
    }
}