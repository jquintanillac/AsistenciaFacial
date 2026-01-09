-- Script to insert default roles
-- Run this script in your SQL Server database

IF NOT EXISTS (SELECT 1 FROM Rol WHERE Nombre = 'Administrador')
BEGIN
    INSERT INTO Rol (Nombre, Descripcion) VALUES ('Administrador', 'Acceso total al sistema y configuración');
END

IF NOT EXISTS (SELECT 1 FROM Rol WHERE Nombre = 'Supervisor')
BEGIN
    INSERT INTO Rol (Nombre, Descripcion) VALUES ('Supervisor', 'Gestión de equipos, horarios y aprobación de asistencia');
END

IF NOT EXISTS (SELECT 1 FROM Rol WHERE Nombre = 'Colaborador')
BEGIN
    INSERT INTO Rol (Nombre, Descripcion) VALUES ('Colaborador', 'Registro de asistencia y consulta de historial propio');
END
