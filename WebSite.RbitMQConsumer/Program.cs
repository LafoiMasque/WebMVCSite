using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSiter.RbitMQConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            //部署在我的服务器 http://120.78.212.68:15672/#/  帐号密码不公开
            var factory = new ConnectionFactory
            {
                HostName = "localhost",//RabbitMQ服务在本地运行
                //Port = 5672,
                UserName = "****",
                Password = "****"
            };


            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //是否持久化   生产-消费者一致
                    bool durable = true;
                    channel.QueueDeclare("Test", durable, false, false, null);
                    channel.BasicQos(0, 1, false);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("Test", false, consumer);

                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        int dots = message.Split('.').Length - 1;
                        Thread.Sleep(dots * 1000);

                        Console.WriteLine("Received {0}", message);
                        Console.WriteLine("Done");

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
            }
        }
    }
}
