using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using CsvHelper;

namespace AdminOperador
{
    public partial class InstalListaWindow : Window
    {
        private GrpcChannel channel1;
        public InstalListaWindow(GrpcChannel channel)
        {
            InitializeComponent();
            PopulateDataAsync(channel);
            channel1 = channel;
        }

        public async void PopulateDataAsync(GrpcChannel channel)
        {
            // Limpar os itens existentes no GridInstalacao
            GridInstalacao.Items.Clear();

            // Criar um cliente para se comunicar com o servidor gRPC
            var operatorClient = new Operator.OperatorClient(channel);

            // Lista para armazenar as instalações retornadas do servidor
            List<Instalacao> ListIntal = new List<Instalacao>();

            // Fazer a chamada assíncrona para retornar todas as instalações
            using (var callOp = operatorClient.ReturnAllInstal(new tempvar()))
            {
                // Enquanto houver itens no fluxo de resposta
                while (await callOp.ResponseStream.MoveNext())
                {
                    // Obter a instalação atual
                    var currentRow = callOp.ResponseStream.Current;

                    // Adicionar uma nova linha ao GridInstalacao com os dados da instalação
                    GridInstalacao.Items.Add(new
                    {
                        NumeroAdmin = currentRow.NumAdmin,
                        Domicilio = currentRow.Domicilio,
                        Modalidade = currentRow.Modalidade,
                        Tecnologia = currentRow.Tecnologia,
                        Estado = currentRow.Estado,
                        Date = currentRow.Data
                    });
                }
            }
        }

        private void ReservaBttn_Click(object sender, RoutedEventArgs e)
        {
            // Abrir a janela de reserva
            ReservaWindow reservaWindow = new ReservaWindow(channel1);
            reservaWindow.Show();
            this.Close();
        }

        private void AtivacaoBttn_Click(object sender, RoutedEventArgs e)
        {
            // Abrir a janela para definir o número administrativo e a ação de ativação
            SetNumAdmin setNumAdminWindow = new SetNumAdmin("ATIVACAO", channel1, this);
            setNumAdminWindow.Show();
        }

        private void DesativacaoBttnClick(object sender, RoutedEventArgs e)
        {
            // Abrir a janela para definir o número administrativo e a ação de desativação
            SetNumAdmin setNumAdminWindow = new SetNumAdmin("DESATIVACAO", channel1, this);
            setNumAdminWindow.Show();
        }

        private void TerminacaoBttnClick(object sender, RoutedEventArgs e)
        {
            // Abrir a janela para definir o número administrativo e a ação de terminação
            SetNumAdmin setNumAdminWindow = new SetNumAdmin("TERMINACAO", channel1, this);
            setNumAdminWindow.Show();
        }

        private void GestOperadoresBttn_Click(object sender, RoutedEventArgs e)
        {
            // Abrir a janela de lista de operadores
            OperadListaWindow operadListaWindow = new OperadListaWindow(channel1);
            operadListaWindow.Show();
            this.Close();
        }

        private void DescarregarCsvBttn_Click(object sender, RoutedEventArgs e)
        {
            // Abrir a janela para selecionar um arquivo CSV
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Arquivos CSV (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                List<Instalacao> listaInsta = new List<Instalacao>();
                string caminhoDoArquivo = openFileDialog.FileName;

                // Ler o arquivo CSV e extrair os dados para a lista de instalações
                using (var reader = new StreamReader(caminhoDoArquivo))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    while (csv.Read())
                    {
                        Instalacao instalacaoTemp = new Instalacao();

                        string InstalacaoString = csv.GetField<string>(0);
                        string[] partes = InstalacaoString.Split(';');
                        instalacaoTemp.Domicilio = partes[0];
                        instalacaoTemp.Tecnologia = partes[1];
                        instalacaoTemp.Estado = partes[2];
                        instalacaoTemp.Modalidade = partes[3];
                        instalacaoTemp.Data = partes[4];

                        listaInsta.Add(instalacaoTemp);
                    }
                }

                var operatorClient = new Operator.OperatorClient(channel1);
                IsPossible wasSucessfull = new IsPossible();
                var lista = new ListaInstalacao { Instalacao = { listaInsta } };

                // Enviar a lista de instalações para o servidor para descarregar no banco de dados
                wasSucessfull = operatorClient.DescarregarCSVFile(lista);

                if (wasSucessfull.IsPossible_)
                {
                    MessageBox.Show("Os dados foram implementados com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    PopulateDataAsync(channel1);
                }
                else
                {
                    MessageBox.Show("Ocorreu um erro", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor selecione um arquivo CSV", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}
