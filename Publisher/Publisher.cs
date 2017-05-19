﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitPublisher
{
    public class Publisher : IDisposable
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string ExchangeName = "Tom.Exchange";
        private const bool IsDurable = true;
        private const string VirtualHost = "";
        private int Port = 0;

        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;
        public Publisher()
        {
            SetupRabbitMq();
        }

        private void SetupRabbitMq()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };

            if (string.IsNullOrEmpty(VirtualHost) == false)
                _connectionFactory.VirtualHost = VirtualHost;
            if (Port > 0)
                _connectionFactory.Port = Port;

            _connection = _connectionFactory.CreateConnection();
            _model = _connection.CreateModel();
        }

        public string Send(string message)
        {
            var properties = _model.CreateBasicProperties();
            properties.Persistent = true;
            byte[] messageBuffer = Encoding.Default.GetBytes(message);
            var routingKey = "Tom.Data.CardTransaction";
            _model.BasicPublish(ExchangeName, routingKey, properties, messageBuffer);
            return routingKey;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (_connection != null)
                _connection.Close();

            if (_model != null && _model.IsOpen)
                _model.Abort();

            _connectionFactory = null;

            GC.SuppressFinalize(this);
        }
    }

    public enum DataType
    {
        CardTransaction = 0,
        CardTransactionDetails = 1
    }

    public class CardTransaction
    {
        public int CardTransactionId { get; set; }
    }
    public class CardTransactionDetails
    {
        public int CardTransactionDetailsId { get; set; }
    }
}
