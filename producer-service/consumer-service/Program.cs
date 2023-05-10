using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using commons.DTO;
using Newtonsoft.Json;


class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var queueName = "product.log";

           
            channel.QueueDeclare(queueName, false, false, false, null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);


                var product = JsonConvert.DeserializeObject<ProductDTO>(message);
                Console.WriteLine("Produto recebido:  ID - {0} DESC - {1} PREÇO {2}", product.id, product.name, product.price);
            };

            channel.BasicConsume(queueName, true, consumer);

            Console.WriteLine("Pressione qualquer tecla para sair");
            Console.ReadKey();
        }
    }
}
