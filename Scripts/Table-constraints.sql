use PruebaHas 

-- =============================================
-- 1. CLAVES FORÁNEAS (RELACIONES ENTRE TABLAS)
-- =============================================

-- Un Producto pertenece a una Categoria
ALTER TABLE Producto
ADD CONSTRAINT FK_Producto_Categoria 
FOREIGN KEY (IdCategoria) REFERENCES Categoria(IdCategoria);

-- Una Venta es realizada por un Usuario
ALTER TABLE Venta
ADD CONSTRAINT FK_Venta_Usuario 
FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario);

-- Una Venta pertenece a un Cliente
ALTER TABLE Venta
ADD CONSTRAINT FK_Venta_Cliente 
FOREIGN KEY (IdCliente) REFERENCES Cliente(IdCliente);

-- Detalle de Venta: Relación con la Venta principal
ALTER TABLE VentaProducto
ADD CONSTRAINT FK_VentaProducto_Venta 
FOREIGN KEY (IdVenta) REFERENCES Venta(IdVenta);

-- Detalle de Venta: Relación con el Producto vendido
ALTER TABLE VentaProducto
ADD CONSTRAINT FK_VentaProducto_Producto 
FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto);


-- =============================================
-- 2. REGLAS DE NEGOCIO (CHECKS)
-- =============================================

-- El precio del producto no puede ser negativo
ALTER TABLE Producto
ADD CONSTRAINT CK_Producto_PrecioPositivo 
CHECK (PrecioProducto >= 0);

-- La cantidad vendida debe ser al menos 1
ALTER TABLE VentaProducto
ADD CONSTRAINT CK_VentaProducto_CantidadMinima 
CHECK (CantidadProducto > 0);

-- El total de la venta no puede ser negativo
ALTER TABLE Venta
ADD CONSTRAINT CK_Venta_TotalPositivo 
CHECK (TotalVenta >= 0);


-- =============================================
-- 3. VALORES POR DEFECTO (OPCIONAL PERO RECOMENDADO)
-- =============================================

-- Si no se especifica, el producto NO está agotado (0)
ALTER TABLE Producto
ADD CONSTRAINT DF_Producto_IsAgotado 
DEFAULT 0 FOR IsAgotado;