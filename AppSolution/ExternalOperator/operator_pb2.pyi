from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from typing import ClassVar as _ClassVar, Optional as _Optional

DESCRIPTOR: _descriptor.FileDescriptor

class Instalacao(_message.Message):
    __slots__ = ["Data", "Domicilio", "Estado", "Modalidade", "Tecnologia", "num_Admin"]
    DATA_FIELD_NUMBER: _ClassVar[int]
    DOMICILIO_FIELD_NUMBER: _ClassVar[int]
    Data: str
    Domicilio: str
    ESTADO_FIELD_NUMBER: _ClassVar[int]
    Estado: str
    MODALIDADE_FIELD_NUMBER: _ClassVar[int]
    Modalidade: str
    NUM_ADMIN_FIELD_NUMBER: _ClassVar[int]
    TECNOLOGIA_FIELD_NUMBER: _ClassVar[int]
    Tecnologia: str
    num_Admin: str
    def __init__(self, num_Admin: _Optional[str] = ..., Domicilio: _Optional[str] = ..., Tecnologia: _Optional[str] = ..., Modalidade: _Optional[str] = ..., Estado: _Optional[str] = ..., Data: _Optional[str] = ...) -> None: ...

class IsPossible(_message.Message):
    __slots__ = ["IsPossible"]
    ISPOSSIBLE_FIELD_NUMBER: _ClassVar[int]
    IsPossible: bool
    def __init__(self, IsPossible: bool = ...) -> None: ...

class NumeroAdministrador(_message.Message):
    __slots__ = ["NumAdmin"]
    NUMADMIN_FIELD_NUMBER: _ClassVar[int]
    NumAdmin: int
    def __init__(self, NumAdmin: _Optional[int] = ...) -> None: ...

class Operador(_message.Message):
    __slots__ = ["NameOperador", "PalavraPasse"]
    NAMEOPERADOR_FIELD_NUMBER: _ClassVar[int]
    NameOperador: str
    PALAVRAPASSE_FIELD_NUMBER: _ClassVar[int]
    PalavraPasse: str
    def __init__(self, NameOperador: _Optional[str] = ..., PalavraPasse: _Optional[str] = ...) -> None: ...

class OperadorPerfil(_message.Message):
    __slots__ = ["IsAdmin", "NomeOperador", "PalavraPasse", "id"]
    ID_FIELD_NUMBER: _ClassVar[int]
    ISADMIN_FIELD_NUMBER: _ClassVar[int]
    IsAdmin: bool
    NOMEOPERADOR_FIELD_NUMBER: _ClassVar[int]
    NomeOperador: str
    PALAVRAPASSE_FIELD_NUMBER: _ClassVar[int]
    PalavraPasse: str
    id: int
    def __init__(self, id: _Optional[int] = ..., NomeOperador: _Optional[str] = ..., PalavraPasse: _Optional[str] = ..., IsAdmin: bool = ...) -> None: ...

class VerificarConta(_message.Message):
    __slots__ = ["Existente"]
    EXISTENTE_FIELD_NUMBER: _ClassVar[int]
    Existente: bool
    def __init__(self, Existente: bool = ...) -> None: ...

class tempvar(_message.Message):
    __slots__ = ["tempString"]
    TEMPSTRING_FIELD_NUMBER: _ClassVar[int]
    tempString: str
    def __init__(self, tempString: _Optional[str] = ...) -> None: ...
