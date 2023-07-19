using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdminOperador
{
    /// <summary>
    /// Interaction logic for OperadListaWindow.xaml
    /// </summary>
    public partial class OperadListaWindow : Window
    {
        private GrpcChannel channel1;
        public OperadListaWindow(GrpcChannel grpcChannel)
        {
            InitializeComponent();
            _ = ReadAllOperaAsync(grpcChannel);
            channel1 = grpcChannel;
        }
        private async Task ReadAllOperaAsync(GrpcChannel channel)
        {
            // Criar um cliente gRPC para interagir com o servidor
            var operatorClient = new Operator.OperatorClient(channel);
            List<OperadorPerfil> ListIntal = new List<OperadorPerfil>();

            // Chamar o método do servidor para retornar todos os operadores
            using (var callOp = operatorClient.ReturnAllOperador(new tempvar()))
            {
                while (await callOp.ResponseStream.MoveNext())
                {
                    var currentRow = callOp.ResponseStream.Current;

                    ListIntal.Add(currentRow);
                }
            }

            // Atribuir a lista de operadores à origem de dados da grade
            GridOperador.ItemsSource = ListIntal;
        }

        private void InstalListBttn_Click(object sender, RoutedEventArgs e)
        {
            // Abrir a janela de lista de instalações
            InstalListaWindow instalListaWindow = new InstalListaWindow(channel1);
            instalListaWindow.Show();
            this.Close();
        }

        private void ElimOperadorBttn_Click(object sender, RoutedEventArgs e)
        {
            if (GridOperador.SelectedItem != null)
            {
                // Obter a linha selecionada da grade
                var selectedRow = (OperadorPerfil)GridOperador.SelectedItem;
                int numOperador = selectedRow.Id;

                var operatorClient = new Operator.OperatorClient(channel1);
                IsPossible isEliminated = new IsPossible();

                // Chamar o método do servidor para eliminar o operador selecionado
                isEliminated = operatorClient.EliminarOperador(selectedRow);

                if (isEliminated.IsPossible_)
                {
                    MessageBox.Show("A conta foi eliminada com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    _ = ReadAllOperaAsync(channel1);
                }
                else
                {
                    MessageBox.Show("Ocorreu um erro", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Selecione uma linha", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AdminMudBttn_Click(object sender, RoutedEventArgs e)
        {
            if (GridOperador.SelectedItem != null)
            {
                // Obter a linha selecionada da grade
                var selectedRow = (OperadorPerfil)GridOperador.SelectedItem;
                if (!selectedRow.IsAdmin)
                {
                    var operatorClient = new Operator.OperatorClient(channel1);
                    IsPossible isChanged = new IsPossible();

                    // Chamar o método do servidor para transformar o operador em administrador
                    isChanged = operatorClient.OperadorToAdmin(selectedRow);

                    if (isChanged.IsPossible_)
                    {
                        MessageBox.Show("O operador foi mudado com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                        _ = ReadAllOperaAsync(channel1);
                    }
                    else
                    {
                        MessageBox.Show("O operador não foi mudado com sucesso", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Este operador já é um Administrador!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Selecione uma linha", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OperadorMudBttn_Click(object sender, RoutedEventArgs e)
        {
            if (GridOperador.SelectedItem != null)
            {
                // Obter a linha selecionada da grade
                var selectedRow = (OperadorPerfil)GridOperador.SelectedItem;
                if (selectedRow.IsAdmin)
                {
                    var operatorClient = new Operator.OperatorClient(channel1);
                    IsPossible isChanged = new IsPossible();

                    // Chamar o método do servidor para transformar o administrador em operador externo
                    isChanged = operatorClient.AdminToOperador(selectedRow);

                    if (isChanged.IsPossible_)
                    {
                        MessageBox.Show("O administrador foi mudado com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                        _ = ReadAllOperaAsync(channel1);
                    }
                    else
                    {
                        MessageBox.Show("O administrador não foi mudado com sucesso", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Este operador já é um Operador Externo!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Selecione uma linha", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void NovoOperadorBttn_Click(object sender, RoutedEventArgs e)
        {
            // Abrir a janela de adicionar operador
            AddOperatorWindow addOperatorWindow = new AddOperatorWindow(channel1);
            addOperatorWindow.Show();
            this.Close();
        }

    }
}
