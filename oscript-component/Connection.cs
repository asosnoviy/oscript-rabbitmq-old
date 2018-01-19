using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using RabbitMQ.Client;

namespace oscriptcomponent
{
	/// <summary>
	/// Класс СоединениеRMQ. Служит для подключения к серверу RabbitMQ и получения клиента управления.
	/// </summary>
	[ContextClass("СоединениеRMQ", "ConnectionRMQ")]
	public class Connection : AutoContext<Connection>
	{
	
        public Connection()
		{
		}

		private IConnection RmqConnection;

		/// <summary>
		/// Пользователь.
		/// </summary>
		[ContextProperty("Пользователь")]
		public string User { get; set; }

		
		/// <summary>
		/// Пароль пользователя.
		/// </summary>
		[ContextProperty("Пароль")]
		public string Pass { get; set; }

		/// <summary>
		/// Виртуальный хост (vhost).
		/// </summary>
		[ContextProperty("ВиртуальныйХост")]
		public string Vhost { get; set; }
	
		
		/// <summary>
		/// Адрес сервера - hostname или ip-адрес.
		/// </summary>
		[ContextProperty("Сервер")]
		public string Host { get; set; }
		
		/// <summary>
		/// Порт.
		/// </summary>
		[ContextProperty("Порт")]
		public int Port { get; set; }
		
		/// <summary>
		/// Установить соединение с сервером RabbitMQ.
		/// Возвращает клиент управления.
		/// </summary>
		/// <returns>КлиентRMQ</returns>
		[ContextMethod("Установить")]
		public AmqpLib Create()
		{
			var factory = new ConnectionFactory();

			if (!string.IsNullOrEmpty(User)) factory.UserName = User;
			if (!string.IsNullOrEmpty(Pass)) factory.Password = Pass;
			if (!string.IsNullOrEmpty(Vhost)) factory.VirtualHost = Vhost;
			if (!string.IsNullOrEmpty(Host)) factory.HostName = Host;
			if (Port != 0) factory.Port = Port;
			
			RmqConnection = factory.CreateConnection();
			var rmqModel =  RmqConnection.CreateModel();
			
			return new AmqpLib(rmqModel);

		}
		
		/// <summary>
		/// Закрыть соединение.
		/// </summary>
		[ContextMethod("Закрыть")]
		public void Close()
		{
			RmqConnection.Close();
		}
		
		/// <summary>
		/// По умолчанию
		/// </summary>
		/// <returns>СоединениеRMQ</returns>
		[ScriptConstructor]
		public static Connection Constructor()
		{
			return new Connection();
		}
	}
}

