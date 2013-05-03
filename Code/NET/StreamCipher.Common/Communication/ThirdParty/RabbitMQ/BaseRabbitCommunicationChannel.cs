using System;
using System.Reflection;
using RabbitMQ.Client;
using StreamCipher.Common.Communication.Impl;
using StreamCipher.Common.Logging;
using StreamCipher.Common.Utilities.Concurrency;

namespace StreamCipher.Common.Communication.ThirdParty.RabbitMQ
{
    public class BaseRabbitCommunicationChannel:AbstractCommunicationChannel
    {
        #region Member Variables

        protected const String EXCHANGE_NAME = "StreamCipher";
        protected IConnection _connection;
        protected ExclusiveAccess<IModel> _session;
        private MemberInfo[] memberInfos = typeof (ConnectionFactory).GetMembers(BindingFlags.Instance|BindingFlags.Public);

        #endregion
        
        #region Init

        protected BaseRabbitCommunicationChannel(ICommunicationServiceConfig config, 
            String connectionIdSuffix) : base(config, connectionIdSuffix)
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
            _session = new ExclusiveAccess<IModel>(_connection.CreateModel());
            _session.Use.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic, true);
        }

        protected override void DisconnectCore()
        {
            Logger.Debug(this, "Disconnecting connection...");
            _session.Use.Close();
            _connection.Close();
            Logger.Debug(this, "Disconnected connection.");
        }

        protected override bool IsConnectedCore()
        {
            return _session.Use.IsOpen;
        }

        #endregion
    }
}
