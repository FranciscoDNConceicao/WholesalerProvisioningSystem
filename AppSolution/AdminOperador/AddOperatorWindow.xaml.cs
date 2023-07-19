using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
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
    /// Interaction logic for AddOperatorWindow.xaml
    /// </summary>
    public partial class AddOperatorWindow : Window
    {
        private GrpcChannel channel1;
        public AddOperatorWindow(GrpcChannel grpcChannel)
        {
            InitializeComponent();
            channel1 = grpcChannel;
        }

        private void CarregarOperadorBttn_Click(object sender, RoutedEventArgs e)
        {
            if (UsernameBoxInput.Text == null || PassWordBoxInput.Text == null || IsAdminComboBox.Text == null)
            {
                MessageBox.Show("Por favor Insira não deixe campos vazios", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                OperadListaWindow operadListaWindo = new OperadListaWindow(channel1);
                operadListaWindo.Show();
                this.Close();
            }
            var operatorClient = new Operator.OperatorClient(channel1);
            OperadorPerfil operadorPerfil = new OperadorPerfil();

            operadorPerfil.NomeOperador = UsernameBoxInput.Text;
            operadorPerfil.PalavraPasse = PassWordBoxInput.Text;
            if(IsAdminComboBox.Text == "Sim")
            {
                operadorPerfil.IsAdmin = true;
            }
            else
            {
                operadorPerfil.IsAdmin = false;
            }

            IsPossible hasSucess = new IsPossible();
            hasSucess = operatorClient.AdicionarOperador(operadorPerfil);

            if (hasSucess.IsPossible_)
            {
                MessageBox.Show("O operador foi adicionado com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Ocorreu um erro ao adicionar o operador", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            OperadListaWindow operadListaWindow = new OperadListaWindow(channel1);
            operadListaWindow.Show();
            this.Close();
        }
            
        
    }
}
