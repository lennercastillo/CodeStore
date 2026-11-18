
CREATE DATABASE CodeStore;
GO

USE CodeStore;
GO

CREATE TABLE Roles (
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    CodigoRol VARCHAR(10) NOT NULL UNIQUE,
    NombreRol VARCHAR(50) NOT NULL UNIQUE,
    Descripcion VARCHAR(255) NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    IdRol INT NOT NULL,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    Correo VARCHAR(120) NOT NULL UNIQUE,
    PasswordHash VARCHAR(256) NOT NULL,
    Telefono VARCHAR(20) NULL,
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (IdRol) REFERENCES Roles(IdRol)
);

CREATE TABLE Clientes (
    IdCliente INT IDENTITY(1,1) PRIMARY KEY,
    NumeroIdentificacion VARCHAR(25) NOT NULL UNIQUE,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20) NULL,
    Correo VARCHAR(120) NULL,
    Direccion VARCHAR(250) NULL,
    Activo BIT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Proveedores (
    IdProveedor INT IDENTITY(1,1) PRIMARY KEY,
    NumeroRUC VARCHAR(25) NOT NULL UNIQUE,
    RazonSocial VARCHAR(150) NOT NULL,
    NombreContacto VARCHAR(100) NULL,
    Telefono VARCHAR(20) NULL,
    Correo VARCHAR(120) NULL,
    Direccion VARCHAR(250) NULL,
    Activo BIT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Productos (
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
    CodigoSKU VARCHAR(50) NOT NULL UNIQUE,
    Nombre VARCHAR(150) NOT NULL,
    Marca VARCHAR(80) NOT NULL,
    Modelo VARCHAR(80) NOT NULL,
    TipoEquipo VARCHAR(80) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    PrecioVenta DECIMAL(18,2) NOT NULL CHECK (PrecioVenta >= 0),
    UltimoCostoCompra DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (UltimoCostoCompra >= 0),
    CostoPromedio DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (CostoPromedio >= 0),
    StockActual INT NOT NULL DEFAULT 0 CHECK (StockActual >= 0),
    StockMinimo INT NOT NULL DEFAULT 1 CHECK (StockMinimo >= 0),
    MesesGarantia INT NOT NULL DEFAULT 0 CHECK (MesesGarantia >= 0),
    RequiereSerie BIT NOT NULL DEFAULT 0,
    IdProveedorPreferido INT NULL,
    Activo BIT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Productos_Proveedores FOREIGN KEY (IdProveedorPreferido) REFERENCES Proveedores(IdProveedor)
);

CREATE TABLE Compras (
    IdCompra INT IDENTITY(1,1) PRIMARY KEY,
    NumeroDocumento VARCHAR(50) NOT NULL,
    IdProveedor INT NOT NULL,
    IdUsuarioRegistro INT NOT NULL,
    IdUsuarioRecepcion INT NULL,
    FechaCompra DATETIME NOT NULL DEFAULT GETDATE(),
    FechaRecepcion DATETIME NULL,
    TotalCompra DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (TotalCompra >= 0),
    Estado VARCHAR(20) NOT NULL DEFAULT 'Pendiente' 
        CHECK (Estado IN ('Pendiente', 'Recibida', 'Anulada')),
    Observaciones VARCHAR(255) NULL,
    IdUsuarioAnulacion INT NULL,
    FechaAnulacion DATETIME NULL,
    MotivoAnulacion VARCHAR(255) NULL,
    CONSTRAINT FK_Compras_Proveedores FOREIGN KEY (IdProveedor) REFERENCES Proveedores(IdProveedor),
    CONSTRAINT FK_Compras_UsuarioRegistro FOREIGN KEY (IdUsuarioRegistro) REFERENCES Usuarios(IdUsuario),
    CONSTRAINT FK_Compras_UsuarioRecepcion FOREIGN KEY (IdUsuarioRecepcion) REFERENCES Usuarios(IdUsuario),
    CONSTRAINT FK_Compras_UsuarioAnulacion FOREIGN KEY (IdUsuarioAnulacion) REFERENCES Usuarios(IdUsuario)
);

CREATE TABLE DetalleCompras (
    IdDetalleCompra INT IDENTITY(1,1) PRIMARY KEY,
    IdCompra INT NOT NULL,
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    CostoUnitario DECIMAL(18,2) NOT NULL CHECK (CostoUnitario > 0),
    Subtotal AS (Cantidad * CostoUnitario) PERSISTED,
    CONSTRAINT FK_DetalleCompras_Compras FOREIGN KEY (IdCompra) REFERENCES Compras(IdCompra),
    CONSTRAINT FK_DetalleCompras_Productos FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto)
);

CREATE TABLE Ventas (
    IdVenta INT IDENTITY(1,1) PRIMARY KEY,
    NumeroFactura VARCHAR(30) NOT NULL UNIQUE,
    IdCliente INT NOT NULL,
    IdUsuarioVendedor INT NOT NULL,
    FechaVenta DATETIME NOT NULL DEFAULT GETDATE(),
    TipoVenta VARCHAR(20) NOT NULL CHECK (TipoVenta IN ('Contado', 'Credito')),
    MetodoPago VARCHAR(30) NOT NULL CHECK (MetodoPago IN ('Efectivo', 'Tarjeta', 'Transferencia', 'Mixto', 'Credito')),
    PorcentajeImpuesto DECIMAL(5,2) NOT NULL DEFAULT 15.00 CHECK (PorcentajeImpuesto >= 0),
    Subtotal DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (Subtotal >= 0),
    Impuesto DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (Impuesto >= 0),
    TotalVenta DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (TotalVenta >= 0),
    MontoRecibido DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (MontoRecibido >= 0),
    Vuelto DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (Vuelto >= 0),
    Estado VARCHAR(20) NOT NULL DEFAULT 'Pendiente'
        CHECK (Estado IN ('Pendiente', 'Completada', 'Anulada')),
    IdUsuarioAutorizaAnulacion INT NULL,
    FechaAnulacion DATETIME NULL,
    MotivoAnulacion VARCHAR(255) NULL,
    CONSTRAINT FK_Ventas_Clientes FOREIGN KEY (IdCliente) REFERENCES Clientes(IdCliente),
    CONSTRAINT FK_Ventas_UsuarioVendedor FOREIGN KEY (IdUsuarioVendedor) REFERENCES Usuarios(IdUsuario),
    CONSTRAINT FK_Ventas_UsuarioAnulacion FOREIGN KEY (IdUsuarioAutorizaAnulacion) REFERENCES Usuarios(IdUsuario)
);

CREATE TABLE DetalleVentas (
    IdDetalleVenta INT IDENTITY(1,1) PRIMARY KEY,
    IdVenta INT NOT NULL,
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario DECIMAL(18,2) NOT NULL CHECK (PrecioUnitario >= 0),
    CostoUnitarioHistorico DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (CostoUnitarioHistorico >= 0),
    Subtotal AS (Cantidad * PrecioUnitario) PERSISTED,
    MesesGarantiaHistorico INT NOT NULL DEFAULT 0,
    FechaFinGarantia DATE NULL,
    CONSTRAINT FK_DetalleVentas_Ventas FOREIGN KEY (IdVenta) REFERENCES Ventas(IdVenta),
    CONSTRAINT FK_DetalleVentas_Productos FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto)
);

CREATE TABLE DetalleVentaSeries (
    IdSerieVendida INT IDENTITY(1,1) PRIMARY KEY,
    IdDetalleVenta INT NOT NULL,
    NumeroSerie VARCHAR(100) NOT NULL UNIQUE,
    EstadoSerie VARCHAR(30) NOT NULL DEFAULT 'Vendido'
        CHECK (EstadoSerie IN ('Vendido', 'En Garantia', 'Reemplazado', 'Devuelto por Anulacion')),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_DetalleVentaSeries_DetalleVentas FOREIGN KEY (IdDetalleVenta) REFERENCES DetalleVentas(IdDetalleVenta)
);

CREATE TABLE Creditos (
    IdCredito INT IDENTITY(1,1) PRIMARY KEY,
    IdVenta INT NOT NULL UNIQUE,
    MontoVentaTotal DECIMAL(18,2) NOT NULL CHECK (MontoVentaTotal > 0),
    PrimaInicial DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (PrimaInicial >= 0),
    MontoFinanciado DECIMAL(18,2) NOT NULL CHECK (MontoFinanciado > 0),
    FrecuenciaPago VARCHAR(20) NOT NULL CHECK (FrecuenciaPago IN ('Semanal', 'Quincenal', 'Mensual')),
    CantidadCuotas INT NOT NULL CHECK (CantidadCuotas > 0),
    PorcentajeInteres DECIMAL(5,2) NOT NULL DEFAULT 0 CHECK (PorcentajeInteres >= 0),
    MontoInteresesPactados DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (MontoInteresesPactados >= 0),
    MontoTotalCredito DECIMAL(18,2) NOT NULL CHECK (MontoTotalCredito > 0),
    SaldoPendiente DECIMAL(18,2) NOT NULL CHECK (SaldoPendiente >= 0),
    Estado VARCHAR(20) NOT NULL DEFAULT 'Activo'
        CHECK (Estado IN ('Activo', 'Pagado', 'EnMora', 'Anulado')),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Creditos_Ventas FOREIGN KEY (IdVenta) REFERENCES Ventas(IdVenta)
);

CREATE TABLE CuotasCredito (
    IdCuota INT IDENTITY(1,1) PRIMARY KEY,
    IdCredito INT NOT NULL,
    NumeroCuota INT NOT NULL CHECK (NumeroCuota > 0),
    FechaVencimiento DATE NOT NULL,
    MontoCuota DECIMAL(18,2) NOT NULL CHECK (MontoCuota > 0),
    RecargoMora DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (RecargoMora >= 0),
    MoraAbonada DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (MoraAbonada >= 0),
    CapitalInteresAbonado DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (CapitalInteresAbonado >= 0),
    MontoAbonado AS (MoraAbonada + CapitalInteresAbonado) PERSISTED,
    SaldoCuota AS ((MontoCuota + RecargoMora) - (MoraAbonada + CapitalInteresAbonado)) PERSISTED,
    Estado VARCHAR(20) NOT NULL DEFAULT 'Pendiente'
        CHECK (Estado IN ('Pendiente', 'Parcial', 'Pagada', 'Vencida', 'Anulada')),
    CONSTRAINT FK_CuotasCredito_Creditos FOREIGN KEY (IdCredito) REFERENCES Creditos(IdCredito),
    CONSTRAINT UQ_Credito_NumeroCuota UNIQUE (IdCredito, NumeroCuota)
);

CREATE TABLE Abonos (
    IdAbono INT IDENTITY(1,1) PRIMARY KEY,
    NumeroRecibo VARCHAR(30) NOT NULL UNIQUE,
    IdCuota INT NOT NULL,
    IdUsuarioRegistro INT NOT NULL,
    FechaAbono DATETIME NOT NULL DEFAULT GETDATE(),
    MontoAbono DECIMAL(18,2) NOT NULL CHECK (MontoAbono > 0),
    MontoAplicadoMora DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (MontoAplicadoMora >= 0),
    MontoAplicadoCapital DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (MontoAplicadoCapital >= 0),
    MetodoPago VARCHAR(30) NOT NULL CHECK (MetodoPago IN ('Efectivo', 'Tarjeta', 'Transferencia')),
    Observaciones VARCHAR(255) NULL,
    Estado VARCHAR(20) NOT NULL DEFAULT 'Activo' CHECK (Estado IN ('Activo', 'Anulado')),
    IdUsuarioAnulacion INT NULL,
    FechaAnulacion DATETIME NULL,
    MotivoAnulacion VARCHAR(255) NULL,
    CONSTRAINT FK_Abonos_CuotasCredito FOREIGN KEY (IdCuota) REFERENCES CuotasCredito(IdCuota),
    CONSTRAINT FK_Abonos_UsuarioRegistro FOREIGN KEY (IdUsuarioRegistro) REFERENCES Usuarios(IdUsuario),
    CONSTRAINT FK_Abonos_UsuarioAnulacion FOREIGN KEY (IdUsuarioAnulacion) REFERENCES Usuarios(IdUsuario)
);

CREATE TABLE Kardex (
    IdMovimiento INT IDENTITY(1,1) PRIMARY KEY,
    IdProducto INT NOT NULL,
    FechaMovimiento DATETIME NOT NULL DEFAULT GETDATE(),
    TipoMovimiento VARCHAR(40) NOT NULL 
        CHECK (TipoMovimiento IN (
            'Entrada por Compra', 
            'Salida por Venta', 
            'Reversion por Anulacion Venta',
            'Reversion por Anulacion Compra',
            'Ajuste de Entrada', 
            'Ajuste de Salida', 
            'Baja de Inventario', 
            'Salida por Reemplazo Garantia'
        )),
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    StockAnterior INT NOT NULL CHECK (StockAnterior >= 0),
    StockNuevo INT NOT NULL CHECK (StockNuevo >= 0),
    CostoUnitario DECIMAL(18,2) NOT NULL DEFAULT 0,
    DocumentoOrigen VARCHAR(50) NULL,
    IdReferenciaOrigen INT NULL,
    Justificacion VARCHAR(255) NULL,
    IdUsuarioResponsable INT NOT NULL,
    CONSTRAINT FK_Kardex_Productos FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto),
    CONSTRAINT FK_Kardex_Usuarios FOREIGN KEY (IdUsuarioResponsable) REFERENCES Usuarios(IdUsuario)
);

CREATE TABLE ReclamosGarantia (
    IdGarantia INT IDENTITY(1,1) PRIMARY KEY,
    NumeroReclamo VARCHAR(30) NOT NULL UNIQUE,
    IdDetalleVenta INT NOT NULL,
    IdSerieVendida INT NULL,
    IdUsuarioTecnico INT NOT NULL,
    FechaReclamo DATETIME NOT NULL DEFAULT GETDATE(),
    MotivoFalla VARCHAR(500) NOT NULL,
    EstadoRevision VARCHAR(30) NOT NULL DEFAULT 'Recibido'
        CHECK (EstadoRevision IN ('Recibido', 'En Revision', 'En Reparacion', 'Resuelto', 'Rechazado')),
    AccionTomada VARCHAR(40) NULL
        CHECK (AccionTomada IN ('Reparacion Tecnica', 'Reemplazo de Equipo', 'Nota de Credito', 'No Aplica Garantia')),
    DescripcionResolucion VARCHAR(500) NULL,
    FechaResolucion DATETIME NULL,
    CONSTRAINT FK_Garantias_DetalleVentas FOREIGN KEY (IdDetalleVenta) REFERENCES DetalleVentas(IdDetalleVenta),
    CONSTRAINT FK_Garantias_DetalleVentaSeries FOREIGN KEY (IdSerieVendida) REFERENCES DetalleVentaSeries(IdSerieVendida),
    CONSTRAINT FK_Garantias_Usuarios FOREIGN KEY (IdUsuarioTecnico) REFERENCES Usuarios(IdUsuario)
);

CREATE TABLE OrdenesReparacion (
    IdOrdenReparacion INT IDENTITY(1,1) PRIMARY KEY,
    NumeroOrden VARCHAR(30) NOT NULL UNIQUE,
    IdGarantia INT NULL,
    IdCliente INT NOT NULL,
    IdUsuarioTecnico INT NOT NULL,
    DescripcionEquipo VARCHAR(150) NOT NULL,
    Marca VARCHAR(80) NOT NULL,
    Modelo VARCHAR(80) NOT NULL,
    NumeroSerie VARCHAR(100) NOT NULL,
    DiagnosticoTecnico VARCHAR(500) NULL,
    EstadoEquipo VARCHAR(30) NOT NULL DEFAULT 'Recibido'
        CHECK (EstadoEquipo IN ('Recibido', 'En Diagnostico', 'En Reparacion', 'Esperando Repuesto', 'Reparado', 'Entregado', 'Sin Solucion')),
    CostoReparacion DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (CostoReparacion >= 0),
    FechaIngreso DATETIME NOT NULL DEFAULT GETDATE(),
    FechaFinalizacion DATETIME NULL,
    FechaEntrega DATETIME NULL,
    Observaciones VARCHAR(500) NULL,
    CONSTRAINT FK_OrdenesReparacion_Garantias FOREIGN KEY (IdGarantia) REFERENCES ReclamosGarantia(IdGarantia),
    CONSTRAINT FK_OrdenesReparacion_Clientes FOREIGN KEY (IdCliente) REFERENCES Clientes(IdCliente),
    CONSTRAINT FK_OrdenesReparacion_Usuarios FOREIGN KEY (IdUsuarioTecnico) REFERENCES Usuarios(IdUsuario)
);
GO


-- STORED PROCEDUREs



   --1. ROLES
 

CREATE OR ALTER PROCEDURE InsertarRol
    @CodigoRol VARCHAR(10),
    @NombreRol VARCHAR(50),
    @Descripcion VARCHAR(255),
    @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO Roles (CodigoRol, NombreRol, Descripcion, Activo)
        VALUES (@CodigoRol, @NombreRol, @Descripcion, @Activo);

        SELECT SCOPE_IDENTITY() AS IdRol;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe un rol con ese código o nombre.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarRol
    @IdRol INT,
    @CodigoRol VARCHAR(10),
    @NombreRol VARCHAR(50),
    @Descripcion VARCHAR(255),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE Roles
        SET CodigoRol = @CodigoRol, NombreRol = @NombreRol, Descripcion = @Descripcion, Activo = @Activo
        WHERE IdRol = @IdRol;

        IF @@ROWCOUNT = 0 THROW 50003, 'El rol no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe un rol con ese código o nombre.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarRol
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM Roles WHERE IdRol = @IdRol;
        IF @@ROWCOUNT = 0 THROW 50003, 'El rol no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() = 547 THROW 50004, 'No se puede eliminar el rol: tiene usuarios asociados.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarRoles
    @IdRol INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdRol, CodigoRol, NombreRol, Descripcion, Activo, FechaCreacion
    FROM Roles
    WHERE @IdRol IS NULL OR IdRol = @IdRol
    ORDER BY IdRol;
END;
GO

 
   --2. USUARIOS
   --En Actualizar, si @PasswordHash es NULL se conserva la contraseña actual.
   --Listar no devuelve el hash.
  

CREATE OR ALTER PROCEDURE InsertarUsuario
    @IdRol INT,
    @Nombres VARCHAR(100),
    @Apellidos VARCHAR(100),
    @Correo VARCHAR(120),
    @PasswordHash VARCHAR(256),
    @Telefono VARCHAR(20) = NULL,
    @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO Usuarios (IdRol, Nombres, Apellidos, Correo, PasswordHash, Telefono, Activo)
        VALUES (@IdRol, @Nombres, @Apellidos, @Correo, @PasswordHash, @Telefono, @Activo);

        SELECT SCOPE_IDENTITY() AS IdUsuario;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe un usuario con ese correo.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarUsuario
    @IdUsuario INT,
    @IdRol INT,
    @Nombres VARCHAR(100),
    @Apellidos VARCHAR(100),
    @Correo VARCHAR(120),
    @Activo BIT,
    @PasswordHash VARCHAR(256) = NULL,
    @Telefono VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE Usuarios
        SET IdRol = @IdRol, Nombres = @Nombres, Apellidos = @Apellidos, Correo = @Correo,
            PasswordHash = ISNULL(@PasswordHash, PasswordHash), Telefono = @Telefono, Activo = @Activo
        WHERE IdUsuario = @IdUsuario;

        IF @@ROWCOUNT = 0 THROW 50003, 'El usuario no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe un usuario con ese correo.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarUsuario
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM Usuarios WHERE IdUsuario = @IdUsuario;
        IF @@ROWCOUNT = 0 THROW 50003, 'El usuario no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() = 547 THROW 50004, 'No se puede eliminar el usuario: tiene compras, ventas u otros registros asociados.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarUsuarios
    @IdUsuario INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdUsuario, IdRol, Nombres, Apellidos, Correo, Telefono, Activo, FechaCreacion
    FROM Usuarios
    WHERE @IdUsuario IS NULL OR IdUsuario = @IdUsuario
    ORDER BY IdUsuario;
END;
GO

--3. CLIENTES

CREATE OR ALTER PROCEDURE InsertarCliente
    @NumeroIdentificacion VARCHAR(25),
    @Nombres VARCHAR(100),
    @Apellidos VARCHAR(100),
    @Telefono VARCHAR(20) = NULL,
    @Correo VARCHAR(120) = NULL,
    @Direccion VARCHAR(250) = NULL,
    @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO Clientes (NumeroIdentificacion, Nombres, Apellidos, Telefono, Correo, Direccion, Activo)
        VALUES (@NumeroIdentificacion, @Nombres, @Apellidos, @Telefono, @Correo, @Direccion, @Activo);

        SELECT SCOPE_IDENTITY() AS IdCliente;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe un cliente con esa identificación.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarCliente
    @IdCliente INT,
    @NumeroIdentificacion VARCHAR(25),
    @Nombres VARCHAR(100),
    @Apellidos VARCHAR(100),
    @Activo BIT,
    @Telefono VARCHAR(20) = NULL,
    @Correo VARCHAR(120) = NULL,
    @Direccion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE Clientes
        SET NumeroIdentificacion = @NumeroIdentificacion, Nombres = @Nombres, Apellidos = @Apellidos,
            Telefono = @Telefono, Correo = @Correo, Direccion = @Direccion, Activo = @Activo
        WHERE IdCliente = @IdCliente;

        IF @@ROWCOUNT = 0 THROW 50003, 'El cliente no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe un cliente con esa identificación.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarCliente
    @IdCliente INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM Clientes WHERE IdCliente = @IdCliente;
        IF @@ROWCOUNT = 0 THROW 50003, 'El cliente no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() = 547 THROW 50004, 'No se puede eliminar el cliente: tiene ventas u órdenes de reparación asociadas.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarClientes
    @IdCliente INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdCliente, NumeroIdentificacion, Nombres, Apellidos, Telefono, Correo, Direccion, Activo, FechaRegistro
    FROM Clientes
    WHERE @IdCliente IS NULL OR IdCliente = @IdCliente
    ORDER BY IdCliente;
END;
GO

 --4. PROVEEDORES

CREATE OR ALTER PROCEDURE InsertarProveedor
    @NumeroRUC VARCHAR(25),
    @RazonSocial VARCHAR(150),
    @NombreContacto VARCHAR(100) = NULL,
    @Telefono VARCHAR(20) = NULL,
    @Correo VARCHAR(120) = NULL,
    @Direccion VARCHAR(250) = NULL,
    @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO Proveedores (NumeroRUC, RazonSocial, NombreContacto, Telefono, Correo, Direccion, Activo)
        VALUES (@NumeroRUC, @RazonSocial, @NombreContacto, @Telefono, @Correo, @Direccion, @Activo);

        SELECT SCOPE_IDENTITY() AS IdProveedor;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe un proveedor con ese RUC.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarProveedor
    @IdProveedor INT,
    @NumeroRUC VARCHAR(25),
    @RazonSocial VARCHAR(150),
    @Activo BIT,
    @NombreContacto VARCHAR(100) = NULL,
    @Telefono VARCHAR(20) = NULL,
    @Correo VARCHAR(120) = NULL,
    @Direccion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE Proveedores
        SET NumeroRUC = @NumeroRUC, RazonSocial = @RazonSocial, NombreContacto = @NombreContacto,
            Telefono = @Telefono, Correo = @Correo, Direccion = @Direccion, Activo = @Activo
        WHERE IdProveedor = @IdProveedor;

        IF @@ROWCOUNT = 0 THROW 50003, 'El proveedor no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe un proveedor con ese RUC.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarProveedor
    @IdProveedor INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM Proveedores WHERE IdProveedor = @IdProveedor;
        IF @@ROWCOUNT = 0 THROW 50003, 'El proveedor no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() = 547 THROW 50004, 'No se puede eliminar el proveedor: tiene compras o productos asociados.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarProveedores
    @IdProveedor INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdProveedor, NumeroRUC, RazonSocial, NombreContacto, Telefono, Correo, Direccion, Activo, FechaRegistro
    FROM Proveedores
    WHERE @IdProveedor IS NULL OR IdProveedor = @IdProveedor
    ORDER BY IdProveedor;
END;
GO


   --5. PRODUCTOS


CREATE OR ALTER PROCEDURE InsertarProducto
    @CodigoSKU VARCHAR(50),
    @Nombre VARCHAR(150),
    @Marca VARCHAR(80),
    @Modelo VARCHAR(80),
    @TipoEquipo VARCHAR(80),
    @PrecioVenta DECIMAL(18,2),
    @Descripcion VARCHAR(255) = NULL,
    @UltimoCostoCompra DECIMAL(18,2) = 0,
    @CostoPromedio DECIMAL(18,2) = 0,
    @StockActual INT = 0,
    @StockMinimo INT = 1,
    @MesesGarantia INT = 0,
    @RequiereSerie BIT = 0,
    @IdProveedorPreferido INT = NULL,
    @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO Productos (CodigoSKU, Nombre, Marca, Modelo, TipoEquipo, Descripcion, PrecioVenta,
                               UltimoCostoCompra, CostoPromedio, StockActual, StockMinimo, MesesGarantia,
                               RequiereSerie, IdProveedorPreferido, Activo)
        VALUES (@CodigoSKU, @Nombre, @Marca, @Modelo, @TipoEquipo, @Descripcion, @PrecioVenta,
                @UltimoCostoCompra, @CostoPromedio, @StockActual, @StockMinimo, @MesesGarantia,
                @RequiereSerie, @IdProveedorPreferido, @Activo);

        SELECT SCOPE_IDENTITY() AS IdProducto;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe un producto con ese SKU.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarProducto
    @IdProducto INT,
    @CodigoSKU VARCHAR(50),
    @Nombre VARCHAR(150),
    @Marca VARCHAR(80),
    @Modelo VARCHAR(80),
    @TipoEquipo VARCHAR(80),
    @PrecioVenta DECIMAL(18,2),
    @UltimoCostoCompra DECIMAL(18,2),
    @CostoPromedio DECIMAL(18,2),
    @StockActual INT,
    @StockMinimo INT,
    @MesesGarantia INT,
    @RequiereSerie BIT,
    @Activo BIT,
    @Descripcion VARCHAR(255) = NULL,
    @IdProveedorPreferido INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE Productos
        SET CodigoSKU = @CodigoSKU, Nombre = @Nombre, Marca = @Marca, Modelo = @Modelo,
            TipoEquipo = @TipoEquipo, Descripcion = @Descripcion, PrecioVenta = @PrecioVenta,
            UltimoCostoCompra = @UltimoCostoCompra, CostoPromedio = @CostoPromedio,
            StockActual = @StockActual, StockMinimo = @StockMinimo, MesesGarantia = @MesesGarantia,
            RequiereSerie = @RequiereSerie, IdProveedorPreferido = @IdProveedorPreferido, Activo = @Activo
        WHERE IdProducto = @IdProducto;

        IF @@ROWCOUNT = 0 THROW 50003, 'El producto no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe un producto con ese SKU.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarProducto
    @IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM Productos WHERE IdProducto = @IdProducto;
        IF @@ROWCOUNT = 0 THROW 50003, 'El producto no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() = 547 THROW 50004, 'No se puede eliminar el producto: tiene compras, ventas o movimientos de kardex asociados.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarProductos
    @IdProducto INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdProducto, CodigoSKU, Nombre, Marca, Modelo, TipoEquipo, Descripcion, PrecioVenta,
           UltimoCostoCompra, CostoPromedio, StockActual, StockMinimo, MesesGarantia,
           RequiereSerie, IdProveedorPreferido, Activo, FechaRegistro
    FROM Productos
    WHERE @IdProducto IS NULL OR IdProducto = @IdProducto
    ORDER BY IdProducto;
END;
GO


   --6. COMPRAS


CREATE OR ALTER PROCEDURE InsertarCompra
    @NumeroDocumento VARCHAR(50),
    @IdProveedor INT,
    @IdUsuarioRegistro INT,
    @FechaCompra DATETIME = NULL,
    @IdUsuarioRecepcion INT = NULL,
    @FechaRecepcion DATETIME = NULL,
    @TotalCompra DECIMAL(18,2) = 0,
    @Estado VARCHAR(20) = 'Pendiente',
    @Observaciones VARCHAR(255) = NULL,
    @IdUsuarioAnulacion INT = NULL,
    @FechaAnulacion DATETIME = NULL,
    @MotivoAnulacion VARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO Compras (NumeroDocumento, IdProveedor, IdUsuarioRegistro, IdUsuarioRecepcion, FechaCompra,
                             FechaRecepcion, TotalCompra, Estado, Observaciones, IdUsuarioAnulacion,
                             FechaAnulacion, MotivoAnulacion)
        VALUES (@NumeroDocumento, @IdProveedor, @IdUsuarioRegistro, @IdUsuarioRecepcion,
                ISNULL(@FechaCompra, GETDATE()), @FechaRecepcion, @TotalCompra, @Estado, @Observaciones,
                @IdUsuarioAnulacion, @FechaAnulacion, @MotivoAnulacion);

        SELECT SCOPE_IDENTITY() AS IdCompra;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarCompra
    @IdCompra INT,
    @NumeroDocumento VARCHAR(50),
    @IdProveedor INT,
    @IdUsuarioRegistro INT,
    @FechaCompra DATETIME,
    @TotalCompra DECIMAL(18,2),
    @Estado VARCHAR(20),
    @IdUsuarioRecepcion INT = NULL,
    @FechaRecepcion DATETIME = NULL,
    @Observaciones VARCHAR(255) = NULL,
    @IdUsuarioAnulacion INT = NULL,
    @FechaAnulacion DATETIME = NULL,
    @MotivoAnulacion VARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE Compras
        SET NumeroDocumento = @NumeroDocumento, IdProveedor = @IdProveedor, IdUsuarioRegistro = @IdUsuarioRegistro,
            IdUsuarioRecepcion = @IdUsuarioRecepcion, FechaCompra = @FechaCompra, FechaRecepcion = @FechaRecepcion,
            TotalCompra = @TotalCompra, Estado = @Estado, Observaciones = @Observaciones,
            IdUsuarioAnulacion = @IdUsuarioAnulacion, FechaAnulacion = @FechaAnulacion,
            MotivoAnulacion = @MotivoAnulacion
        WHERE IdCompra = @IdCompra;

        IF @@ROWCOUNT = 0 THROW 50003, 'La compra no existe.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarCompra
    @IdCompra INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM Compras WHERE IdCompra = @IdCompra;
        IF @@ROWCOUNT = 0 THROW 50003, 'La compra no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() = 547 THROW 50004, 'No se puede eliminar la compra: tiene detalle asociado. Elimine primero su detalle.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarCompras
    @IdCompra INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdCompra, NumeroDocumento, IdProveedor, IdUsuarioRegistro, IdUsuarioRecepcion, FechaCompra,
           FechaRecepcion, TotalCompra, Estado, Observaciones, IdUsuarioAnulacion, FechaAnulacion, MotivoAnulacion
    FROM Compras
    WHERE @IdCompra IS NULL OR IdCompra = @IdCompra
    ORDER BY IdCompra;
END;
GO


   --7. DETALLE DE COMPRAS  (Subtotal es columna calculada)


CREATE OR ALTER PROCEDURE InsertarDetalleCompra
    @IdCompra INT,
    @IdProducto INT,
    @Cantidad INT,
    @CostoUnitario DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO DetalleCompras (IdCompra, IdProducto, Cantidad, CostoUnitario)
        VALUES (@IdCompra, @IdProducto, @Cantidad, @CostoUnitario);

        SELECT SCOPE_IDENTITY() AS IdDetalleCompra;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarDetalleCompra
    @IdDetalleCompra INT,
    @IdCompra INT,
    @IdProducto INT,
    @Cantidad INT,
    @CostoUnitario DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE DetalleCompras
        SET IdCompra = @IdCompra, IdProducto = @IdProducto, Cantidad = @Cantidad, CostoUnitario = @CostoUnitario
        WHERE IdDetalleCompra = @IdDetalleCompra;

        IF @@ROWCOUNT = 0 THROW 50003, 'El detalle de compra no existe.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarDetalleCompra
    @IdDetalleCompra INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM DetalleCompras WHERE IdDetalleCompra = @IdDetalleCompra;
        IF @@ROWCOUNT = 0 THROW 50003, 'El detalle de compra no existe.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarDetalleCompras
    @IdDetalleCompra INT = NULL,
    @IdCompra INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdDetalleCompra, IdCompra, IdProducto, Cantidad, CostoUnitario, Subtotal
    FROM DetalleCompras
    WHERE (@IdDetalleCompra IS NULL OR IdDetalleCompra = @IdDetalleCompra)
      AND (@IdCompra IS NULL OR IdCompra = @IdCompra)
    ORDER BY IdDetalleCompra;
END;
GO


   --8. VENTAS
 

CREATE OR ALTER PROCEDURE InsertarVenta
    @NumeroFactura VARCHAR(30),
    @IdCliente INT,
    @IdUsuarioVendedor INT,
    @TipoVenta VARCHAR(20),
    @MetodoPago VARCHAR(30),
    @FechaVenta DATETIME = NULL,
    @PorcentajeImpuesto DECIMAL(5,2) = 15.00,
    @Subtotal DECIMAL(18,2) = 0,
    @Impuesto DECIMAL(18,2) = 0,
    @TotalVenta DECIMAL(18,2) = 0,
    @MontoRecibido DECIMAL(18,2) = 0,
    @Vuelto DECIMAL(18,2) = 0,
    @Estado VARCHAR(20) = 'Pendiente',
    @IdUsuarioAutorizaAnulacion INT = NULL,
    @FechaAnulacion DATETIME = NULL,
    @MotivoAnulacion VARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO Ventas (NumeroFactura, IdCliente, IdUsuarioVendedor, FechaVenta, TipoVenta, MetodoPago,
                            PorcentajeImpuesto, Subtotal, Impuesto, TotalVenta, MontoRecibido, Vuelto, Estado,
                            IdUsuarioAutorizaAnulacion, FechaAnulacion, MotivoAnulacion)
        VALUES (@NumeroFactura, @IdCliente, @IdUsuarioVendedor, ISNULL(@FechaVenta, GETDATE()), @TipoVenta,
                @MetodoPago, @PorcentajeImpuesto, @Subtotal, @Impuesto, @TotalVenta, @MontoRecibido, @Vuelto,
                @Estado, @IdUsuarioAutorizaAnulacion, @FechaAnulacion, @MotivoAnulacion);

        SELECT SCOPE_IDENTITY() AS IdVenta;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe una venta con ese número de factura.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarVenta
    @IdVenta INT,
    @NumeroFactura VARCHAR(30),
    @IdCliente INT,
    @IdUsuarioVendedor INT,
    @FechaVenta DATETIME,
    @TipoVenta VARCHAR(20),
    @MetodoPago VARCHAR(30),
    @PorcentajeImpuesto DECIMAL(5,2),
    @Subtotal DECIMAL(18,2),
    @Impuesto DECIMAL(18,2),
    @TotalVenta DECIMAL(18,2),
    @MontoRecibido DECIMAL(18,2),
    @Vuelto DECIMAL(18,2),
    @Estado VARCHAR(20),
    @IdUsuarioAutorizaAnulacion INT = NULL,
    @FechaAnulacion DATETIME = NULL,
    @MotivoAnulacion VARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE Ventas
        SET NumeroFactura = @NumeroFactura, IdCliente = @IdCliente, IdUsuarioVendedor = @IdUsuarioVendedor,
            FechaVenta = @FechaVenta, TipoVenta = @TipoVenta, MetodoPago = @MetodoPago,
            PorcentajeImpuesto = @PorcentajeImpuesto, Subtotal = @Subtotal, Impuesto = @Impuesto,
            TotalVenta = @TotalVenta, MontoRecibido = @MontoRecibido, Vuelto = @Vuelto, Estado = @Estado,
            IdUsuarioAutorizaAnulacion = @IdUsuarioAutorizaAnulacion, FechaAnulacion = @FechaAnulacion,
            MotivoAnulacion = @MotivoAnulacion
        WHERE IdVenta = @IdVenta;

        IF @@ROWCOUNT = 0 THROW 50003, 'La venta no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe una venta con ese número de factura.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarVenta
    @IdVenta INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM Ventas WHERE IdVenta = @IdVenta;
        IF @@ROWCOUNT = 0 THROW 50003, 'La venta no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() = 547 THROW 50004, 'No se puede eliminar la venta: tiene detalle o crédito asociado.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarVentas
    @IdVenta INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdVenta, NumeroFactura, IdCliente, IdUsuarioVendedor, FechaVenta, TipoVenta, MetodoPago,
           PorcentajeImpuesto, Subtotal, Impuesto, TotalVenta, MontoRecibido, Vuelto, Estado,
           IdUsuarioAutorizaAnulacion, FechaAnulacion, MotivoAnulacion
    FROM Ventas
    WHERE @IdVenta IS NULL OR IdVenta = @IdVenta
    ORDER BY IdVenta;
END;
GO


   --9. DETALLE DE VENTAS  (Subtotal es columna calculada)
  

CREATE OR ALTER PROCEDURE InsertarDetalleVenta
    @IdVenta INT,
    @IdProducto INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(18,2),
    @CostoUnitarioHistorico DECIMAL(18,2) = 0,
    @MesesGarantiaHistorico INT = 0,
    @FechaFinGarantia DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO DetalleVentas (IdVenta, IdProducto, Cantidad, PrecioUnitario, CostoUnitarioHistorico,
                                   MesesGarantiaHistorico, FechaFinGarantia)
        VALUES (@IdVenta, @IdProducto, @Cantidad, @PrecioUnitario, @CostoUnitarioHistorico,
                @MesesGarantiaHistorico, @FechaFinGarantia);

        SELECT SCOPE_IDENTITY() AS IdDetalleVenta;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarDetalleVenta
    @IdDetalleVenta INT,
    @IdVenta INT,
    @IdProducto INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(18,2),
    @CostoUnitarioHistorico DECIMAL(18,2),
    @MesesGarantiaHistorico INT,
    @FechaFinGarantia DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE DetalleVentas
        SET IdVenta = @IdVenta, IdProducto = @IdProducto, Cantidad = @Cantidad, PrecioUnitario = @PrecioUnitario,
            CostoUnitarioHistorico = @CostoUnitarioHistorico, MesesGarantiaHistorico = @MesesGarantiaHistorico,
            FechaFinGarantia = @FechaFinGarantia
        WHERE IdDetalleVenta = @IdDetalleVenta;

        IF @@ROWCOUNT = 0 THROW 50003, 'El detalle de venta no existe.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarDetalleVenta
    @IdDetalleVenta INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM DetalleVentas WHERE IdDetalleVenta = @IdDetalleVenta;
        IF @@ROWCOUNT = 0 THROW 50003, 'El detalle de venta no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() = 547 THROW 50004, 'No se puede eliminar el detalle: tiene series o reclamos de garantía asociados.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarDetalleVentas
    @IdDetalleVenta INT = NULL,
    @IdVenta INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdDetalleVenta, IdVenta, IdProducto, Cantidad, PrecioUnitario, CostoUnitarioHistorico,
           Subtotal, MesesGarantiaHistorico, FechaFinGarantia
    FROM DetalleVentas
    WHERE (@IdDetalleVenta IS NULL OR IdDetalleVenta = @IdDetalleVenta)
      AND (@IdVenta IS NULL OR IdVenta = @IdVenta)
    ORDER BY IdDetalleVenta;
END;
GO


   --10. DETALLE DE VENTA - SERIES
 

CREATE OR ALTER PROCEDURE InsertarDetalleVentaSerie
    @IdDetalleVenta INT,
    @NumeroSerie VARCHAR(100),
    @EstadoSerie VARCHAR(30) = 'Vendido'
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO DetalleVentaSeries (IdDetalleVenta, NumeroSerie, EstadoSerie)
        VALUES (@IdDetalleVenta, @NumeroSerie, @EstadoSerie);

        SELECT SCOPE_IDENTITY() AS IdSerieVendida;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe ese número de serie.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarDetalleVentaSerie
    @IdSerieVendida INT,
    @IdDetalleVenta INT,
    @NumeroSerie VARCHAR(100),
    @EstadoSerie VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE DetalleVentaSeries
        SET IdDetalleVenta = @IdDetalleVenta, NumeroSerie = @NumeroSerie, EstadoSerie = @EstadoSerie
        WHERE IdSerieVendida = @IdSerieVendida;

        IF @@ROWCOUNT = 0 THROW 50003, 'La serie no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe ese número de serie.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarDetalleVentaSerie
    @IdSerieVendida INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM DetalleVentaSeries WHERE IdSerieVendida = @IdSerieVendida;
        IF @@ROWCOUNT = 0 THROW 50003, 'La serie no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() = 547 THROW 50004, 'No se puede eliminar la serie: tiene reclamos de garantía asociados.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarDetalleVentaSeries
    @IdSerieVendida INT = NULL,
    @IdDetalleVenta INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdSerieVendida, IdDetalleVenta, NumeroSerie, EstadoSerie, FechaRegistro
    FROM DetalleVentaSeries
    WHERE (@IdSerieVendida IS NULL OR IdSerieVendida = @IdSerieVendida)
      AND (@IdDetalleVenta IS NULL OR IdDetalleVenta = @IdDetalleVenta)
    ORDER BY IdSerieVendida;
END;
GO


   --11. CRÉDITOS


CREATE OR ALTER PROCEDURE InsertarCredito
    @IdVenta INT,
    @MontoVentaTotal DECIMAL(18,2),
    @MontoFinanciado DECIMAL(18,2),
    @FrecuenciaPago VARCHAR(20),
    @CantidadCuotas INT,
    @MontoTotalCredito DECIMAL(18,2),
    @SaldoPendiente DECIMAL(18,2),
    @PrimaInicial DECIMAL(18,2) = 0,
    @PorcentajeInteres DECIMAL(5,2) = 0,
    @MontoInteresesPactados DECIMAL(18,2) = 0,
    @Estado VARCHAR(20) = 'Activo'
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO Creditos (IdVenta, MontoVentaTotal, PrimaInicial, MontoFinanciado, FrecuenciaPago,
                              CantidadCuotas, PorcentajeInteres, MontoInteresesPactados, MontoTotalCredito,
                              SaldoPendiente, Estado)
        VALUES (@IdVenta, @MontoVentaTotal, @PrimaInicial, @MontoFinanciado, @FrecuenciaPago,
                @CantidadCuotas, @PorcentajeInteres, @MontoInteresesPactados, @MontoTotalCredito,
                @SaldoPendiente, @Estado);

        SELECT SCOPE_IDENTITY() AS IdCredito;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'La venta ya tiene un crédito.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarCredito
    @IdCredito INT,
    @IdVenta INT,
    @MontoVentaTotal DECIMAL(18,2),
    @PrimaInicial DECIMAL(18,2),
    @MontoFinanciado DECIMAL(18,2),
    @FrecuenciaPago VARCHAR(20),
    @CantidadCuotas INT,
    @PorcentajeInteres DECIMAL(5,2),
    @MontoInteresesPactados DECIMAL(18,2),
    @MontoTotalCredito DECIMAL(18,2),
    @SaldoPendiente DECIMAL(18,2),
    @Estado VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE Creditos
        SET IdVenta = @IdVenta, MontoVentaTotal = @MontoVentaTotal, PrimaInicial = @PrimaInicial,
            MontoFinanciado = @MontoFinanciado, FrecuenciaPago = @FrecuenciaPago, CantidadCuotas = @CantidadCuotas,
            PorcentajeInteres = @PorcentajeInteres, MontoInteresesPactados = @MontoInteresesPactados,
            MontoTotalCredito = @MontoTotalCredito, SaldoPendiente = @SaldoPendiente, Estado = @Estado
        WHERE IdCredito = @IdCredito;

        IF @@ROWCOUNT = 0 THROW 50003, 'El crédito no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'La venta ya tiene un crédito.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarCredito
    @IdCredito INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM Creditos WHERE IdCredito = @IdCredito;
        IF @@ROWCOUNT = 0 THROW 50003, 'El crédito no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() = 547 THROW 50004, 'No se puede eliminar el crédito: tiene cuotas asociadas.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarCreditos
    @IdCredito INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdCredito, IdVenta, MontoVentaTotal, PrimaInicial, MontoFinanciado, FrecuenciaPago, CantidadCuotas,
           PorcentajeInteres, MontoInteresesPactados, MontoTotalCredito, SaldoPendiente, Estado, FechaRegistro
    FROM Creditos
    WHERE @IdCredito IS NULL OR IdCredito = @IdCredito
    ORDER BY IdCredito;
END;
GO


   --12. CUOTAS DE CRÉDITO  


CREATE OR ALTER PROCEDURE InsertarCuotaCredito
    @IdCredito INT,
    @NumeroCuota INT,
    @FechaVencimiento DATE,
    @MontoCuota DECIMAL(18,2),
    @RecargoMora DECIMAL(18,2) = 0,
    @MoraAbonada DECIMAL(18,2) = 0,
    @CapitalInteresAbonado DECIMAL(18,2) = 0,
    @Estado VARCHAR(20) = 'Pendiente'
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO CuotasCredito (IdCredito, NumeroCuota, FechaVencimiento, MontoCuota, RecargoMora,
                                   MoraAbonada, CapitalInteresAbonado, Estado)
        VALUES (@IdCredito, @NumeroCuota, @FechaVencimiento, @MontoCuota, @RecargoMora,
                @MoraAbonada, @CapitalInteresAbonado, @Estado);

        SELECT SCOPE_IDENTITY() AS IdCuota;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe esa cuota para el crédito.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarCuotaCredito
    @IdCuota INT,
    @IdCredito INT,
    @NumeroCuota INT,
    @FechaVencimiento DATE,
    @MontoCuota DECIMAL(18,2),
    @RecargoMora DECIMAL(18,2),
    @MoraAbonada DECIMAL(18,2),
    @CapitalInteresAbonado DECIMAL(18,2),
    @Estado VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE CuotasCredito
        SET IdCredito = @IdCredito, NumeroCuota = @NumeroCuota, FechaVencimiento = @FechaVencimiento,
            MontoCuota = @MontoCuota, RecargoMora = @RecargoMora, MoraAbonada = @MoraAbonada,
            CapitalInteresAbonado = @CapitalInteresAbonado, Estado = @Estado
        WHERE IdCuota = @IdCuota;

        IF @@ROWCOUNT = 0 THROW 50003, 'La cuota no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe esa cuota para el crédito.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarCuotaCredito
    @IdCuota INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM CuotasCredito WHERE IdCuota = @IdCuota;
        IF @@ROWCOUNT = 0 THROW 50003, 'La cuota no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() = 547 THROW 50004, 'No se puede eliminar la cuota: tiene abonos asociados.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarCuotasCredito
    @IdCuota INT = NULL,
    @IdCredito INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdCuota, IdCredito, NumeroCuota, FechaVencimiento, MontoCuota, RecargoMora, MoraAbonada,
           CapitalInteresAbonado, MontoAbonado, SaldoCuota, Estado
    FROM CuotasCredito
    WHERE (@IdCuota IS NULL OR IdCuota = @IdCuota)
      AND (@IdCredito IS NULL OR IdCredito = @IdCredito)
    ORDER BY IdCredito, NumeroCuota;
END;
GO


   --13. ABONOS
  

CREATE OR ALTER PROCEDURE InsertarAbono
    @NumeroRecibo VARCHAR(30),
    @IdCuota INT,
    @IdUsuarioRegistro INT,
    @MontoAbono DECIMAL(18,2),
    @MetodoPago VARCHAR(30),
    @FechaAbono DATETIME = NULL,
    @MontoAplicadoMora DECIMAL(18,2) = 0,
    @MontoAplicadoCapital DECIMAL(18,2) = 0,
    @Observaciones VARCHAR(255) = NULL,
    @Estado VARCHAR(20) = 'Activo',
    @IdUsuarioAnulacion INT = NULL,
    @FechaAnulacion DATETIME = NULL,
    @MotivoAnulacion VARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO Abonos (NumeroRecibo, IdCuota, IdUsuarioRegistro, FechaAbono, MontoAbono, MontoAplicadoMora,
                            MontoAplicadoCapital, MetodoPago, Observaciones, Estado, IdUsuarioAnulacion,
                            FechaAnulacion, MotivoAnulacion)
        VALUES (@NumeroRecibo, @IdCuota, @IdUsuarioRegistro, ISNULL(@FechaAbono, GETDATE()), @MontoAbono,
                @MontoAplicadoMora, @MontoAplicadoCapital, @MetodoPago, @Observaciones, @Estado,
                @IdUsuarioAnulacion, @FechaAnulacion, @MotivoAnulacion);

        SELECT SCOPE_IDENTITY() AS IdAbono;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe un abono con ese número de recibo.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarAbono
    @IdAbono INT,
    @NumeroRecibo VARCHAR(30),
    @IdCuota INT,
    @IdUsuarioRegistro INT,
    @FechaAbono DATETIME,
    @MontoAbono DECIMAL(18,2),
    @MontoAplicadoMora DECIMAL(18,2),
    @MontoAplicadoCapital DECIMAL(18,2),
    @MetodoPago VARCHAR(30),
    @Estado VARCHAR(20),
    @Observaciones VARCHAR(255) = NULL,
    @IdUsuarioAnulacion INT = NULL,
    @FechaAnulacion DATETIME = NULL,
    @MotivoAnulacion VARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE Abonos
        SET NumeroRecibo = @NumeroRecibo, IdCuota = @IdCuota, IdUsuarioRegistro = @IdUsuarioRegistro,
            FechaAbono = @FechaAbono, MontoAbono = @MontoAbono, MontoAplicadoMora = @MontoAplicadoMora,
            MontoAplicadoCapital = @MontoAplicadoCapital, MetodoPago = @MetodoPago, Observaciones = @Observaciones,
            Estado = @Estado, IdUsuarioAnulacion = @IdUsuarioAnulacion, FechaAnulacion = @FechaAnulacion,
            MotivoAnulacion = @MotivoAnulacion
        WHERE IdAbono = @IdAbono;

        IF @@ROWCOUNT = 0 THROW 50003, 'El abono no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe un abono con ese número de recibo.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarAbono
    @IdAbono INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM Abonos WHERE IdAbono = @IdAbono;
        IF @@ROWCOUNT = 0 THROW 50003, 'El abono no existe.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarAbonos
    @IdAbono INT = NULL,
    @IdCuota INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdAbono, NumeroRecibo, IdCuota, IdUsuarioRegistro, FechaAbono, MontoAbono, MontoAplicadoMora,
           MontoAplicadoCapital, MetodoPago, Observaciones, Estado, IdUsuarioAnulacion, FechaAnulacion,
           MotivoAnulacion
    FROM Abonos
    WHERE (@IdAbono IS NULL OR IdAbono = @IdAbono)
      AND (@IdCuota IS NULL OR IdCuota = @IdCuota)
    ORDER BY IdAbono;
END;
GO


   --14. KARDEX


CREATE OR ALTER PROCEDURE InsertarKardex
    @IdProducto INT,
    @TipoMovimiento VARCHAR(40),
    @Cantidad INT,
    @StockAnterior INT,
    @StockNuevo INT,
    @IdUsuarioResponsable INT,
    @FechaMovimiento DATETIME = NULL,
    @CostoUnitario DECIMAL(18,2) = 0,
    @DocumentoOrigen VARCHAR(50) = NULL,
    @IdReferenciaOrigen INT = NULL,
    @Justificacion VARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO Kardex (IdProducto, FechaMovimiento, TipoMovimiento, Cantidad, StockAnterior, StockNuevo,
                            CostoUnitario, DocumentoOrigen, IdReferenciaOrigen, Justificacion, IdUsuarioResponsable)
        VALUES (@IdProducto, ISNULL(@FechaMovimiento, GETDATE()), @TipoMovimiento, @Cantidad, @StockAnterior,
                @StockNuevo, @CostoUnitario, @DocumentoOrigen, @IdReferenciaOrigen, @Justificacion,
                @IdUsuarioResponsable);

        SELECT SCOPE_IDENTITY() AS IdMovimiento;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarKardex
    @IdMovimiento INT,
    @IdProducto INT,
    @FechaMovimiento DATETIME,
    @TipoMovimiento VARCHAR(40),
    @Cantidad INT,
    @StockAnterior INT,
    @StockNuevo INT,
    @CostoUnitario DECIMAL(18,2),
    @IdUsuarioResponsable INT,
    @DocumentoOrigen VARCHAR(50) = NULL,
    @IdReferenciaOrigen INT = NULL,
    @Justificacion VARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE Kardex
        SET IdProducto = @IdProducto, FechaMovimiento = @FechaMovimiento, TipoMovimiento = @TipoMovimiento,
            Cantidad = @Cantidad, StockAnterior = @StockAnterior, StockNuevo = @StockNuevo,
            CostoUnitario = @CostoUnitario, DocumentoOrigen = @DocumentoOrigen,
            IdReferenciaOrigen = @IdReferenciaOrigen, Justificacion = @Justificacion,
            IdUsuarioResponsable = @IdUsuarioResponsable
        WHERE IdMovimiento = @IdMovimiento;

        IF @@ROWCOUNT = 0 THROW 50003, 'El movimiento de kardex no existe.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarKardex
    @IdMovimiento INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM Kardex WHERE IdMovimiento = @IdMovimiento;
        IF @@ROWCOUNT = 0 THROW 50003, 'El movimiento de kardex no existe.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarKardex
    @IdMovimiento INT = NULL,
    @IdProducto INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdMovimiento, IdProducto, FechaMovimiento, TipoMovimiento, Cantidad, StockAnterior, StockNuevo,
           CostoUnitario, DocumentoOrigen, IdReferenciaOrigen, Justificacion, IdUsuarioResponsable
    FROM Kardex
    WHERE (@IdMovimiento IS NULL OR IdMovimiento = @IdMovimiento)
      AND (@IdProducto IS NULL OR IdProducto = @IdProducto)
    ORDER BY FechaMovimiento, IdMovimiento;
END;
GO


   --15. RECLAMOS DE GARANTÍA


CREATE OR ALTER PROCEDURE InsertarReclamoGarantia
    @NumeroReclamo VARCHAR(30),
    @IdDetalleVenta INT,
    @IdUsuarioTecnico INT,
    @MotivoFalla VARCHAR(500),
    @IdSerieVendida INT = NULL,
    @FechaReclamo DATETIME = NULL,
    @EstadoRevision VARCHAR(30) = 'Recibido',
    @AccionTomada VARCHAR(40) = NULL,
    @DescripcionResolucion VARCHAR(500) = NULL,
    @FechaResolucion DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO ReclamosGarantia (NumeroReclamo, IdDetalleVenta, IdSerieVendida, IdUsuarioTecnico, FechaReclamo,
                                      MotivoFalla, EstadoRevision, AccionTomada, DescripcionResolucion, FechaResolucion)
        VALUES (@NumeroReclamo, @IdDetalleVenta, @IdSerieVendida, @IdUsuarioTecnico, ISNULL(@FechaReclamo, GETDATE()),
                @MotivoFalla, @EstadoRevision, @AccionTomada, @DescripcionResolucion, @FechaResolucion);

        SELECT SCOPE_IDENTITY() AS IdGarantia;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe un reclamo con ese número.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarReclamoGarantia
    @IdGarantia INT,
    @NumeroReclamo VARCHAR(30),
    @IdDetalleVenta INT,
    @IdUsuarioTecnico INT,
    @FechaReclamo DATETIME,
    @MotivoFalla VARCHAR(500),
    @EstadoRevision VARCHAR(30),
    @IdSerieVendida INT = NULL,
    @AccionTomada VARCHAR(40) = NULL,
    @DescripcionResolucion VARCHAR(500) = NULL,
    @FechaResolucion DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE ReclamosGarantia
        SET NumeroReclamo = @NumeroReclamo, IdDetalleVenta = @IdDetalleVenta, IdSerieVendida = @IdSerieVendida,
            IdUsuarioTecnico = @IdUsuarioTecnico, FechaReclamo = @FechaReclamo, MotivoFalla = @MotivoFalla,
            EstadoRevision = @EstadoRevision, AccionTomada = @AccionTomada,
            DescripcionResolucion = @DescripcionResolucion, FechaResolucion = @FechaResolucion
        WHERE IdGarantia = @IdGarantia;

        IF @@ROWCOUNT = 0 THROW 50003, 'El reclamo de garantía no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe un reclamo con ese número.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarReclamoGarantia
    @IdGarantia INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM ReclamosGarantia WHERE IdGarantia = @IdGarantia;
        IF @@ROWCOUNT = 0 THROW 50003, 'El reclamo de garantía no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() = 547 THROW 50004, 'No se puede eliminar el reclamo: tiene órdenes de reparación asociadas.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarReclamosGarantia
    @IdGarantia INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdGarantia, NumeroReclamo, IdDetalleVenta, IdSerieVendida, IdUsuarioTecnico, FechaReclamo,
           MotivoFalla, EstadoRevision, AccionTomada, DescripcionResolucion, FechaResolucion
    FROM ReclamosGarantia
    WHERE @IdGarantia IS NULL OR IdGarantia = @IdGarantia
    ORDER BY IdGarantia;
END;
GO


   --16. ÓRDENES DE REPARACIÓN


CREATE OR ALTER PROCEDURE InsertarOrdenReparacion
    @NumeroOrden VARCHAR(30),
    @IdCliente INT,
    @IdUsuarioTecnico INT,
    @DescripcionEquipo VARCHAR(150),
    @Marca VARCHAR(80),
    @Modelo VARCHAR(80),
    @NumeroSerie VARCHAR(100),
    @IdGarantia INT = NULL,
    @DiagnosticoTecnico VARCHAR(500) = NULL,
    @EstadoEquipo VARCHAR(30) = 'Recibido',
    @CostoReparacion DECIMAL(18,2) = 0,
    @FechaIngreso DATETIME = NULL,
    @FechaFinalizacion DATETIME = NULL,
    @FechaEntrega DATETIME = NULL,
    @Observaciones VARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO OrdenesReparacion (NumeroOrden, IdGarantia, IdCliente, IdUsuarioTecnico, DescripcionEquipo,
                                       Marca, Modelo, NumeroSerie, DiagnosticoTecnico, EstadoEquipo,
                                       CostoReparacion, FechaIngreso, FechaFinalizacion, FechaEntrega, Observaciones)
        VALUES (@NumeroOrden, @IdGarantia, @IdCliente, @IdUsuarioTecnico, @DescripcionEquipo, @Marca, @Modelo,
                @NumeroSerie, @DiagnosticoTecnico, @EstadoEquipo, @CostoReparacion,
                ISNULL(@FechaIngreso, GETDATE()), @FechaFinalizacion, @FechaEntrega, @Observaciones);

        SELECT SCOPE_IDENTITY() AS IdOrdenReparacion;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe una orden con ese número.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ActualizarOrdenReparacion
    @IdOrdenReparacion INT,
    @NumeroOrden VARCHAR(30),
    @IdCliente INT,
    @IdUsuarioTecnico INT,
    @DescripcionEquipo VARCHAR(150),
    @Marca VARCHAR(80),
    @Modelo VARCHAR(80),
    @NumeroSerie VARCHAR(100),
    @EstadoEquipo VARCHAR(30),
    @CostoReparacion DECIMAL(18,2),
    @FechaIngreso DATETIME,
    @IdGarantia INT = NULL,
    @DiagnosticoTecnico VARCHAR(500) = NULL,
    @FechaFinalizacion DATETIME = NULL,
    @FechaEntrega DATETIME = NULL,
    @Observaciones VARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE OrdenesReparacion
        SET NumeroOrden = @NumeroOrden, IdGarantia = @IdGarantia, IdCliente = @IdCliente,
            IdUsuarioTecnico = @IdUsuarioTecnico, DescripcionEquipo = @DescripcionEquipo, Marca = @Marca,
            Modelo = @Modelo, NumeroSerie = @NumeroSerie, DiagnosticoTecnico = @DiagnosticoTecnico,
            EstadoEquipo = @EstadoEquipo, CostoReparacion = @CostoReparacion, FechaIngreso = @FechaIngreso,
            FechaFinalizacion = @FechaFinalizacion, FechaEntrega = @FechaEntrega, Observaciones = @Observaciones
        WHERE IdOrdenReparacion = @IdOrdenReparacion;

        IF @@ROWCOUNT = 0 THROW 50003, 'La orden de reparación no existe.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601, 2627) THROW 50002, 'Ya existe una orden con ese número.', 1;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE EliminarOrdenReparacion
    @IdOrdenReparacion INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM OrdenesReparacion WHERE IdOrdenReparacion = @IdOrdenReparacion;
        IF @@ROWCOUNT = 0 THROW 50003, 'La orden de reparación no existe.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE ListarOrdenesReparacion
    @IdOrdenReparacion INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdOrdenReparacion, NumeroOrden, IdGarantia, IdCliente, IdUsuarioTecnico, DescripcionEquipo, Marca,
           Modelo, NumeroSerie, DiagnosticoTecnico, EstadoEquipo, CostoReparacion, FechaIngreso,
           FechaFinalizacion, FechaEntrega, Observaciones
    FROM OrdenesReparacion
    WHERE @IdOrdenReparacion IS NULL OR IdOrdenReparacion = @IdOrdenReparacion
    ORDER BY IdOrdenReparacion;
END;
GO


-- =========================================================
-- DATOS SEMILLA INICIALES (ROLES Y ADMINISTRADOR)
-- =========================================================
IF NOT EXISTS (SELECT 1 FROM Roles WHERE CodigoRol = 'Rol-01')
BEGIN
    INSERT INTO Roles (CodigoRol, NombreRol, Descripcion, Activo)
    VALUES ('Rol-01', 'Administrador', 'Acceso total al sistema, configuracion y anulaciones', 1);
END
GO

IF NOT EXISTS (SELECT 1 FROM Roles WHERE CodigoRol = 'Rol-02')
BEGIN
    INSERT INTO Roles (CodigoRol, NombreRol, Descripcion, Activo)
    VALUES ('Rol-02', 'Vendedor', 'Gestion de clientes, ventas POS y consultas de catalogo', 1);
END
GO

IF NOT EXISTS (SELECT 1 FROM Roles WHERE CodigoRol = 'Rol-03')
BEGIN
    INSERT INTO Roles (CodigoRol, NombreRol, Descripcion, Activo)
    VALUES ('Rol-03', 'Tecnico', 'Gestion de ordenes de reparacion y garantias', 1);
END
GO

-- Usuario Administrador por defecto: admin@codestore.com / Admin123*
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Correo = 'admin@codestore.com')
BEGIN
    DECLARE @IdRolAdmin INT = (SELECT TOP 1 IdRol FROM Roles WHERE CodigoRol = 'Rol-01');
    INSERT INTO Usuarios (IdRol, Nombres, Apellidos, Correo, PasswordHash, Telefono, Activo)
    VALUES (
        @IdRolAdmin,
        'Administrador',
        'CodeStore',
        'admin@codestore.com',
        '0A5BC3E342432F1BAD92FFD51B785343EC72906CDBA6A26131060B008E786656',
        '0000-0000',
        1
    );
END
GO
