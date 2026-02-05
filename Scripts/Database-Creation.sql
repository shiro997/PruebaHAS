CREATE DATABASE PruebaHas;

use PruebaHas

--creación de tabla
create table Usuario(IdUsuario int identity(1,1) primary key, NombreUsuario nvarchar(max), Email nvarchar(max), UsrPassword nvarchar(max))

