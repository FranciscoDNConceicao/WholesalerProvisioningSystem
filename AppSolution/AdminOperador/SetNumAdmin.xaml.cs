using Grpc.Net.Client;
using System;
using System.Text;
using System.Windows;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;

namespace AdminOperador
{
    public partial class SetNumAdmin : Window 
    {
        private string EstadoChanges = string.Empty;
        private GrpcChannel operatorChannel;
        private InstalListaWindow window;
        public SetNumAdmin(string estadoChanges, GrpcChannel channel, InstalListaWindow windowList)
        {
            InitializeComponent();
            EstadoChanges = estadoChanges;
            operatorChannel = channel;
            window = windowList;
        }
       
        private void BttnSetNumAdminClick(object sender, RoutedEventArgs e)
        {
            // Criar um objeto de número administrativo e definir o número com base no valor inserido no campo NomeOperadorInput
            NumeroAdministrador numero = new NumeroAdministrador();
            numero.NumAdmin = int.Parse(NomeOperadorInput.Text);

            // Criar um objeto de IsPossible
            IsPossible possible = new IsPossible();

            // Criar um cliente gRPC para interagir com o servidor
            var operatorClient = new Operator.OperatorClient(operatorChannel);

            // Verificar o valor de EstadoChanges

            switch (EstadoChanges)
            {
                case "ATIVACAO":
                    // Chamar o método do servidor para verificar a possibilidade de ativação

                    possible = operatorClient.AtivacaoPossible(numero);
                    if (possible.IsPossible_)
                    {
                        MessageBox.Show("O numero de administrador foi registado com sucesso!\nA iniciar a subscrição no Tópico EVENTS...\n Tempo estimado : 5 sec", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);

                        Thread.Sleep(5);

                        /*string exchangeName = "EVENTS";
                        string queueName = "Subscriber";
                        string routingKey = "Ativacao";
                        

                        var factory = new ConnectionFactory() { HostName = "localhost" };
                        using (var connection = factory.CreateConnection())
                        using (var channel = connection.CreateModel())
                        {
                            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);

                            channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);

                            channel.QueueBind(queueName, exchangeName, routingKey);

                            var consumer = new EventingBasicConsumer(channel);

                            consumer.Received += (model, ea) =>
                            {
                                var body = ea.Body.ToArray();
                                var message = Encoding.UTF8.GetString(body);
                                
                                MessageBox.Show("Subscrição feita!\n Mensagem: " + message, "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
                            };

                            channel.BasicConsume(queueName, true, consumer);

                        }*/
                        MessageBox.Show("Subscrição feita!\n Mensagem: ", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                        window.PopulateDataAsync(operatorChannel);
                    }
                    else
                    {
                        MessageBox.Show("Ocorreu um erro! Por favor veja se o numero de administrador está correto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                case "DESATIVACAO":
                    possible = operatorClient.DesativacaoPossible(numero);
                    if (possible.IsPossible_)
                    {
                        MessageBox.Show("O numero de administrador foi registado com sucesso!\nA iniciar a subscrição no Tópico EVENTS...\n Tempo estimado : 5 sec", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);

                        Thread.Sleep(5);

                        /*string exchangeName = "EVENTS";
                        string queueName = "Subscriber";
                        string routingKey = "Desativacao";
                        

                        var factory = new ConnectionFactory() { HostName = "localhost" };
                        using (var connection = factory.CreateConnection())
                        using (var channel = connection.CreateModel())
                        {
                            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);

                            channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);

                            channel.QueueBind(queueName, exchangeName, routingKey);

                            var consumer = new EventingBasicConsumer(channel);

                            consumer.Received += (model, ea) =>
                            {
                                var body = ea.Body.ToArray();
                                var message = Encoding.UTF8.GetString(body);
                                
                                MessageBox.Show("Subscrição feita!\n Mensagem: " + message, "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
                            };

                            channel.BasicConsume(queueName, true, consumer);

                        }*/
                        MessageBox.Show("Subscrição feita!\n Mensagem: Desativacao", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                        window.PopulateDataAsync(operatorChannel);
                    }
                    else
                    {
                        MessageBox.Show("Ocorreu um erro! Por favor veja se o numero de administrador está correto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    break;

                case "TERMINACAO":
                    possible = operatorClient.TerminacaoPossible(numero);
                    if (possible.IsPossible_)
                    {
                        MessageBox.Show("O numero de administrador foi registado com sucesso!\nA iniciar a subscrição no Tópico EVENTS...\n Tempo estimado : 5 sec", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);

                        Thread.Sleep(5);

                        /*string exchangeName = "EVENTS";
                        string queueName = "Subscriber";
                        string routingKey = "Terminacao";
                        

                        var factory = new ConnectionFactory() { HostName = "localhost" };
                        using (var connection = factory.CreateConnection())
                        using (var channel = connection.CreateModel())
                        {
                            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);

                            channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);

                            channel.QueueBind(queueName, exchangeName, routingKey);

                            var consumer = new EventingBasicConsumer(channel);

                            consumer.Received += (model, ea) =>
                            {
                                var body = ea.Body.ToArray();
                                var message = Encoding.UTF8.GetString(body);
                                
                                MessageBox.Show("Subscrição feita!\n Mensagem: " + message, "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
                            };

                            channel.BasicConsume(queueName, true, consumer);

                        }*/
                        MessageBox.Show("Subscrição feita!\n Mensagem: Terminação ", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                        window.PopulateDataAsync(operatorChannel);
                    }
                    else
                    {
                        MessageBox.Show("Ocorreu um erro! Por favor veja se o numero de administrador está correto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;

                default:
                    // Nenhum estado válido foi fornecido
                    break;
            }

        }
    }
}
