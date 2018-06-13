using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSite.RabbitMQProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            //部署在我的服务器 http://120.78.212.68:15672/#/  帐号密码不公开
            var factory = new ConnectionFactory
            {
                HostName = "localhost",//RabbitMQ服务在本地运行
               // Port = 5672,
                UserName = "****",
                Password = "****"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    //channel 通信
                    //第二个参数为是否持久化
                    channel.QueueDeclare("Test", true, false, false, null);//创建一个名称为hello的消息队列
                    string message = "vidic.huang"; //传递的消息内容
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("", "Test", null, body); //开始传递
                    Console.WriteLine("已发送： {0}", message);
                    Console.ReadLine();
                }
            }

            Console.ReadKey();
        }
    
    }
}
