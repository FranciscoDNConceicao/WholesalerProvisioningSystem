# Generated by the gRPC Python protocol compiler plugin. DO NOT EDIT!
"""Client and server classes corresponding to protobuf-defined services."""
import grpc

import operator_pb2 as operator__pb2


class OperatorStub(object):
    """Missing associated documentation comment in .proto file."""

    def __init__(self, channel):
        """Constructor.

        Args:
            channel: A grpc.Channel.
        """
        self.ReturnAllInstal = channel.unary_stream(
                '/Operator/ReturnAllInstal',
                request_serializer=operator__pb2.tempvar.SerializeToString,
                response_deserializer=operator__pb2.Instalacao.FromString,
                )
        self.ReturnAllOperador = channel.unary_stream(
                '/Operator/ReturnAllOperador',
                request_serializer=operator__pb2.tempvar.SerializeToString,
                response_deserializer=operator__pb2.OperadorPerfil.FromString,
                )
        self.AccountExist = channel.unary_unary(
                '/Operator/AccountExist',
                request_serializer=operator__pb2.Operador.SerializeToString,
                response_deserializer=operator__pb2.VerificarConta.FromString,
                )
        self.AccoutnExistOperadorExt = channel.unary_unary(
                '/Operator/AccoutnExistOperadorExt',
                request_serializer=operator__pb2.Operador.SerializeToString,
                response_deserializer=operator__pb2.VerificarConta.FromString,
                )
        self.ReservaPossible = channel.unary_unary(
                '/Operator/ReservaPossible',
                request_serializer=operator__pb2.Instalacao.SerializeToString,
                response_deserializer=operator__pb2.NumeroAdministrador.FromString,
                )
        self.AtivacaoPossible = channel.unary_unary(
                '/Operator/AtivacaoPossible',
                request_serializer=operator__pb2.NumeroAdministrador.SerializeToString,
                response_deserializer=operator__pb2.IsPossible.FromString,
                )
        self.DesativacaoPossible = channel.unary_unary(
                '/Operator/DesativacaoPossible',
                request_serializer=operator__pb2.NumeroAdministrador.SerializeToString,
                response_deserializer=operator__pb2.IsPossible.FromString,
                )
        self.TerminacaoPossible = channel.unary_unary(
                '/Operator/TerminacaoPossible',
                request_serializer=operator__pb2.NumeroAdministrador.SerializeToString,
                response_deserializer=operator__pb2.IsPossible.FromString,
                )
        self.EliminarOperador = channel.unary_unary(
                '/Operator/EliminarOperador',
                request_serializer=operator__pb2.OperadorPerfil.SerializeToString,
                response_deserializer=operator__pb2.IsPossible.FromString,
                )
        self.AdicionarOperador = channel.unary_unary(
                '/Operator/AdicionarOperador',
                request_serializer=operator__pb2.OperadorPerfil.SerializeToString,
                response_deserializer=operator__pb2.IsPossible.FromString,
                )
        self.OperadorToAdmin = channel.unary_unary(
                '/Operator/OperadorToAdmin',
                request_serializer=operator__pb2.OperadorPerfil.SerializeToString,
                response_deserializer=operator__pb2.IsPossible.FromString,
                )
        self.AdminToOperador = channel.unary_unary(
                '/Operator/AdminToOperador',
                request_serializer=operator__pb2.OperadorPerfil.SerializeToString,
                response_deserializer=operator__pb2.IsPossible.FromString,
                )


class OperatorServicer(object):
    """Missing associated documentation comment in .proto file."""

    def ReturnAllInstal(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def ReturnAllOperador(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def AccountExist(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def AccoutnExistOperadorExt(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def ReservaPossible(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def AtivacaoPossible(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def DesativacaoPossible(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def TerminacaoPossible(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def EliminarOperador(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def AdicionarOperador(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def OperadorToAdmin(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def AdminToOperador(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')


def add_OperatorServicer_to_server(servicer, server):
    rpc_method_handlers = {
            'ReturnAllInstal': grpc.unary_stream_rpc_method_handler(
                    servicer.ReturnAllInstal,
                    request_deserializer=operator__pb2.tempvar.FromString,
                    response_serializer=operator__pb2.Instalacao.SerializeToString,
            ),
            'ReturnAllOperador': grpc.unary_stream_rpc_method_handler(
                    servicer.ReturnAllOperador,
                    request_deserializer=operator__pb2.tempvar.FromString,
                    response_serializer=operator__pb2.OperadorPerfil.SerializeToString,
            ),
            'AccountExist': grpc.unary_unary_rpc_method_handler(
                    servicer.AccountExist,
                    request_deserializer=operator__pb2.Operador.FromString,
                    response_serializer=operator__pb2.VerificarConta.SerializeToString,
            ),
            'AccoutnExistOperadorExt': grpc.unary_unary_rpc_method_handler(
                    servicer.AccoutnExistOperadorExt,
                    request_deserializer=operator__pb2.Operador.FromString,
                    response_serializer=operator__pb2.VerificarConta.SerializeToString,
            ),
            'ReservaPossible': grpc.unary_unary_rpc_method_handler(
                    servicer.ReservaPossible,
                    request_deserializer=operator__pb2.Instalacao.FromString,
                    response_serializer=operator__pb2.NumeroAdministrador.SerializeToString,
            ),
            'AtivacaoPossible': grpc.unary_unary_rpc_method_handler(
                    servicer.AtivacaoPossible,
                    request_deserializer=operator__pb2.NumeroAdministrador.FromString,
                    response_serializer=operator__pb2.IsPossible.SerializeToString,
            ),
            'DesativacaoPossible': grpc.unary_unary_rpc_method_handler(
                    servicer.DesativacaoPossible,
                    request_deserializer=operator__pb2.NumeroAdministrador.FromString,
                    response_serializer=operator__pb2.IsPossible.SerializeToString,
            ),
            'TerminacaoPossible': grpc.unary_unary_rpc_method_handler(
                    servicer.TerminacaoPossible,
                    request_deserializer=operator__pb2.NumeroAdministrador.FromString,
                    response_serializer=operator__pb2.IsPossible.SerializeToString,
            ),
            'EliminarOperador': grpc.unary_unary_rpc_method_handler(
                    servicer.EliminarOperador,
                    request_deserializer=operator__pb2.OperadorPerfil.FromString,
                    response_serializer=operator__pb2.IsPossible.SerializeToString,
            ),
            'AdicionarOperador': grpc.unary_unary_rpc_method_handler(
                    servicer.AdicionarOperador,
                    request_deserializer=operator__pb2.OperadorPerfil.FromString,
                    response_serializer=operator__pb2.IsPossible.SerializeToString,
            ),
            'OperadorToAdmin': grpc.unary_unary_rpc_method_handler(
                    servicer.OperadorToAdmin,
                    request_deserializer=operator__pb2.OperadorPerfil.FromString,
                    response_serializer=operator__pb2.IsPossible.SerializeToString,
            ),
            'AdminToOperador': grpc.unary_unary_rpc_method_handler(
                    servicer.AdminToOperador,
                    request_deserializer=operator__pb2.OperadorPerfil.FromString,
                    response_serializer=operator__pb2.IsPossible.SerializeToString,
            ),
    }
    generic_handler = grpc.method_handlers_generic_handler(
            'Operator', rpc_method_handlers)
    server.add_generic_rpc_handlers((generic_handler,))


 # This class is part of an EXPERIMENTAL API.
class Operator(object):
    """Missing associated documentation comment in .proto file."""

    @staticmethod
    def ReturnAllInstal(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_stream(request, target, '/Operator/ReturnAllInstal',
            operator__pb2.tempvar.SerializeToString,
            operator__pb2.Instalacao.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

    @staticmethod
    def ReturnAllOperador(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_stream(request, target, '/Operator/ReturnAllOperador',
            operator__pb2.tempvar.SerializeToString,
            operator__pb2.OperadorPerfil.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

    @staticmethod
    def AccountExist(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/Operator/AccountExist',
            operator__pb2.Operador.SerializeToString,
            operator__pb2.VerificarConta.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

    @staticmethod
    def AccoutnExistOperadorExt(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/Operator/AccoutnExistOperadorExt',
            operator__pb2.Operador.SerializeToString,
            operator__pb2.VerificarConta.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

    @staticmethod
    def ReservaPossible(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/Operator/ReservaPossible',
            operator__pb2.Instalacao.SerializeToString,
            operator__pb2.NumeroAdministrador.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

    @staticmethod
    def AtivacaoPossible(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/Operator/AtivacaoPossible',
            operator__pb2.NumeroAdministrador.SerializeToString,
            operator__pb2.IsPossible.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

    @staticmethod
    def DesativacaoPossible(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/Operator/DesativacaoPossible',
            operator__pb2.NumeroAdministrador.SerializeToString,
            operator__pb2.IsPossible.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

    @staticmethod
    def TerminacaoPossible(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/Operator/TerminacaoPossible',
            operator__pb2.NumeroAdministrador.SerializeToString,
            operator__pb2.IsPossible.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

    @staticmethod
    def EliminarOperador(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/Operator/EliminarOperador',
            operator__pb2.OperadorPerfil.SerializeToString,
            operator__pb2.IsPossible.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

    @staticmethod
    def AdicionarOperador(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/Operator/AdicionarOperador',
            operator__pb2.OperadorPerfil.SerializeToString,
            operator__pb2.IsPossible.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

    @staticmethod
    def OperadorToAdmin(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/Operator/OperadorToAdmin',
            operator__pb2.OperadorPerfil.SerializeToString,
            operator__pb2.IsPossible.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

    @staticmethod
    def AdminToOperador(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/Operator/AdminToOperador',
            operator__pb2.OperadorPerfil.SerializeToString,
            operator__pb2.IsPossible.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)
