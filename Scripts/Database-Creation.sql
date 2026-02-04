CREATE DATABASE PruebaHas;

use PruebaHas go;

--creación de tablas
create table Usuario(IdUsuario int primary key, NombreUsuario nvarchar(max), Email nvarchar(max), UsrPassword nvarchar(max))

create table Cliente (IdCliente int primary key, NombreCliete nvarchar(max))

create table Categoria (IdCategoria int primary key, NombreCategoria nvarchar(max))

create table Producto (IdProducto int primary key, NombreProducto nvarchar(max), IdCategoria int, PrecioProducto int, IsAgotado bit)

create table Venta (IdVenta int primary key, IdUsuario int, IdCliente int, TotalVenta int)

create table VentaProducto (IdVenta int, IdProducto int, CantidadProducto int)

