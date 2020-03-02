using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using CargaMoedasRabbitMQ.Models;

namespace CargaMoedasRabbitMQ
{
    class Program
    {
        private const string QUEUE = "queue-cotacoes";
        private static string _BROKER;

        public static void EnviarMensagem(Cotacao cotacao)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_BROKER)
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: QUEUE,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = JsonSerializer.Serialize(cotacao);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: QUEUE,
                                     basicProperties: null,
                                     body: body);
                
                Console.WriteLine($"Cadastrado: {message}");
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }

        static void Main(string[] args)
        {
            if (!String.IsNullOrWhiteSpace(args[0]))
            {
                _BROKER = args[0];

                EnviarMensagem(new Cotacao { Sigla = "USD", Valor = 4.479 });               
                EnviarMensagem(new Cotacao { Sigla = "EUR", Valor = 4.939 });               
                EnviarMensagem(new Cotacao { Sigla = "LIB", Valor = 4.746 });               
                EnviarMensagem(new Cotacao { Sigla = "USD", Valor = 4.489 });               

                Console.WriteLine("Carga concluida!");
            }
            else
                Console.WriteLine("Informe como parametro uma string de conexao ao RabbitMQ!");
        }
    }
}