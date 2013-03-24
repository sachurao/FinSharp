using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace StreamCipher.Common.Communication.ThirdParty.RabbitMQ
{
    class RabbitMessageConsumer:DefaultBasicConsumer
    {
        private Action<BasicDeliverEventArgs> _onReceivingDispatch;
        public RabbitMessageConsumer(Action<BasicDeliverEventArgs> processReceivedMessage)
        {
            _onReceivingDispatch = processReceivedMessage;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            var bdea = new BasicDeliverEventArgs()
            {
                ConsumerTag = consumerTag,
                DeliveryTag = deliveryTag,
                Redelivered = redelivered,
                Exchange = exchange,
                RoutingKey = routingKey,
                BasicProperties = properties,
                Body = body
            };
            _onReceivingDispatch(bdea);
        }
    }
}
