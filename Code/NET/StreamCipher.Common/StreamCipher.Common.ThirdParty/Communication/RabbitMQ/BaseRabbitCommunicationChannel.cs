using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using StreamCipher.Common.Components.Communication;
using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.Interfaces.Communication;
using RabbitMQ.Client;
using StreamCipher.Common.Utilities.Concurrency;
using StreamCipher.Common.Utilities.ObjectType;

namespace StreamCipher.Common.ThirdParty.Communication.RabbitMQ
{
    public class BaseRabbitCommunicationChannel:AbstractCommunicationChannel
    {
        #region Member Variables

        protected const String EXCHANGE_NAME = "StreamCipher";
        protected IConnection _connection;
        
        //TODO: Ensure threadsafe access to messaging session.
        protected ThreadSafe<IModel> _session;
        private MemberInfo[] memberInfos = typeof (ConnectionFactory).GetMembers(BindingFlags.Instance|BindingFlags.Public);

        #endregion
        
        #region Init

        protected BaseRabbitCommunicationChannel(IDataInterchangeFormatter formatter,
            ICommunicationServiceConfig config,
            String connectionIdSuffix,
            Action<Exception> defaultExceptionHandler) :
            base(ServiceBusType.RABBITMQ, formatter, config, connectionIdSuffix, defaultExceptionHandler)
        {
        }

        #endregion
        
        #region AbstractCommunicationChannel
        
        protected override void ConnectCore()
        {
            var connFactory = new ConnectionFactory();
            connFactory.HostName = _config.ServiceBusAddress;
            
            //Loop through all the custom fields and set them dynamically
            foreach (var memberInfo in memberInfos)
            {
                if (_config.CustomProps.ContainsKey(memberInfo.Name))
                {
                    object val = _config.CustomProps[memberInfo.Name];
                    try
                    {
                        if (memberInfo.MemberType == MemberTypes.Field)
                        {
                            var f = (FieldInfo) memberInfo;
                            var settingValue = Convert.ChangeType(val, f.FieldType);
                            f.SetValue(connFactory, settingValue);    
                        }
                        else if (memberInfo.MemberType == MemberTypes.Property)
                        {
                            var p = (PropertyInfo)memberInfo;
                            var settingValue = Convert.ChangeType(val, p.PropertyType);
                            p.SetValue(connFactory, settingValue, null);
                        }
                        
                    }
                    catch (Exception e)
                    {
                        //TODO: Do something with the exception...
                    }
                }
            }

            _connection = connFactory.CreateConnection();
            
            //You can create multiple channels or sessions on the same connection... although not sure what that means.
            //Using one session per connection here.
            _session = new ThreadSafe<IModel>(_connection.CreateModel());
            _session.Use.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic, true);
        }

        protected override void DisconnectCore()
        {
            _session.Use.Close();
            _connection.Close();
        }

        #endregion
    }
}
