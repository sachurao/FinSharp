using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.Shared.Components.Communication;
using Irrefutable.Shared.Interfaces.DataInterchange;
using Irrefutable.Shared.Interfaces.Communication;
using RabbitMQ.Client;

namespace Irrefutable.Shared.ThirdParty.Communication.RabbitMQ
{
    public class BaseRabbitCommunicationChannel:AbstractCommunicationChannel
    {
        #region Member Variables
        
        protected const String EXCHANGE_NAME = "Irrefutable";
        protected IConnection _connection;
        protected IModel _session;

        #endregion
        
        #region Init

        protected BaseRabbitCommunicationChannel(DataInterchangeFormat format,
            ICommunicationServiceConfig config, String connectionIdSuffix,
            Action<Exception> defaultExceptionHandler) :
            base(ServiceBusType.RABBITMQ, format, config, connectionIdSuffix, defaultExceptionHandler)
        {
        }

        #endregion
        
        #region AbstractCommunicationChannel
        
        protected override void ConnectCore()
        {
            var connFactory = new ConnectionFactory();
            connFactory.HostName = _config.CustomProps["HostName"];
            connFactory.VirtualHost = _config.CustomProps["VirtualHost"];
            connFactory.UserName = "sachin";
            connFactory.Password = "msgadmin!";
            
            _connection = connFactory.CreateConnection();
            _session = _connection.CreateModel();
            _session.ExchangeDeclare(EXCHANGE_NAME, "fanout", true);
        }

        protected override void DisconnectCore()
        {
            _session.Close();
            _connection.Close();
        }

        #endregion
    }
}
