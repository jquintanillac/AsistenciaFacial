-- Actualización de la tabla Enrolamiento
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Enrolamiento]') AND name = 'RutaImagen')
BEGIN
    ALTER TABLE [dbo].[Enrolamiento] ADD [RutaImagen] NVARCHAR(255) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Enrolamiento]') AND name = 'DescriptorFacial')
BEGIN
    ALTER TABLE [dbo].[Enrolamiento] ADD [DescriptorFacial] NVARCHAR(MAX) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Enrolamiento]') AND name = 'FechaRegistro')
BEGIN
    ALTER TABLE [dbo].[Enrolamiento] ADD [FechaRegistro] DATETIME DEFAULT GETDATE();
END
GO

-- Actualización del Stored Procedure sp_Enrolamiento_INSERT
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_Enrolamiento_INSERT')
BEGIN
    DROP PROCEDURE sp_Enrolamiento_INSERT
END
GO

CREATE PROCEDURE [dbo].[sp_Enrolamiento_INSERT]
    @IdEmpleado INT,
    @Tipo NVARCHAR(50),
    @IdentificadorBiometrico VARBINARY(MAX),
    @RutaImagen NVARCHAR(255) = NULL,
    @DescriptorFacial NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Enrolamiento (IdEmpleado, Tipo, IdentificadorBiometrico, RutaImagen, DescriptorFacial, FechaRegistro, Estado)
    VALUES (@IdEmpleado, @Tipo, @IdentificadorBiometrico, @RutaImagen, @DescriptorFacial, GETDATE(), 1);
    
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
GO

-- Actualización del Stored Procedure sp_Enrolamiento_UPDATE
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_Enrolamiento_UPDATE')
BEGIN
    DROP PROCEDURE sp_Enrolamiento_UPDATE
END
GO

CREATE PROCEDURE [dbo].[sp_Enrolamiento_UPDATE]
    @IdEnrolamiento INT,
    @IdEmpleado INT,
    @Tipo NVARCHAR(50),
    @IdentificadorBiometrico VARBINARY(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Enrolamiento
    SET IdEmpleado = @IdEmpleado,
        Tipo = @Tipo,
        IdentificadorBiometrico = @IdentificadorBiometrico
    WHERE IdEnrolamiento = @IdEnrolamiento;
END
GO

-- Actualización del Stored Procedure sp_Enrolamiento_GETALL
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_Enrolamiento_GETALL')
BEGIN
    DROP PROCEDURE sp_Enrolamiento_GETALL
END
GO

CREATE PROCEDURE [dbo].[sp_Enrolamiento_GETALL]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT IdEnrolamiento, IdEmpleado, Tipo, IdentificadorBiometrico, RutaImagen, DescriptorFacial, FechaRegistro, Estado
    FROM Enrolamiento
    WHERE Estado = 1;
END
GO

-- Actualización del Stored Procedure sp_Enrolamiento_GETBYID
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_Enrolamiento_GETBYID')
BEGIN
    DROP PROCEDURE sp_Enrolamiento_GETBYID
END
GO

CREATE PROCEDURE [dbo].[sp_Enrolamiento_GETBYID]
    @IdEnrolamiento INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT IdEnrolamiento, IdEmpleado, Tipo, IdentificadorBiometrico, RutaImagen, DescriptorFacial, FechaRegistro, Estado
    FROM Enrolamiento
    WHERE IdEnrolamiento = @IdEnrolamiento;
END
GO
