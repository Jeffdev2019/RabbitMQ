using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using commons.DTO;
using Newtonsoft.Json;

namespace producer_service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProducerController : Controller
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly RabbitMQSettings _rabbitMQSettings;



        public ProducerController(IOptions<RabbitMQSettings> rabbitMQSettings)
        {
            _rabbitMQSettings = rabbitMQSettings.Value;

            _connectionFactory = new ConnectionFactory()
            {
                HostName = _rabbitMQSettings.HostName,
                UserName = _rabbitMQSettings.UserName,
                Password = _rabbitMQSettings.Password
                
            };
        }

        [HttpPost]
        [Route("/PublicarMensagem")]
        public IActionResult PublicarMensagem([FromBody] ProductDTO product)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: _rabbitMQSettings.ExchangeName, type: ExchangeType.Direct);
                channel.QueueDeclare(queue: _rabbitMQSettings.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: _rabbitMQSettings.QueueName, exchange: _rabbitMQSettings.ExchangeName, routingKey: _rabbitMQSettings.RountingKey);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(product));
                channel.BasicPublish(exchange: _rabbitMQSettings.ExchangeName, routingKey: _rabbitMQSettings.RountingKey, basicProperties: null, body: body);

                return Ok("Send Message");
            }
        }
    }
}
