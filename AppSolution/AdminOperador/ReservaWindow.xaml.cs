using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for ReservaWindow.xaml
    /// </summary>
    public partial class ReservaWindow : Window
    {
        private GrpcChannel operatorChannel;

        public ReservaWindow(GrpcChannel operatorChannel1)
        {
            InitializeComponent();
            operatorChannel = operatorChannel1;

        }
        private void CarregarReserva_Click(object sender, RoutedEventArgs e)
        {
            // Verificar se algum campo está vazio
            if (DomicilioBoxInput.Text == null || TecnologiaComboBox.Text == null || ModalidadeBoxInput.Text == null)
            {
                MessageBox.Show("Por favor Insira não deixe campos vazios", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Criar um objeto de instalação e um objeto de número administrativo
            Instalacao instalacaoTemp = new Instalacao();
            NumeroAdministrador numAdmin = new NumeroAdministrador();

            // Preencher os detalhes da instalação
            instalacaoTemp.Estado = "RESERVA";
            instalacaoTemp.Domicilio = DomicilioBoxInput.Text;
            instalacaoTemp.Tecnologia = TecnologiaComboBox.Text;
            instalacaoTemp.Data = DateTime.Now.ToString();
            instalacaoTemp.Modalidade = ModalidadeBoxInput.Text;

            // Criar um cliente gRPC para interagir com o servidor
            var operatorClient = new Operator.OperatorClient(operatorChannel);

            // Chamar o método do servidor para verificar a possibilidade de reserva
            numAdmin = operatorClient.ReservaPossible(instalacaoTemp);

            // Exibir mensagem de sucesso com o número administrativo
            MessageBox.Show("Reservado a instalação com o número administrativo " + numAdmin.NumAdmin.ToString(), "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            // Abrir a janela de lista de instalações
            InstalListaWindow instalListaWindow = new InstalListaWindow(operatorChannel);
            instalListaWindow.Show();
            this.Close();
        }
    }
}
