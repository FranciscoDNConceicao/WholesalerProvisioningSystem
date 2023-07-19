using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminOperador
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BttnConnect_Click(object sender, RoutedEventArgs e)
        {
            // Criar um canal de comunicação com o servidor gRPC
            var channel = GrpcChannel.ForAddress("https://localhost:7016");

            // Criar um objeto Operador com as informações fornecidas pelo usuário
            var AccountOperador = new Operador
            {
                NameOperador = NomeOperadorInput.Text,
                PalavraPasse = PassWordOperatorInput.Password,
            };

            // Criar um cliente gRPC para interagir com o servidor
            var ClientOperator = new Operator.OperatorClient(channel);

            // Verificar se a conta do operador existe no servidor
            var Verificacao = ClientOperator.AccountExist(AccountOperador);

            if (Verificacao.Existente == true)
            {
                // Se a conta existir, abrir a janela de lista de instalações
                InstalListaWindow instalListaWindow = new InstalListaWindow(channel);
                instalListaWindow.Show();
                this.Close();
            }
            else
            {
                // Se a conta não existir, exibir uma mensagem de erro
                MessageBox.Show("Nome ou Palavra Passe errados", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

       
    }
}
