/*
    STORED PROCEDURES FOR ASISTENCIA SOLUTION
    Generated based on existing Tables and DTOs
*/

-- =============================================
-- 1. EMPRESA
-- =============================================

CREATE PROCEDURE sp_Empresa_INSERT
    @RazonSocial NVARCHAR(255),
    @RUC CHAR(11),
    @Email VARCHAR(50),
    @Telefono VARCHAR(35),
    @Direccion VARCHAR(200),
    @ConfiguracionAsistencia NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Empresa (RazonSocial, RUC, Email, Telefono, Direccion, ConfiguracionAsistencia, Estado, FechaRegistro)
    VALUES (@RazonSocial, @RUC, @Email, @Telefono, @Direccion, @ConfiguracionAsistencia, 1, GETDATE());
    
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE sp_Empresa_UPDATE
    @IdEmpresa INT,
    @RazonSocial NVARCHAR(255),
    @RUC CHAR(11),
    @Email VARCHAR(50),
    @Telefono VARCHAR(35),
    @Direccion VARCHAR(200),
    @ConfiguracionAsistencia NVARCHAR(MAX)
AS
BEGIN
    UPDATE Empresa
    SET RazonSocial = @RazonSocial,
        RUC = @RUC,
        Email = @Email,
        Telefono = @Telefono,
        Direccion = @Direccion,
        ConfiguracionAsistencia = @ConfiguracionAsistencia
    WHERE IdEmpresa = @IdEmpresa;
END
GO

CREATE PROCEDURE sp_Empresa_DELETE
    @IdEmpresa INT
AS
BEGIN
    -- Soft Delete
    UPDATE Empresa SET Estado = 0 WHERE IdEmpresa = @IdEmpresa;
END
GO

CREATE PROCEDURE sp_Empresa_GETALL
AS
BEGIN
    SELECT * FROM Empresa WHERE Estado = 1;
END
GO

CREATE PROCEDURE sp_Empresa_GETBYID
    @IdEmpresa INT
AS
BEGIN
    SELECT * FROM Empresa WHERE IdEmpresa = @IdEmpresa AND Estado = 1;
END
GO

-- =============================================
-- 2. HORARIO
-- =============================================

CREATE PROCEDURE sp_Horario_INSERT
    @Nombre NVARCHAR(100),
    @HoraEntrada TIME,
    @HoraSalida TIME,
    @ToleranciaEntrada INT,
    @ToleranciaSalida INT,
    @IdEmpresa INT
AS
BEGIN
    INSERT INTO Horario (Nombre, HoraEntrada, HoraSalida, ToleranciaEntrada, ToleranciaSalida, IdEmpresa)
    VALUES (@Nombre, @HoraEntrada, @HoraSalida, @ToleranciaEntrada, @ToleranciaSalida, @IdEmpresa);
    
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE sp_Horario_UPDATE
    @IdHorario INT,
    @Nombre NVARCHAR(100),
    @HoraEntrada TIME,
    @HoraSalida TIME,
    @ToleranciaEntrada INT,
    @ToleranciaSalida INT,
    @IdEmpresa INT
AS
BEGIN
    UPDATE Horario
    SET Nombre = @Nombre,
        HoraEntrada = @HoraEntrada,
        HoraSalida = @HoraSalida,
        ToleranciaEntrada = @ToleranciaEntrada,
        ToleranciaSalida = @ToleranciaSalida,
        IdEmpresa = @IdEmpresa
    WHERE IdHorario = @IdHorario;
END
GO

CREATE PROCEDURE sp_Horario_DELETE
    @IdHorario INT
AS
BEGIN
    DELETE FROM Horario WHERE IdHorario = @IdHorario;
END
GO

CREATE PROCEDURE sp_Horario_GETALL
AS
BEGIN
    SELECT * FROM Horario;
END
GO

CREATE PROCEDURE sp_Horario_GETBYID
    @IdHorario INT
AS
BEGIN
    SELECT * FROM Horario WHERE IdHorario = @IdHorario;
END
GO

-- =============================================
-- 3. EMPLEADO
-- =============================================

CREATE PROCEDURE sp_Empleado_INSERT
    @IdEmpresa INT,
    @DNI CHAR(8),
    @Nombres NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @Cargo NVARCHAR(100)
AS
BEGIN
    INSERT INTO Empleado (IdEmpresa, DNI, Nombres, Apellidos, Cargo, Estado, FechaCreacion)
    VALUES (@IdEmpresa, @DNI, @Nombres, @Apellidos, @Cargo, 1, GETDATE());
    
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE sp_Empleado_UPDATE
    @IdEmpleado INT,
    @IdEmpresa INT,
    @DNI CHAR(8),
    @Nombres NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @Cargo NVARCHAR(100)
AS
BEGIN
    UPDATE Empleado
    SET IdEmpresa = @IdEmpresa,
        DNI = @DNI,
        Nombres = @Nombres,
        Apellidos = @Apellidos,
        Cargo = @Cargo
    WHERE IdEmpleado = @IdEmpleado;
END
GO

CREATE PROCEDURE sp_Empleado_DELETE
    @IdEmpleado INT
AS
BEGIN
    -- Soft Delete
    UPDATE Empleado SET Estado = 0 WHERE IdEmpleado = @IdEmpleado;
END
GO

CREATE PROCEDURE sp_Empleado_GETALL
AS
BEGIN
    SELECT * FROM Empleado WHERE Estado = 1;
END
GO

CREATE PROCEDURE sp_Empleado_GETBYID
    @IdEmpleado INT
AS
BEGIN
    SELECT * FROM Empleado WHERE IdEmpleado = @IdEmpleado AND Estado = 1;
END
GO

-- =============================================
-- 4. USUARIO
-- =============================================

CREATE PROCEDURE sp_Usuario_INSERT
    @IdEmpleado INT,
    @Username NVARCHAR(50),
    @PasswordHash NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Usuario (IdEmpleado, Username, PasswordHash, Estado)
    VALUES (@IdEmpleado, @Username, @PasswordHash, 1);
    
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE sp_Usuario_UPDATE
    @IdUsuario INT,
    @IdEmpleado INT,
    @Username NVARCHAR(50),
    @PasswordHash NVARCHAR(MAX)
AS
BEGIN
    UPDATE Usuario
    SET IdEmpleado = @IdEmpleado,
        Username = @Username,
        PasswordHash = @PasswordHash
    WHERE IdUsuario = @IdUsuario;
END
GO

CREATE PROCEDURE sp_Usuario_DELETE
    @IdUsuario INT
AS
BEGIN
    -- Soft Delete
    UPDATE Usuario SET Estado = 0 WHERE IdUsuario = @IdUsuario;
END
GO

CREATE PROCEDURE sp_Usuario_GETALL
AS
BEGIN
    SELECT * FROM Usuario WHERE Estado = 1;
END
GO

CREATE PROCEDURE sp_Usuario_GETBYID
    @IdUsuario INT
AS
BEGIN
    SELECT * FROM Usuario WHERE IdUsuario = @IdUsuario AND Estado = 1;
END
GO

-- =============================================
-- 5. ROL
-- =============================================

CREATE PROCEDURE sp_Rol_INSERT
    @Nombre NVARCHAR(50),
    @Descripcion NVARCHAR(255)
AS
BEGIN
    INSERT INTO Rol (Nombre, Descripcion)
    VALUES (@Nombre, @Descripcion);
    
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE sp_Rol_UPDATE
    @IdRol INT,
    @Nombre NVARCHAR(50),
    @Descripcion NVARCHAR(255)
AS
BEGIN
    UPDATE Rol
    SET Nombre = @Nombre,
        Descripcion = @Descripcion
    WHERE IdRol = @IdRol;
END
GO

CREATE PROCEDURE sp_Rol_DELETE
    @IdRol INT
AS
BEGIN
    DELETE FROM Rol WHERE IdRol = @IdRol;
END
GO

CREATE PROCEDURE sp_Rol_GETALL
AS
BEGIN
    SELECT * FROM Rol;
END
GO

CREATE PROCEDURE sp_Rol_GETBYID
    @IdRol INT
AS
BEGIN
    SELECT * FROM Rol WHERE IdRol = @IdRol;
END
GO

-- =============================================
-- 6. ROLUSUARIO (Many-to-Many)
-- =============================================

CREATE PROCEDURE sp_RolUsuario_INSERT
    @IdUsuario INT,
    @IdRol INT
AS
BEGIN
    INSERT INTO RolUsuario (IdUsuario, IdRol)
    VALUES (@IdUsuario, @IdRol);
END
GO

CREATE PROCEDURE sp_RolUsuario_DELETE
    @IdUsuario INT,
    @IdRol INT
AS
BEGIN
    DELETE FROM RolUsuario WHERE IdUsuario = @IdUsuario AND IdRol = @IdRol;
END
GO

CREATE PROCEDURE sp_RolUsuario_GETALL
AS
BEGIN
    SELECT * FROM RolUsuario;
END
GO

CREATE PROCEDURE sp_RolUsuario_GETBYUSER
    @IdUsuario INT
AS
BEGIN
    SELECT * FROM RolUsuario WHERE IdUsuario = @IdUsuario;
END
GO

-- =============================================
-- 7. ENROLAMIENTO
-- =============================================

CREATE PROCEDURE sp_Enrolamiento_INSERT
    @IdEmpleado INT,
    @Tipo NVARCHAR(50),
    @IdentificadorBiometrico VARBINARY(MAX)
AS
BEGIN
    INSERT INTO Enrolamiento (IdEmpleado, Tipo, IdentificadorBiometrico, Estado)
    VALUES (@IdEmpleado, @Tipo, @IdentificadorBiometrico, 1);
    
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE sp_Enrolamiento_UPDATE
    @IdEnrolamiento INT,
    @IdEmpleado INT,
    @Tipo NVARCHAR(50),
    @IdentificadorBiometrico VARBINARY(MAX)
AS
BEGIN
    UPDATE Enrolamiento
    SET IdEmpleado = @IdEmpleado,
        Tipo = @Tipo,
        IdentificadorBiometrico = @IdentificadorBiometrico
    WHERE IdEnrolamiento = @IdEnrolamiento;
END
GO

CREATE PROCEDURE sp_Enrolamiento_DELETE
    @IdEnrolamiento INT
AS
BEGIN
    -- Soft Delete
    UPDATE Enrolamiento SET Estado = 0 WHERE IdEnrolamiento = @IdEnrolamiento;
END
GO

CREATE PROCEDURE sp_Enrolamiento_GETALL
AS
BEGIN
    SELECT * FROM Enrolamiento WHERE Estado = 1;
END
GO

CREATE PROCEDURE sp_Enrolamiento_GETBYID
    @IdEnrolamiento INT
AS
BEGIN
    SELECT * FROM Enrolamiento WHERE IdEnrolamiento = @IdEnrolamiento AND Estado = 1;
END
GO

-- =============================================
-- 8. HORARIOEMPLEADO
-- =============================================

CREATE PROCEDURE sp_HorarioEmpleado_INSERT
    @IdEmpleado INT,
    @IdHorario INT,
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    INSERT INTO HorarioEmpleado (IdEmpleado, IdHorario, FechaInicio, FechaFin)
    VALUES (@IdEmpleado, @IdHorario, @FechaInicio, @FechaFin);
    
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE sp_HorarioEmpleado_UPDATE
    @IdHorarioEmpleado INT,
    @IdEmpleado INT,
    @IdHorario INT,
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    UPDATE HorarioEmpleado
    SET IdEmpleado = @IdEmpleado,
        IdHorario = @IdHorario,
        FechaInicio = @FechaInicio,
        FechaFin = @FechaFin
    WHERE IdHorarioEmpleado = @IdHorarioEmpleado;
END
GO

CREATE PROCEDURE sp_HorarioEmpleado_DELETE
    @IdHorarioEmpleado INT
AS
BEGIN
    DELETE FROM HorarioEmpleado WHERE IdHorarioEmpleado = @IdHorarioEmpleado;
END
GO

CREATE PROCEDURE sp_HorarioEmpleado_GETALL
AS
BEGIN
    SELECT * FROM HorarioEmpleado;
END
GO

CREATE PROCEDURE sp_HorarioEmpleado_GETBYID
    @IdHorarioEmpleado INT
AS
BEGIN
    SELECT * FROM HorarioEmpleado WHERE IdHorarioEmpleado = @IdHorarioEmpleado;
END
GO

-- =============================================
-- 9. UBICACIONPERMITIDA
-- =============================================

CREATE PROCEDURE sp_UbicacionPermitida_INSERT
    @IdEmpresa INT,
    @Nombre NVARCHAR(100),
    @Latitud DECIMAL(18, 15),
    @Longitud DECIMAL(18, 15),
    @RadioMetros INT
AS
BEGIN
    INSERT INTO UbicacionPermitida (IdEmpresa, Nombre, Latitud, Longitud, RadioMetros)
    VALUES (@IdEmpresa, @Nombre, @Latitud, @Longitud, @RadioMetros);
    
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE sp_UbicacionPermitida_UPDATE
    @IdUbicacion INT,
    @IdEmpresa INT,
    @Nombre NVARCHAR(100),
    @Latitud DECIMAL(18, 15),
    @Longitud DECIMAL(18, 15),
    @RadioMetros INT
AS
BEGIN
    UPDATE UbicacionPermitida
    SET IdEmpresa = @IdEmpresa,
        Nombre = @Nombre,
        Latitud = @Latitud,
        Longitud = @Longitud,
        RadioMetros = @RadioMetros
    WHERE IdUbicacion = @IdUbicacion;
END
GO

CREATE PROCEDURE sp_UbicacionPermitida_DELETE
    @IdUbicacion INT
AS
BEGIN
    DELETE FROM UbicacionPermitida WHERE IdUbicacion = @IdUbicacion;
END
GO

CREATE PROCEDURE sp_UbicacionPermitida_GETALL
AS
BEGIN
    SELECT * FROM UbicacionPermitida;
END
GO

CREATE PROCEDURE sp_UbicacionPermitida_GETBYID
    @IdUbicacion INT
AS
BEGIN
    SELECT * FROM UbicacionPermitida WHERE IdUbicacion = @IdUbicacion;
END
GO

-- =============================================
-- 10. MARCACION
-- =============================================

CREATE PROCEDURE sp_Marcacion_INSERT
    @IdEmpleado INT,
    @FechaHora DATETIME,
    @TipoMarcacion NVARCHAR(20),
    @Latitud DECIMAL(18, 15),
    @Longitud DECIMAL(18, 15),
    @EsValida BIT
AS
BEGIN
    INSERT INTO Marcacion (IdEmpleado, FechaHora, TipoMarcacion, Latitud, Longitud, EsValida)
    VALUES (@IdEmpleado, @FechaHora, @TipoMarcacion, @Latitud, @Longitud, @EsValida);
    
    SELECT CAST(SCOPE_IDENTITY() as bigint);
END
GO

CREATE PROCEDURE sp_Marcacion_UPDATE
    @IdMarcacion BIGINT,
    @IdEmpleado INT,
    @FechaHora DATETIME,
    @TipoMarcacion NVARCHAR(20),
    @Latitud DECIMAL(18, 15),
    @Longitud DECIMAL(18, 15),
    @EsValida BIT
AS
BEGIN
    UPDATE Marcacion
    SET IdEmpleado = @IdEmpleado,
        FechaHora = @FechaHora,
        TipoMarcacion = @TipoMarcacion,
        Latitud = @Latitud,
        Longitud = @Longitud,
        EsValida = @EsValida
    WHERE IdMarcacion = @IdMarcacion;
END
GO

CREATE PROCEDURE sp_Marcacion_DELETE
    @IdMarcacion BIGINT
AS
BEGIN
    DELETE FROM Marcacion WHERE IdMarcacion = @IdMarcacion;
END
GO

CREATE PROCEDURE sp_Marcacion_GETALL
AS
BEGIN
    SELECT * FROM Marcacion;
END
GO

CREATE PROCEDURE sp_Marcacion_GETBYID
    @IdMarcacion BIGINT
AS
BEGIN
    SELECT * FROM Marcacion WHERE IdMarcacion = @IdMarcacion;
END
GO

-- =============================================
-- 11. REGISTROASISTENCIA
-- =============================================

CREATE PROCEDURE sp_RegistroAsistencia_INSERT
    @IdEmpleado INT,
    @Fecha DATE,
    @HoraEntrada DATETIME,
    @HoraSalida DATETIME,
    @MinutosTarde INT,
    @HorasTrabajadas DECIMAL(5,2),
    @EstadoAsistencia NVARCHAR(50)
AS
BEGIN
    INSERT INTO RegistroAsistencia (IdEmpleado, Fecha, HoraEntrada, HoraSalida, MinutosTarde, HorasTrabajadas, EstadoAsistencia)
    VALUES (@IdEmpleado, @Fecha, @HoraEntrada, @HoraSalida, @MinutosTarde, @HorasTrabajadas, @EstadoAsistencia);
    
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE sp_RegistroAsistencia_UPDATE
    @IdRegistro INT,
    @IdEmpleado INT,
    @Fecha DATE,
    @HoraEntrada DATETIME,
    @HoraSalida DATETIME,
    @MinutosTarde INT,
    @HorasTrabajadas DECIMAL(5,2),
    @EstadoAsistencia NVARCHAR(50)
AS
BEGIN
    UPDATE RegistroAsistencia
    SET IdEmpleado = @IdEmpleado,
        Fecha = @Fecha,
        HoraEntrada = @HoraEntrada,
        HoraSalida = @HoraSalida,
        MinutosTarde = @MinutosTarde,
        HorasTrabajadas = @HorasTrabajadas,
        EstadoAsistencia = @EstadoAsistencia
    WHERE IdRegistro = @IdRegistro;
END
GO

CREATE PROCEDURE sp_RegistroAsistencia_DELETE
    @IdRegistro INT
AS
BEGIN
    DELETE FROM RegistroAsistencia WHERE IdRegistro = @IdRegistro;
END
GO

CREATE PROCEDURE sp_RegistroAsistencia_GETALL
AS
BEGIN
    SELECT * FROM RegistroAsistencia;
END
GO

CREATE PROCEDURE sp_RegistroAsistencia_GETBYID
    @IdRegistro INT
AS
BEGIN
    SELECT * FROM RegistroAsistencia WHERE IdRegistro = @IdRegistro;
END
GO

-- =============================================
-- 12. CONFIGURACION
-- =============================================

CREATE PROCEDURE sp_Configuracion_INSERT
    @IdEmpresa INT,
    @Clave NVARCHAR(100),
    @Valor NVARCHAR(MAX),
    @Logo VARCHAR(150)
AS
BEGIN
    INSERT INTO Configuracion (IdEmpresa, Clave, Valor, Logo)
    VALUES (@IdEmpresa, @Clave, @Valor, @Logo);
    
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE sp_Configuracion_UPDATE
    @IdConfiguracion INT,
    @IdEmpresa INT,
    @Clave NVARCHAR(100),
    @Valor NVARCHAR(MAX),
    @Logo VARCHAR(150)
AS
BEGIN
    UPDATE Configuracion
    SET IdEmpresa = @IdEmpresa,
        Clave = @Clave,
        Valor = @Valor,
        Logo = @Logo
    WHERE IdConfiguracion = @IdConfiguracion;
END
GO

CREATE PROCEDURE sp_Configuracion_DELETE
    @IdConfiguracion INT
AS
BEGIN
    DELETE FROM Configuracion WHERE IdConfiguracion = @IdConfiguracion;
END
GO

CREATE PROCEDURE sp_Configuracion_GETALL
AS
BEGIN
    SELECT * FROM Configuracion;
END
GO

CREATE PROCEDURE sp_Configuracion_GETBYID
    @IdConfiguracion INT
AS
BEGIN
    SELECT * FROM Configuracion WHERE IdConfiguracion = @IdConfiguracion;
END
GO

-- =============================================
-- 13. AUDITORIA
-- =============================================

CREATE PROCEDURE sp_Auditoria_INSERT
    @Entidad NVARCHAR(100),
    @Accion NVARCHAR(50),
    @IdUsuario INT,
    @DetalleAnterior NVARCHAR(MAX),
    @DetalleNuevo NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Auditoria (Entidad, Accion, IdUsuario, Fecha, DetalleAnterior, DetalleNuevo)
    VALUES (@Entidad, @Accion, @IdUsuario, GETDATE(), @DetalleAnterior, @DetalleNuevo);
    
    SELECT CAST(SCOPE_IDENTITY() as bigint);
END
GO

CREATE PROCEDURE sp_Auditoria_UPDATE
    @IdAuditoria BIGINT,
    @Entidad NVARCHAR(100),
    @Accion NVARCHAR(50),
    @IdUsuario INT,
    @DetalleAnterior NVARCHAR(MAX),
    @DetalleNuevo NVARCHAR(MAX)
AS
BEGIN
    UPDATE Auditoria
    SET Entidad = @Entidad,
        Accion = @Accion,
        IdUsuario = @IdUsuario,
        DetalleAnterior = @DetalleAnterior,
        DetalleNuevo = @DetalleNuevo
    WHERE IdAuditoria = @IdAuditoria;
END
GO

CREATE PROCEDURE sp_Auditoria_DELETE
    @IdAuditoria BIGINT
AS
BEGIN
    DELETE FROM Auditoria WHERE IdAuditoria = @IdAuditoria;
END
GO

CREATE PROCEDURE sp_Auditoria_GETALL
AS
BEGIN
    SELECT * FROM Auditoria;
END
GO

CREATE PROCEDURE sp_Auditoria_GETBYID
    @IdAuditoria BIGINT
AS
BEGIN
    SELECT * FROM Auditoria WHERE IdAuditoria = @IdAuditoria;
END
GO
