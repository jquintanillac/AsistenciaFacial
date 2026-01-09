-- 1. ENTIDAD: Empresa (SaaS Root)
CREATE TABLE Empresa (
    IdEmpresa INT PRIMARY KEY IDENTITY(1,1),
    RazonSocial NVARCHAR(255) NOT NULL,
    RUC CHAR(11) UNIQUE NOT NULL,
	Email VARCHAR(50) NULL,
	Telefono VARCHAR(35) NULL,
	Direccion VARCHAR(200) NULL,
    Estado BIT DEFAULT 1, -- 1: Activo, 0: Inactivo
    ConfiguracionAsistencia NVARCHAR(MAX), -- JSON para reglas flexibles
    FechaRegistro DATETIME DEFAULT GETDATE()
);

-- 2. ENTIDAD: Horario
CREATE TABLE Horario (
    IdHorario INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    HoraEntrada TIME NOT NULL,
    HoraSalida TIME NOT NULL,
    ToleranciaEntrada INT DEFAULT 0, -- En minutos
    ToleranciaSalida INT DEFAULT 0,
    IdEmpresa INT FOREIGN KEY REFERENCES Empresa(IdEmpresa)
);

-- 3. ENTIDAD: Empleado
CREATE TABLE Empleado (
    IdEmpleado INT PRIMARY KEY IDENTITY(1,1),
    IdEmpresa INT FOREIGN KEY REFERENCES Empresa(IdEmpresa),
    DNI CHAR(8) UNIQUE NOT NULL,
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    Cargo NVARCHAR(100),
    Estado BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);

-- 4. ENTIDAD: Usuario (Acceso al sistema)
CREATE TABLE Usuario (
    IdUsuario INT PRIMARY KEY IDENTITY(1,1),
    IdEmpleado INT NULL FOREIGN KEY REFERENCES Empleado(IdEmpleado),
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Estado BIT DEFAULT 1
);

-- 5. ENTIDAD: Rol y RolUsuario
CREATE TABLE Rol (
    IdRol INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(255)
);

CREATE TABLE RolUsuario (
    IdUsuario INT FOREIGN KEY REFERENCES Usuario(IdUsuario),
    IdRol INT FOREIGN KEY REFERENCES Rol(IdRol),
    PRIMARY KEY (IdUsuario, IdRol)
);

-- 6. ENTIDAD: Enrolamiento (Biometría)
CREATE TABLE Enrolamiento (
    IdEnrolamiento INT PRIMARY KEY IDENTITY(1,1),
    IdEmpleado INT FOREIGN KEY REFERENCES Empleado(IdEmpleado),
    Tipo NVARCHAR(50), -- 'Facial', 'QR'
    IdentificadorBiometrico VARBINARY(MAX) NOT NULL, -- O NVARCHAR si es un Template/Hash
    Estado BIT DEFAULT 1
);

-- 7. ENTIDAD: HorarioEmpleado (Historial y asignación)
CREATE TABLE HorarioEmpleado (
    IdHorarioEmpleado INT PRIMARY KEY IDENTITY(1,1),
    IdEmpleado INT FOREIGN KEY REFERENCES Empleado(IdEmpleado),
    IdHorario INT FOREIGN KEY REFERENCES Horario(IdHorario),
    FechaInicio DATE NOT NULL,
    FechaFin DATE NULL -- NULL si es el horario actual vigente
);

-- 8. ENTIDAD: UbicacionPermitida (Rutas y Geofencing)
CREATE TABLE UbicacionPermitida (
    IdUbicacion INT PRIMARY KEY IDENTITY(1,1),
    IdEmpresa INT FOREIGN KEY REFERENCES Empresa(IdEmpresa),
    Nombre NVARCHAR(100),
    Latitud DECIMAL(18, 15),
    Longitud DECIMAL(18, 15),
    RadioMetros INT DEFAULT 100
);

-- 9. ENTIDAD: Marcacion (Datos crudos del campo)
CREATE TABLE Marcacion (
    IdMarcacion BIGINT PRIMARY KEY IDENTITY(1,1),
    IdEmpleado INT FOREIGN KEY REFERENCES Empleado(IdEmpleado),
    FechaHora DATETIME NOT NULL,
    TipoMarcacion NVARCHAR(20), -- 'ENTRADA', 'SALIDA'
    Latitud DECIMAL(18, 15),
    Longitud DECIMAL(18, 15),
    --Origen NVARCHAR(50), -- 'Movil', 'Web'
    EsValida BIT DEFAULT 0 -- Flag tras procesar contra reglas
);

-- 10. ENTIDAD: RegistroAsistencia (Resultado final para reportes)
CREATE TABLE RegistroAsistencia (
    IdRegistro INT PRIMARY KEY IDENTITY(1,1),
    IdEmpleado INT FOREIGN KEY REFERENCES Empleado(IdEmpleado),
    Fecha DATE NOT NULL,
    HoraEntrada DATETIME NULL,
    HoraSalida DATETIME NULL,
    MinutosTarde INT DEFAULT 0,
    HorasTrabajadas DECIMAL(5,2) DEFAULT 0,
    EstadoAsistencia NVARCHAR(50) -- 'PUNTUAL', 'TARDANZA', 'FALTA'
);

-- 11. ENTIDAD: Configuracion (Parámetros SaaS)
CREATE TABLE Configuracion (
    IdConfiguracion INT PRIMARY KEY IDENTITY(1,1),
    IdEmpresa INT FOREIGN KEY REFERENCES Empresa(IdEmpresa),
    Clave NVARCHAR(100) NOT NULL,
    Valor NVARCHAR(MAX) NOT NULL,
	Logo  VARCHAR(150) NULL
);

-- 12. ENTIDAD: Auditoria
CREATE TABLE Auditoria (
    IdAuditoria BIGINT PRIMARY KEY IDENTITY(1,1),
    Entidad NVARCHAR(100),
    Accion NVARCHAR(50), -- 'INSERT', 'UPDATE', 'DELETE'
    IdUsuario INT,
    Fecha DATETIME DEFAULT GETDATE(),
    DetalleAnterior NVARCHAR(MAX),
    DetalleNuevo NVARCHAR(MAX)
);