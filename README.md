PruebaHAS Mateo Blanco

en esta prueba práctica se encuentra un microservicio que expone un API Rest para el manejo de datos vía CRUD, dedicado a una entidad de las siguientes:

-Usuario

-Categoria

-Producto

-Cliente

-Venta

la entidad seleccionada es Usuario, dicha API Rest tiene expuestos los siguientes endpoints:

-/api/v1/User (GET)

-/api/v1/User (POST)

-/api/v1/User (PUT)

-/api/v1/User/id={id} (GET)

-/api/v1/User/id={id} (DELETE)

-/api/v1/User/Login (POST)

el login es un extra que permite controlar los accesos y autorizaciones a endpoints de otras APi's relacionadas con las demás entidades no escogidas, también en las configuraciones del microservicio se estableció una política de CORS para evitar problemas de comunicación con el frontend, el frontend está hecho en angular 15, con bootstrap 5 como hoja de estilos 
