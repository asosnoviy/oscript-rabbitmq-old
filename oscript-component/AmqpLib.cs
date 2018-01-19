using RabbitMQ.Client;
using ScriptEngine.Machine.Contexts;

namespace oscriptcomponent
{
    /// <summary>
    /// Клиент управления RabbitMQ
    /// </summary>
    [ContextClass("КлиентRMQ")]
    public class AmqpLib : AutoContext<AmqpLib>
    {
        private readonly IModel _rmqModel;

        public AmqpLib(IModel rmqModel)
        {
            _rmqModel = rmqModel;
        }

        /// <summary>
        /// Отправить сообщение в виде строки в указанную точку обмена. 
        /// </summary>
        /// <param name="messageText">Текст сообщения</param>
        /// <param name="exchangeName">Имя точки обмена</param>
        /// <param name="routingKey">Ключ маршрутизации</param>
        [ContextMethod("ОтправитьСтроку")]
        public void PublishString(string messageText, string exchangeName, string routingKey = "")
        {
            if (routingKey == null)
            {
                routingKey = "";
            }

            // todo
            const byte deliveryMode = 2;

            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(messageText);

            var rmqMessageProperties = _rmqModel.CreateBasicProperties();
            rmqMessageProperties.ContentType = "text/plain";
            rmqMessageProperties.ContentEncoding = "string";
            rmqMessageProperties.DeliveryMode = deliveryMode;

            _rmqModel.BasicPublish(exchangeName, routingKey, rmqMessageProperties, messageBodyBytes);
        }

        /// <summary>
        /// Получить текстовое сообщение из очереди.
        /// </summary>
        /// <param name="queueName">Имя очереди</param>
        /// <returns></returns>
        [ContextMethod("ПолучитьСтроку")]
        public string GetString(string queueName)
        {
            BasicGetResult result = _rmqModel.BasicGet(queueName, true);

            string message = "";

            if (result != null)
            {
                byte[] body = result.Body;
                message = System.Text.Encoding.UTF8.GetString(body);
            }

            return message;
        }
    }
}