using Grpc.Core;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static ServidorGRPC.Operator;
using RabbitMQ.Client;
using System.Text;

namespace ServidorGRPC.Services
{
    public class OperatorService : Operator.OperatorBase
    {
        private readonly ILogger<OperatorService> _logger;
        private string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TelecomunicDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // Função responsável por publicar uma mensagem no servidor RabbitMQ
        private void PublishMessage(string message, string routingKey)
        {
            string exchangeName = "EVENTS";
            
            // Configurar a conexão e o canal com o servidor RabbitMQ
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Declarar a exchange
                channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);

                // Publicar a mensagem na exchange com a routing key
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchangeName, routingKey, null, body);

            }
        }

        private void MudaEstado(int numAdmin, string Estado)
        {
            // Query para atualizar o estado na tabela Instalacoes
            string query = "UPDATE Instalacoes SET Estado = @Estado WHERE num_admin = @num_admin";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Adicionar o parâmetro para o número administrativo
                    command.Parameters.AddWithValue("@num_admin", numAdmin);
                    // Adicionar o parâmetro para o novo estado
                    command.Parameters.AddWithValue("@Estado", Estado);

                    // Executar a atualização do estado
                    int rowsAffected = command.ExecuteNonQuery();

                    // O número de linhas afetadas pela atualização é retornado em rowsAffected
                    // Podemos utilizar esse valor para verificar se a atualização foi bem-sucedida ou não
                }

                connection.Close();
            }
        }

        public OperatorService(ILogger<OperatorService> logger)
        {
            _logger = logger;
        }

        public override Task<VerificarConta> AccountExist(Operador request, ServerCallContext context)
        {

            // Instanciar o objeto VerificarConta e definir a propriedade Existente como false
            VerificarConta verificarConta = new VerificarConta();
            verificarConta.Existente = false;

            // Criar uma conexão com o banco de dados SQL Server
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Definir a query SQL para verificar a existência da conta
                string query = "SELECT COUNT(*) FROM Operador WHERE NomeOperador = @OperatorName AND IsAdmin = 1 AND PalavraPasse = @Password";

                // Criar um comando SQL e associá-lo à conexão e à query
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Adicionar os parâmetros ao comando SQL
                    command.Parameters.AddWithValue("@OperatorName", request.NameOperador);
                    command.Parameters.AddWithValue("@Password", request.PalavraPasse);

                    // Executar a consulta e obter o número de linhas retornadas
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    // Verificar se o número de linhas é maior que 0, indicando que a conta existe
                    if (count > 0)
                    {
                        verificarConta.Existente = true;
                    }
                }
            }
            // Retornar o objeto VerificarConta encapsulado em uma Task
            return Task.FromResult(verificarConta);
        }

        public override Task<VerificarConta> AccoutnExistOperadorExt(Operador request, ServerCallContext context)
        {

            // Instanciar o objeto VerificarConta e definir a propriedade Existente como false
            VerificarConta verificarConta = new VerificarConta();
            verificarConta.Existente = false;

            // Criar uma conexão com o banco de dados SQL Server
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Definir a query SQL para verificar a existência da conta
                string query = "SELECT COUNT(*) FROM Operador WHERE NomeOperador = @OperatorName AND PalavraPasse = @Password";

                // Criar um comando SQL e associá-lo à conexão e à query
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Adicionar os parâmetros ao comando SQL
                    command.Parameters.AddWithValue("@OperatorName", request.NameOperador);
                    command.Parameters.AddWithValue("@Password", request.PalavraPasse);

                    // Executar a consulta e obter o número de linhas retornadas
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    // Verificar se o número de linhas é maior que 0, indicando que a conta existe
                    if (count > 0)
                    {
                        verificarConta.Existente = true;
                    }
                }
            }

            // Retornar o objeto VerificarConta encapsulado em uma Task
            return Task.FromResult(verificarConta);
        }

        public override async Task ReturnAllInstal(tempvar request, IServerStreamWriter<Instalacao> responseStream, ServerCallContext context)
        {
            // Criar uma lista para armazenar as instalações
            List<Instalacao> instalacaoList = new List<Instalacao>();

            // Definir a query SQL para selecionar todas as instalações
            string query = "SELECT * FROM Instalacoes";

            // Criar uma conexão com o banco de dados SQL Server
            using SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            // Criar um comando SQL e associá-lo à conexão e à query
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Executar o comando e obter um leitor de dados
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Ler os dados retornados pelo leitor
                    while (reader.Read())
                    {
                        // Criar um objeto Instalacao temporário
                        Instalacao instalacaoTemp = new Instalacao();
                        instalacaoTemp.NumAdmin = reader.GetInt32(0).ToString();
                        instalacaoTemp.Domicilio = reader.GetString(1);
                        instalacaoTemp.Tecnologia = reader.GetString(2);
                        instalacaoTemp.Estado = reader.GetString(3);
                        instalacaoTemp.Modalidade = reader.GetString(4);
                        instalacaoTemp.Data = reader.GetDateTime(5).ToString();

                        // Adicionar a instalação à lista
                        instalacaoList.Add(instalacaoTemp);
                    }
                }
            }

            // Enviar cada instalação para o fluxo de resposta
            foreach (var instalacao in instalacaoList)
            {
                await responseStream.WriteAsync(instalacao);
            }

        }
        
        public override Task<NumeroAdministrador> ReservaPossible(Instalacao request, ServerCallContext context)
        {
            NumeroAdministrador numero = new NumeroAdministrador();

            int lastAdminNumber = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Obter o último número administrativo da tabela
                string getLastAdminNumQuery = "SELECT ISNULL(MAX(num_admin), 0) FROM Instalacoes";
                using (SqlCommand command = new SqlCommand(getLastAdminNumQuery, connection))
                {
                    lastAdminNumber = Convert.ToInt32(command.ExecuteScalar());
                }

                // Inserir uma nova instalação na tabela
                string insertQuery = "INSERT INTO Instalacoes (num_admin, Domicilio, Tecnologia, Estado, Modalidade, Data_Reserva) " +
                                     "VALUES (@NumAdministrativo, @Domicilio, @Tecnologia, @Estado, @Modalidade, @DataReserva)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Definir os parâmetros da query com os valores da instalação recebida
                    command.Parameters.AddWithValue("@NumAdministrativo", lastAdminNumber + 1);
                    command.Parameters.AddWithValue("@Domicilio", request.Domicilio);
                    command.Parameters.AddWithValue("@Tecnologia", request.Tecnologia);
                    command.Parameters.AddWithValue("@Estado", request.Estado);
                    command.Parameters.AddWithValue("@Modalidade", request.Modalidade);
                    command.Parameters.AddWithValue("@DataReserva", DateTime.Parse(request.Data));

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            // Definir o número administrativo da instalação inserida
            numero.NumAdmin = lastAdminNumber + 1;
            return Task.FromResult(numero);
        }

        public override Task<IsPossible> AtivacaoPossible(NumeroAdministrador request, ServerCallContext context)
        {
            IsPossible isPossible = new IsPossible();
            isPossible.IsPossible_ = false;

            int numAdmin = request.NumAdmin;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Verificar se a instalação com o número administrativo fornecido está disponível para ativação
                string query = "SELECT 1 FROM Instalacoes WHERE num_admin = @NumAdministrativo AND (Estado = 'DESATIVACAO' OR Estado = 'RESERVA')";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NumAdministrativo", numAdmin);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    // A instalação está disponível para ativação
                    isPossible.IsPossible_ = true;
                    //PublishMessage("Ativacao do processo: " + request.NumAdmin.ToString(), "Ativacao");
                    MudaEstado(numAdmin, "ATIVACAO"); // Chama o método MudaEstado para atualizar o estado da instalação para "ATIVACAO"
                }

                reader.Close();
            }

            return Task.FromResult(isPossible);
        }

        public override Task<IsPossible> DesativacaoPossible(NumeroAdministrador request, ServerCallContext context)
        {
            IsPossible isPossible = new IsPossible();
            isPossible.IsPossible_ = false;

            int numAdmin = request.NumAdmin;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Verificar se a instalação com o número administrativo fornecido está disponível para ativação
                string query = "SELECT 1 FROM Instalacoes WHERE num_admin = @NumAdministrativo AND (Estado = 'DESATIVACAO' OR Estado = 'RESERVA')";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NumAdministrativo", numAdmin);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    // A instalação está disponível para ativação
                    isPossible.IsPossible_ = true;
                    //PublishMessage("Ativacao do processo: " + request.NumAdmin.ToString(), "Ativacao");
                    MudaEstado(numAdmin, "ATIVACAO"); // Chama o método MudaEstado para atualizar o estado da instalação para "ATIVACAO"
                }

                reader.Close();
            }

            return Task.FromResult(isPossible);
        }

        public override Task<IsPossible> TerminacaoPossible(NumeroAdministrador request, ServerCallContext context)
        {
            IsPossible isPossible = new IsPossible();
            isPossible.IsPossible_ = false;

            int numAdmin = request.NumAdmin;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Verificar se a instalação com o número administrativo fornecido está disponível para terminação
                string query = "SELECT 1 FROM Instalacoes WHERE num_admin = @NumAdministrativo AND (Estado = 'DESATIVACAO')";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NumAdministrativo", numAdmin);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    // A instalação está disponível para terminação
                    isPossible.IsPossible_ = true;
                    //PublishMessage("Terminacao do processo: " + request.NumAdmin.ToString(), "Terminacao");
                    MudaEstado(numAdmin, "TERMINACAO"); // Chama o método MudaEstado para atualizar o estado da instalação para "TERMINACAO"
                }

                reader.Close();
            }

            return Task.FromResult(isPossible);
        }

        public override async Task ReturnAllOperador(tempvar request, IServerStreamWriter<OperadorPerfil> responseStream, ServerCallContext context)
        {
            List<OperadorPerfil> OperadorPerfis = new List<OperadorPerfil>();

            string query = "SELECT * FROM Operador";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Executar a consulta SQL para obter todos os perfis de operador
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OperadorPerfil operadorPerfilTemp = new OperadorPerfil();
                            operadorPerfilTemp.Id = reader.GetInt32(0);
                            operadorPerfilTemp.NomeOperador = reader.GetString(1);
                            operadorPerfilTemp.PalavraPasse = reader.GetString(2);
                            operadorPerfilTemp.IsAdmin = reader.GetBoolean(3);

                            OperadorPerfis.Add(operadorPerfilTemp);
                        }
                    }
                }
                // Escrever cada perfil de operador no fluxo de resposta
                foreach (var operador in OperadorPerfis)
                {
                    await responseStream.WriteAsync(operador);
                }
            }
        }

        public override Task<IsPossible> EliminarOperador(OperadorPerfil request, ServerCallContext context)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                IsPossible isPossible = new IsPossible();
                connection.Open();
                
                string sql = "DELETE FROM Operador WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", request.Id);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        isPossible.IsPossible_ = true;
                    }
                    else
                    {
                        isPossible.IsPossible_ = false;
                    }
                }
                return Task.FromResult(isPossible);
            }
        }

        public override Task<IsPossible> AdicionarOperador(OperadorPerfil request, ServerCallContext context)
        {

            int lastIdNumber = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Consulta para obter o último ID da tabela Operador
                string getLastAdminNumQuery = "SELECT ISNULL(MAX(id), 0) FROM Operador";
                using (SqlCommand command = new SqlCommand(getLastAdminNumQuery, connection))
                {
                    lastIdNumber = Convert.ToInt32(command.ExecuteScalar());
                }

                // Comando SQL para inserir um novo registro na tabela Operador
                string insertQuery = "INSERT INTO Operador (id, NomeOperador, PalavraPasse, IsAdmin) " +
                                     "VALUES (@id, @Nome, @Pass, @IsAdmin)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Parâmetros do comando SQL
                    command.Parameters.AddWithValue("@id", lastIdNumber + 1);
                    command.Parameters.AddWithValue("@Nome", request.NomeOperador);
                    command.Parameters.AddWithValue("@Pass", request.PalavraPasse);
                    command.Parameters.AddWithValue("@IsAdmin", request.IsAdmin);

                    // Executa o comando SQL para inserir o novo registro
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            // Criação do objeto IsPossible para indicar se a adição foi possível ou não
            IsPossible isPossible = new IsPossible();
            isPossible.IsPossible_ = true;
            if (lastIdNumber == 0)
            {
                isPossible.IsPossible_ = false;
            }

            return Task.FromResult(isPossible);
        }

        public override Task<IsPossible> OperadorToAdmin(OperadorPerfil request, ServerCallContext context)
        {
            IsPossible isPossible = new IsPossible();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Comando SQL para atualizar o campo IsAdmin para 1 na tabela Operador
                string sql = "UPDATE Operador SET IsAdmin = 1 WHERE id = @Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Parâmetro do comando SQL
                    command.Parameters.AddWithValue("@Id", request.Id);

                    // Executa o comando SQL para atualizar o registro
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        isPossible.IsPossible_ = true;
                    }
                    else
                    {
                        isPossible.IsPossible_ = false;
                    }
                }

                connection.Close();
            }

            return Task.FromResult(isPossible);
        }

        public override Task<IsPossible> AdminToOperador(OperadorPerfil request, ServerCallContext context)
        {
            IsPossible isPossible = new IsPossible();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Comando SQL para atualizar o campo IsAdmin para 0 na tabela Operador
                string sql = "UPDATE Operador SET IsAdmin = 0 WHERE id = @Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Parâmetro do comando SQL
                    command.Parameters.AddWithValue("@Id", request.Id);

                    // Executa o comando SQL para atualizar o registro
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        isPossible.IsPossible_ = true;
                    }
                    else
                    {
                        isPossible.IsPossible_ = false;
                    }
                }

                connection.Close();
            }

            return Task.FromResult(isPossible);
        }

        public override Task<IsPossible> DescarregarCSVFile(ListaInstalacao request, ServerCallContext context)
        {
            List<Instalacao> instalacaos = new List<Instalacao>();
            instalacaos = request.Instalacao.ToList();

            int lastAdminNumber = 0;

            IsPossible isPossible = new IsPossible();
            isPossible.IsPossible_ = false;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Obter o último número de administração existente
                string getLastAdminNumQuery = "SELECT ISNULL(MAX(num_admin), 0) FROM Instalacoes";
                using (SqlCommand command = new SqlCommand(getLastAdminNumQuery, connection))
                {
                    lastAdminNumber = Convert.ToInt32(command.ExecuteScalar());
                }
                lastAdminNumber++;

                // Percorrer a lista de instalações
                foreach (Instalacao instalacao in instalacaos)
                {
                    DateTime dataReserva;
                    DateTime.TryParse(instalacao.Data, out dataReserva);

                    // Construir a consulta SQL para inserir uma nova instalação na tabela
                    string query = $"INSERT INTO Instalacoes (num_admin, Domicilio, Tecnologia, Estado, Modalidade, Data_Reserva) VALUES ('{lastAdminNumber}', '{instalacao.Domicilio}', '{instalacao.Tecnologia}','{instalacao.Estado}','{instalacao.Modalidade}','{dataReserva.ToString("yyyy-MM-dd HH:mm:ss")}')";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Executar a consulta SQL para inserir a instalação
                        command.ExecuteNonQuery();
                    }

                    lastAdminNumber++;
                }

                // Definir a flag IsPossible como true, indicando que a operação foi bem-sucedida
                isPossible.IsPossible_ = true;
            }

            return Task.FromResult(isPossible);
        }


    }

    
}
