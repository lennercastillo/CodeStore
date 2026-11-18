USE master;
GO

IF DB_ID('CodeStoreDB') IS NOT NULL
BEGIN
    ALTER DATABASE CodeStoreDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE CodeStoreDB;
END
GO

CREATE DATABASE CodeStoreDB;
GO

USE CodeStoreDB;
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
    StockActual INT NOT NULL DEFAULT 0 CHECK (StockActual >= 0),
    StockMinimo INT NOT NULL DEFAULT 1 CHECK (StockMinimo >= 0),
    MesesGarantia INT NOT NULL DEFAULT 0 CHECK (MesesGarantia >= 0),
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
    Subtotal AS (Cantidad * PrecioUnitario) PERSISTED,
    MesesGarantiaHistorico INT NOT NULL DEFAULT 0,
    FechaFinGarantia DATE NULL,
    CONSTRAINT FK_DetalleVentas_Ventas FOREIGN KEY (IdVenta) REFERENCES Ventas(IdVenta),
    CONSTRAINT FK_DetalleVentas_Productos FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto)
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
    MontoAbonado DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (MontoAbonado >= 0),
    SaldoCuota AS ((MontoCuota + RecargoMora) - MontoAbonado) PERSISTED,
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
    CostoUnitario DECIMAL(18,2) NULL,
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

CREATE NONCLUSTERED INDEX IX_Productos_Stock ON Productos(StockActual, StockMinimo) INCLUDE (CodigoSKU, Nombre, PrecioVenta);
CREATE NONCLUSTERED INDEX IX_Ventas_Fecha_Estado ON Ventas(FechaVenta, Estado) INCLUDE (NumeroFactura, IdCliente, TotalVenta);
CREATE NONCLUSTERED INDEX IX_Kardex_Producto_Fecha ON Kardex(IdProducto, FechaMovimiento DESC);
CREATE NONCLUSTERED INDEX IX_Cuotas_Credito_Estado ON CuotasCredito(IdCredito, Estado, FechaVencimiento);
GO

CREATE VIEW vw_ProductosStockBajo AS
SELECT 
    p.IdProducto,
    p.CodigoSKU,
    p.Nombre,
    p.Marca,
    p.Modelo,
    p.TipoEquipo,
    p.StockActual,
    p.StockMinimo,
    (p.StockMinimo - p.StockActual) AS CantidadFaltanteMinima,
    p.UltimoCostoCompra,
    pr.RazonSocial AS ProveedorPreferido,
    pr.Telefono AS TelefonoProveedor
FROM Productos p
LEFT JOIN Proveedores pr ON p.IdProveedorPreferido = pr.IdProveedor
WHERE p.Activo = 1 
  AND p.StockActual < p.StockMinimo;
GO

CREATE VIEW vw_EstadoCuentaCreditos AS
SELECT 
    cr.IdCredito,
    v.NumeroFactura,
    c.NumeroIdentificacion,
    c.Nombres + ' ' + c.Apellidos AS NombreCliente,
    c.Telefono,
    cr.MontoVentaTotal,
    cr.PrimaInicial,
    cr.MontoTotalCredito,
    cr.SaldoPendiente,
    cr.FrecuenciaPago,
    cr.Estado AS EstadoCredito,
    COUNT(cu.IdCuota) AS TotalCuotas,
    SUM(CASE WHEN cu.Estado = 'Pagada' THEN 1 ELSE 0 END) AS CuotasPagadas,
    MIN(CASE WHEN cu.Estado IN ('Pendiente', 'Parcial', 'Vencida') THEN cu.FechaVencimiento END) AS ProximoVencimiento
FROM Creditos cr
INNER JOIN Ventas v ON cr.IdVenta = v.IdVenta
INNER JOIN Clientes c ON v.IdCliente = c.IdCliente
LEFT JOIN CuotasCredito cu ON cr.IdCredito = cu.IdCredito
GROUP BY 
    cr.IdCredito, v.NumeroFactura, c.NumeroIdentificacion, c.Nombres, c.Apellidos, 
    c.Telefono, cr.MontoVentaTotal, cr.PrimaInicial, cr.MontoTotalCredito, 
    cr.SaldoPendiente, cr.FrecuenciaPago, cr.Estado;
GO

CREATE PROCEDURE sp_ConfirmarRecepcionCompra
    @IdCompra INT,
    @IdUsuarioRecepcion INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @EstadoActual VARCHAR(20), @NumeroDoc VARCHAR(50);
        SELECT @EstadoActual = Estado, @NumeroDoc = NumeroDocumento 
        FROM Compras WITH (UPDLOCK) WHERE IdCompra = @IdCompra;

        IF @EstadoActual IS NULL
            THROW 50001, 'La compra especificada no existe.', 1;

        IF @EstadoActual <> 'Pendiente'
            THROW 50002, 'Solo se pueden recepcionar compras en estado Pendiente.', 1;

        INSERT INTO Kardex (
            IdProducto, TipoMovimiento, Cantidad, StockAnterior, StockNuevo, 
            CostoUnitario, DocumentoOrigen, IdReferenciaOrigen, Justificacion, IdUsuarioResponsable
        )
        SELECT 
            dc.IdProducto,
            'Entrada por Compra',
            dc.Cantidad,
            p.StockActual,
            p.StockActual + dc.Cantidad,
            dc.CostoUnitario,
            @NumeroDoc,
            @IdCompra,
            'Recepción de compra confirmada',
            @IdUsuarioRecepcion
        FROM DetalleCompras dc
        INNER JOIN Productos p WITH (UPDLOCK) ON dc.IdProducto = p.IdProducto
        WHERE dc.IdCompra = @IdCompra;

        UPDATE p
        SET p.StockActual = p.StockActual + dc.Cantidad,
            p.UltimoCostoCompra = dc.CostoUnitario
        FROM Productos p
        INNER JOIN DetalleCompras dc ON p.IdProducto = dc.IdProducto
        WHERE dc.IdCompra = @IdCompra;

        UPDATE Compras
        SET Estado = 'Recibida',
            IdUsuarioRecepcion = @IdUsuarioRecepcion,
            FechaRecepcion = GETDATE()
        WHERE IdCompra = @IdCompra;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE sp_AnularCompra
    @IdCompra INT,
    @IdUsuarioAdmin INT,
    @MotivoAnulacion VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @EstadoActual VARCHAR(20), @NumeroDoc VARCHAR(50);
        SELECT @EstadoActual = Estado, @NumeroDoc = NumeroDocumento 
        FROM Compras WITH (UPDLOCK) WHERE IdCompra = @IdCompra;

        IF @EstadoActual = 'Anulada'
            THROW 50003, 'La compra ya se encuentra anulada.', 1;

        IF @EstadoActual = 'Recibida'
        BEGIN
            IF EXISTS (
                SELECT 1 
                FROM DetalleCompras dc
                INNER JOIN Productos p WITH (UPDLOCK) ON dc.IdProducto = p.IdProducto
                WHERE dc.IdCompra = @IdCompra AND p.StockActual < dc.Cantidad
            )
            BEGIN
                THROW 50004, 'No hay stock suficiente para revertir los productos de esta compra.', 1;
            END

            INSERT INTO Kardex (
                IdProducto, TipoMovimiento, Cantidad, StockAnterior, StockNuevo, 
                CostoUnitario, DocumentoOrigen, IdReferenciaOrigen, Justificacion, IdUsuarioResponsable
            )
            SELECT 
                dc.IdProducto,
                'Reversion por Anulacion Compra',
                dc.Cantidad,
                p.StockActual,
                p.StockActual - dc.Cantidad,
                dc.CostoUnitario,
                @NumeroDoc,
                @IdCompra,
                @MotivoAnulacion,
                @IdUsuarioAdmin
            FROM DetalleCompras dc
            INNER JOIN Productos p ON dc.IdProducto = p.IdProducto
            WHERE dc.IdCompra = @IdCompra;

            UPDATE p
            SET p.StockActual = p.StockActual - dc.Cantidad
            FROM Productos p
            INNER JOIN DetalleCompras dc ON p.IdProducto = dc.IdProducto
            WHERE dc.IdCompra = @IdCompra;
        END

        UPDATE Compras
        SET Estado = 'Anulada',
            IdUsuarioAnulacion = @IdUsuarioAdmin,
            FechaAnulacion = GETDATE(),
            MotivoAnulacion = @MotivoAnulacion
        WHERE IdCompra = @IdCompra;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE sp_ConfirmarVenta
    @IdVenta INT,
    @IdUsuarioVendedor INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @EstadoVenta VARCHAR(20), @NumeroFactura VARCHAR(30);
        SELECT @EstadoVenta = Estado, @NumeroFactura = NumeroFactura
        FROM Ventas WITH (UPDLOCK) WHERE IdVenta = @IdVenta;

        IF @EstadoVenta IS NULL
            THROW 50005, 'La venta no existe.', 1;

        IF @EstadoVenta <> 'Pendiente'
            THROW 50006, 'Solo se pueden confirmar ventas en estado Pendiente.', 1;

        IF NOT EXISTS (SELECT 1 FROM DetalleVentas WHERE IdVenta = @IdVenta)
            THROW 50007, 'La venta no tiene productos agregados.', 1;

        IF EXISTS (
            SELECT 1 
            FROM DetalleVentas dv
            INNER JOIN Productos p WITH (UPDLOCK) ON dv.IdProducto = p.IdProducto
            WHERE dv.IdVenta = @IdVenta AND p.StockActual < dv.Cantidad
        )
        BEGIN
            THROW 50008, 'Stock insuficiente en uno o más productos para completar la venta.', 1;
        END

        INSERT INTO Kardex (
            IdProducto, TipoMovimiento, Cantidad, StockAnterior, StockNuevo, 
            CostoUnitario, DocumentoOrigen, IdReferenciaOrigen, Justificacion, IdUsuarioResponsable
        )
        SELECT 
            dv.IdProducto,
            'Salida por Venta',
            dv.Cantidad,
            p.StockActual,
            p.StockActual - dv.Cantidad,
            p.UltimoCostoCompra,
            @NumeroFactura,
            @IdVenta,
            'Venta confirmada',
            @IdUsuarioVendedor
        FROM DetalleVentas dv
        INNER JOIN Productos p ON dv.IdProducto = p.IdProducto
        WHERE dv.IdVenta = @IdVenta;

        UPDATE p
        SET p.StockActual = p.StockActual - dv.Cantidad
        FROM Productos p
        INNER JOIN DetalleVentas dv ON p.IdProducto = dv.IdProducto
        WHERE dv.IdVenta = @IdVenta;

        UPDATE dv
        SET dv.MesesGarantiaHistorico = p.MesesGarantia,
            dv.FechaFinGarantia = DATEADD(MONTH, p.MesesGarantia, CAST(GETDATE() AS DATE))
        FROM DetalleVentas dv
        INNER JOIN Productos p ON dv.IdProducto = p.IdProducto
        WHERE dv.IdVenta = @IdVenta;

        UPDATE Ventas
        SET Estado = 'Completada'
        WHERE IdVenta = @IdVenta;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE sp_AnularVenta
    @IdVenta INT,
    @IdUsuarioAutoriza INT,
    @MotivoAnulacion VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (
            SELECT 1 FROM Usuarios u
            INNER JOIN Roles r ON u.IdRol = r.IdRol
            WHERE u.IdUsuario = @IdUsuarioAutoriza AND r.CodigoRol = 'Rol-01' AND u.Activo = 1
        )
        BEGIN
            THROW 50009, 'Solo un usuario con rol Administrador (Rol-01) puede autorizar la anulación.', 1;
        END

        DECLARE @EstadoVenta VARCHAR(20), @NumeroFactura VARCHAR(30);
        SELECT @EstadoVenta = Estado, @NumeroFactura = NumeroFactura
        FROM Ventas WITH (UPDLOCK) WHERE IdVenta = @IdVenta;

        IF @EstadoVenta = 'Anulada'
            THROW 50010, 'La venta ya fue anulada previamente.', 1;

        IF @EstadoVenta = 'Completada'
        BEGIN
            INSERT INTO Kardex (
                IdProducto, TipoMovimiento, Cantidad, StockAnterior, StockNuevo, 
                CostoUnitario, DocumentoOrigen, IdReferenciaOrigen, Justificacion, IdUsuarioResponsable
            )
            SELECT 
                dv.IdProducto,
                'Reversion por Anulacion Venta',
                dv.Cantidad,
                p.StockActual,
                p.StockActual + dv.Cantidad,
                p.UltimoCostoCompra,
                @NumeroFactura,
                @IdVenta,
                @MotivoAnulacion,
                @IdUsuarioAutoriza
            FROM DetalleVentas dv
            INNER JOIN Productos p WITH (UPDLOCK) ON dv.IdProducto = p.IdProducto
            WHERE dv.IdVenta = @IdVenta;

            UPDATE p
            SET p.StockActual = p.StockActual + dv.Cantidad
            FROM Productos p
            INNER JOIN DetalleVentas dv ON p.IdProducto = dv.IdProducto
            WHERE dv.IdVenta = @IdVenta;
        END

        UPDATE CuotasCredito
        SET Estado = 'Anulada'
        WHERE IdCredito IN (SELECT IdCredito FROM Creditos WHERE IdVenta = @IdVenta);

        UPDATE Creditos
        SET Estado = 'Anulado', SaldoPendiente = 0
        WHERE IdVenta = @IdVenta;

        UPDATE Ventas
        SET Estado = 'Anulada',
            IdUsuarioAutorizaAnulacion = @IdUsuarioAutoriza,
            FechaAnulacion = GETDATE(),
            MotivoAnulacion = @MotivoAnulacion
        WHERE IdVenta = @IdVenta;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE sp_RegistrarAbono
    @NumeroRecibo VARCHAR(30),
    @IdCuota INT,
    @IdUsuarioRegistro INT,
    @MontoAbono DECIMAL(18,2),
    @MetodoPago VARCHAR(30),
    @Observaciones VARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IdCredito INT, @SaldoCuota DECIMAL(18,2), @EstadoCuota VARCHAR(20);
        SELECT @IdCredito = IdCredito, @SaldoCuota = SaldoCuota, @EstadoCuota = Estado
        FROM CuotasCredito WITH (UPDLOCK) WHERE IdCuota = @IdCuota;

        IF @EstadoCuota IN ('Pagada', 'Anulada')
            THROW 50011, 'La cuota seleccionada ya está pagada o anulada.', 1;

        IF @MontoAbono > @SaldoCuota
            THROW 50012, 'El monto del abono supera el saldo pendiente de la cuota.', 1;

        INSERT INTO Abonos (NumeroRecibo, IdCuota, IdUsuarioRegistro, MontoAbono, MetodoPago, Observaciones)
        VALUES (@NumeroRecibo, @IdCuota, @IdUsuarioRegistro, @MontoAbono, @MetodoPago, @Observaciones);

        UPDATE CuotasCredito
        SET MontoAbonado = MontoAbonado + @MontoAbono,
            Estado = CASE WHEN (MontoAbonado + @MontoAbono) >= (MontoCuota + RecargoMora) 
                          THEN '