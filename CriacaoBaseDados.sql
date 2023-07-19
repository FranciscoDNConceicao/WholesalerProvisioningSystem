CREATE TABLE Instalacoes (
  num_admin INT NOT NULL,
  Domicilio VARCHAR(255),
  Tecnologia VARCHAR(50),
  Estado VARCHAR(50),
  Modalidade VARCHAR(50),
  Data_Reserva DATE,
  PRIMARY KEY (num_admin)
);
CREATE TABLE Operador (
  id INT NOT NULL,
  NomeOperador VARCHAR(255),
  PalavraPasse VARCHAR(255),
  IsAdmin BIT,
  PRIMARY KEY (id)
);

CREATE TABLE OperadorInstalacao (
  id INT NOT NULL,
  OperadorID INT,
  InstalacaoID INT,
  PRIMARY KEY (id),
  FOREIGN KEY (OperadorID) REFERENCES Operador(id),
  FOREIGN KEY (InstalacaoID) REFERENCES Instalacoes(num_admin)
);
