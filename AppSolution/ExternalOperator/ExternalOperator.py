#Bibliotecas necessarias para o funcionamento da aplicação
import grpc
import operator_pb2 as operatorVar
import operator_pb2_grpc as operatorFunc
from colorama import Fore, Style
from datetime import datetime
import time
import pika
import getpass

#Variaveis globais
global channel 
global stub

#Conectar em grpc com o Servidor
channel = grpc.insecure_channel('localhost:5016')
stub = operatorFunc.OperatorStub(channel)

#Função que valida o login dada o username e a password
def ValidateLogin(username, password):
    #Chama a função AccountExistOperador ao servidor GRPC de modo a que este verifique se existe na base de dados
    OperatorTemp = operatorVar.Operador(NameOperador=username, PalavraPasse=password)
    IsValid = stub.AccoutnExistOperadorExt(OperatorTemp)
    #Retorna True se estiver na base de dados, senão retorna false
    if(IsValid.Existente == True):
        return True
    else:
        return False

#Função que desenha um titulo na consola
def printTitle(title):
    width = len(title) + 6
    line = "=" * (width + 2)
    padding = " " * ((width - len(title)) // 2)
    
    print(line)
    print(f"|{padding}{title}{padding}|")
    print(line)

#Função que vai buscar todos os Domicilios existentes na base de dados
def ShowAllDomicilios():
    printTitle("Sistema de Domicilios")
    
    message = operatorVar.tempvar(tempString="exemplo")
    #Chama a função ReturnAllInstall ao servidor GRPC para obter os Domicilios
    response_iterator = stub.ReturnAllInstal(message)
    
    print("=============================================================================================")
    print("| Numero de Administrador - Domicilios - Tecnologia - Modalidade - Estado - Data da Reserva |")
    print("=============================================================================================")

    #A cada linha do return da função equivale a um domicilio
    for line in response_iterator:
        #Conforme o estado altera a cor
        estilo = Fore.WHITE
        if(line.Estado == "ATIVACAO"):
            estilo = Fore.GREEN
        elif(line.Estado == "TERMINACAO"):
            estilo = Fore.LIGHTWHITE_EX
        elif(line.Estado == "DESATIVACAO"):
            estilo = Fore.RED
        elif(line.Estado == "RESERVA"):
            estilo = Fore.YELLOW
        #Imprime cada domicilio na consola
        print(f"|  {line.num_Admin} -"+ Fore.BLUE + f" {line.Domicilio} "+ Style.RESET_ALL + f" - {line.Tecnologia} - {line.Modalidade} -"+ estilo + f" {line.Estado} "+ Style.RESET_ALL + f"- {line.Data}")
    print("===========================================================================================\n")

#Função que desenha o MENU da aplicação com as opções disponiveis para o operador escolher
def Menu():
    print("===============================")
    print("|             MENU            |")
    print("===============================")
    print("|1-Mostrar todos os domicilios|")
    print("|2-Ativacao de domicilio      |")
    print("|3-Desativacao de domicilio   |")
    print("|4-Terminacao de domicilio    |")
    print("|5-Reserva de domicilio       |")
    print("|6-Sair                       |")
    print("===============================")

#Função de subscrição que vai buscar mensagens ao servidor
def callback(ch, method, properties, body):
    print("Mensagem recebida:", body)

#Função que reserva domicilios
def ReservaDomicilios():
    printTitle("Reserva de Domicilio")
    #Inputs com as  informações do domicilio a ser reservado colocadas pelo Operador
    NomeDomicilioTemp = str(input("Nome do Domicilio: "))
    TecnologiaTemp = str(input("Tecnologia: "))
    ModalidadeTemp = str(input("Modalidade: "))

    instalacaoTemp = operatorVar.Instalacao(
        num_Admin=str(0),
        Domicilio=NomeDomicilioTemp,
        Tecnologia=TecnologiaTemp,
        Modalidade=ModalidadeTemp,
        Estado="RESERVA",
        Data=datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        )
    #Chama a função ReservaPossible que reserva a Instalação na base de dados
    NumAdmin = stub.ReservaPossible(instalacaoTemp)
    
    #Se foi sucedida a reserva ou não
    if(str(NumAdmin) == "NumAdmin:0\n"):
        print(Fore.RED + "Ocorreu um problema ao reservar o domicilio" + Style.RESET_ALL)
    else:
        print(Fore.GREEN + f"\n A reserva foi um sucesso, com o {str(NumAdmin)}"+ Style.RESET_ALL)

#Ativação de um Domicilio que esteja reservado ou ativado
def AtivacaoDomicilios():
    printTitle("Ativacao de Domicilios")
    
    #Input para o operador colocar um numero de Administrador
    NumAdministrador = operatorVar.NumeroAdministrador();
    NumAdministrador.NumAdmin = int(input("Numero de Administrador: "))
    
    giveErrorTemp = operatorVar.IsPossible();
    #Chama a função ao servidor GRPC para verificar se é possivel a ativacao
    giveErrorTemp = stub.AtivacaoPossible(NumAdministrador)

    if(giveErrorTemp.IsPossible == True):
        #A ativação foi bem sucedida
        print(Fore.GREEN + "\n A ativacao foi um sucesso \n Por favor aguarde pela subscricao\n Tempo Estimado: 5sec\n")
        
        #Subscrição no topico na queue="Ativacao" atraves do RabbitMQ
        """
        connection = pika.BlockingConnection(pika.ConnectionParameters('localhost'))
        channel = connection.channel()

        channel.queue_declare(queue='ativacao')

        channel.basic_consume(queue='ativacao', on_message_callback=callback, auto_ack=True)

        print('Aguardando mensagens...')
        channel.start_consuming()"""
        #Simulacao de tempo com o time sleep
        time.sleep(5)
        print("Subscricao feita!\n Mensagem: "+ Style.RESET_ALL)
    else:
        print(Fore.RED + "Ocorreu um problema ao ativar o domicilio \n Por favor verifique se o domicilio encontra se reservado\n" + Style.RESET_ALL)
    time.sleep(2)

def DesativacaoDomicilios():
    printTitle("Desativacao de Domicilios")
     #Input para o operador colocar um numero de Administrador
    NumAdministrador = operatorVar.NumeroAdministrador();
    NumAdministrador.NumAdmin = int(input("Numero de Administrador: "))
    
    giveErrorTemp = operatorVar.IsPossible();
    #Chama a função ao servidor GRPC para verificar se é possivel a desativacao
    giveErrorTemp = stub.DesativacaoPossible(NumAdministrador)

    if(giveErrorTemp.IsPossible == True):
         #A desativação foi bem sucedida
        print(Fore.GREEN + "\n A desativacao foi um sucesso \n Por favor aguarde pela subscricao\n")
        
        #Subscrição no topico na queue="desativacao" atraves do RabbitMQ
        #connection = pika.BlockingConnection(pika.ConnectionParameters('localhost'))
        #channel = connection.channel()
        #channel.queue_declare(queue='desativacao')
        #channel.basic_consume(queue='desativacao', on_message_callback=callback, auto_ack=True)
        #print('Aguardando mensagens...')
        #channel.start_consuming()
        #Simulacao de tempo com o time sleep
        
        time.sleep(5)
        print("Subscricao feita!\n Mensagem: "+ Style.RESET_ALL)
    else:
        print(Fore.RED + "Ocorreu um problema ao desativar o domicilio \n Por favor verifique se o domicilio encontra se ativado\n" + Style.RESET_ALL)
    time.sleep(2)

def TerminacaoDomicilios():
    printTitle("Terminacao de Domicilios")
     #Input para o operador colocar um numero de Administrador
    NumAdministrador = operatorVar.NumeroAdministrador();
    NumAdministrador.NumAdmin = int(input("Numero de Administrador: "))
    
    giveErrorTemp = operatorVar.IsPossible();
    #Chama a função ao servidor GRPC para verificar se é possivel a terminação
    giveErrorTemp = stub.TerminacaoPossible(NumAdministrador)

    if(giveErrorTemp.IsPossible == True):
        #A terminacao foi bem sucedida
        print(Fore.GREEN + "\n A terminacao foi um sucesso \n Por favor aguarde pela subscricao\n")
        
        #Subscrição no topico na queue="terminacao" atraves do RabbitMQ
        #connection = pika.BlockingConnection(pika.ConnectionParameters('localhost'))
        #channel = connection.channel()

        #channel.queue_declare(queue='terminacao')

        #channel.basic_consume(queue='terminacao', on_message_callback=callback, auto_ack=True)

        #print('Aguardando mensagens...')
        #channel.start_consuming()
        #Simulacao de tempo com o time sleep
        time.sleep(5)
        print("Subscricao feita!\n Mensagem: "+ Style.RESET_ALL)
    else:
        print(Fore.RED + "Ocorreu um problema ao terminar o domicilio \n Por favor verifique se o domicilio encontra se desativado\n" + Style.RESET_ALL)
    time.sleep(2)


LoginValidation = False

#Loop que inicia o sistema com o formulario de login
while(LoginValidation == False):
    
    printTitle("Sistema de aprovisionamento WholeSaler")
    username = str(input("Username: "))
    #Esconde a palavra passe
    password = getpass.getpass("Password: ")
    #Chama a função feita em cima
    LoginValidation = ValidateLogin(username, password);
    if(LoginValidation == True):
        print(Fore.GREEN + f"\nSeja bem-vindo {username}!\n"+ Style.RESET_ALL)
        break
    else:
        print(Fore.RED + "Credenciais nao aceites\nPor favor tente novamente ... \n" + Style.RESET_ALL)
   
#Mostra todos os domicilios
ShowAllDomicilios()

while True:
    Menu()
    opcao = int(input("Insira a sua opcao:"))
    #Funções feitas em relação ao menu
    match opcao:
        case 1:
            ShowAllDomicilios()
        case 2:
            AtivacaoDomicilios()
        case 3:
            DesativacaoDomicilios()
        case 4:
            TerminacaoDomicilios()
        case 5:
            ReservaDomicilios()
        case 6:
            break;
