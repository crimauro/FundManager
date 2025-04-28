# FundCoreAPI

FundCoreAPI es un proyecto basado en **ASP.NET Core** que implementa un Web API para la gestión de fondos, clientes, transacciones y vínculos activos. Este proyecto utiliza tecnologías modernas y sigue los principios de **Arquitectura Limpia (Clean Architecture)** para garantizar escalabilidad, mantenibilidad y flexibilidad.

---

## Tecnologías utilizadas

### Frameworks y herramientas principales:
- **ASP.NET Core**: Framework para construir aplicaciones web y servicios RESTful de alto rendimiento.
- **Amazon DynamoDB**: Base de datos NoSQL utilizada para almacenar datos relacionados con fondos, clientes, transacciones y vínculos activos.
- **Amazon SNS**: Notificaciones a EMails y SMS
- **Swagger/OpenAPI**: Herramienta para documentar y probar los endpoints del Web API.
- **CORS (Cross-Origin Resource Sharing)**: Configurado para permitir solicitudes desde cualquier origen, facilitando la interacción con clientes externos.

### SDKs y bibliotecas:
- **AWS SDK para .NET**: Utilizado para interactuar con servicios de AWS como DynamoDB, Secrets Manager y SNS.
- **Swashbuckle.AspNetCore**: Generación automática de documentación Swagger para el Web API.
- **xUnit y Moq**: Herramientas para pruebas unitarias y simulación de dependencias.

### Lenguaje y framework:
- **C# 13.0**: Lenguaje de programación utilizado para el desarrollo del proyecto.
- **.NET 9**: Framework objetivo que ofrece características modernas y mejoras de rendimiento.

---

## Arquitectura limpia implementada

El proyecto sigue los principios de **Arquitectura Limpia (Clean Architecture)**, que se centra en la separación de responsabilidades y la independencia de los componentes. A continuación, se describen las capas principales:

### Capas del sistema:
1. **Capa de Presentación (Controllers)**:
   - Contiene los controladores que exponen los endpoints del Web API.
   - No incluye lógica de negocio, delegando las operaciones a los servicios.

2. **Capa de Aplicación (Services)**:
   - Contiene la lógica de negocio y las reglas de la aplicación.
   - Los servicios interactúan con los repositorios para realizar operaciones sobre los datos.

3. **Capa de Infraestructura (Repositories)**:
   - Implementa el acceso a datos y la interacción con DynamoDB.
   - Proporciona una abstracción para la capa de aplicación.

4. **Capa de Dominio (Models)**:
   - Define las entidades principales del sistema, como `Fund`, `Customer`, `Transaction`, etc.
   - Estas entidades son independientes de cualquier tecnología o framework.

### Principios clave:
- **Independencia de frameworks**: El código no depende directamente de ASP.NET Core o DynamoDB, sino que utiliza interfaces y abstracciones para desacoplar las dependencias.
- **Inversión de dependencias**: Las dependencias se inyectan en tiempo de ejecución, facilitando las pruebas unitarias y la flexibilidad.
- **Separación de responsabilidades**: Cada capa tiene una responsabilidad específica, mejorando la mantenibilidad y escalabilidad.

---

## Configuración inicial

### Variables de entorno requeridas:
El proyecto utiliza credenciales de AWS para interactuar con DynamoDB. Asegúrate de configurar las siguientes variables de entorno antes de ejecutar la aplicación:

- `AWS_ACCESS_KEY_ID`: Clave de acceso de AWS.
- `AWS_SECRET_ACCESS_KEY`: Clave secreta de AWS.
- `AWS_REGION`: Región de AWS (por ejemplo, `us-east-1`).

Si alguna de estas variables no está configurada, la aplicación lanzará una excepción.

---

## Ejecución del proyecto sin Contenerizar

1. Clona este repositorio:
git clone https://github.com/tu-usuario/FundCoreAPI.git cd FundCoreAPI


2. Configura las variables de entorno requeridas.

3. Restaura las dependencias y compila el proyecto:
dotnet restore dotnet build


4. Ejecuta la aplicación:
dotnet run --project FundCoreAPI   


5. Accede a la documentación Swagger en:  
[http://localhost:60721/swagger](http://localhost:60721/swagger)

---

## Pruebas

El proyecto incluye pruebas unitarias utilizando **xUnit** y **Moq**. Para ejecutar las pruebas, utiliza el siguiente comando:
dotnet test


# Fundweb

## Descripción Técnica

### Tecnologías Utilizadas

1. **Angular**: Framework de desarrollo frontend basado en TypeScript que permite construir aplicaciones web dinámicas y escalables. Angular proporciona herramientas como enrutamiento, inyección de dependencias y un sistema de componentes reutilizables.
2. **Node.js**: Utilizado como entorno de ejecución para construir y empaquetar la aplicación Angular. Node.js permite la instalación de dependencias y la ejecución de scripts de construcción mediante `npm`.
3. **Docker**: Herramienta de contenerización que permite empaquetar la aplicación y sus dependencias en un contenedor portátil. Esto asegura que la aplicación se ejecute de manera consistente en cualquier entorno.

### Arquitectura Limpia

La aplicación sigue principios de arquitectura limpia, organizando el código en capas bien definidas para mejorar la mantenibilidad, escalabilidad y testabilidad. A continuación, se describen las capas principales:

1. **Capa de Presentación (Frontend)**:
   - Implementada con Angular.
   - Contiene componentes, servicios y rutas que manejan la interacción del usuario.
   - Utiliza servicios para comunicarse con la API y obtener datos.

2. **Capa de Servicios**:
   - Los servicios como [`ApiFundsService`](src/app/services/api-funds.service.ts) encapsulan la lógica de comunicación con la API REST.
   - Facilitan la reutilización del código y desacoplan la lógica de negocio de los componentes.

3. **Capa de Modelos**:
   - Define interfaces como [`ApiTransaction`](src/app/models/api-transaction.model.ts) y [`ApiFund`](src/app/models/api-fund.model.ts) para estructurar los datos que se manejan en la aplicación.
   - Garantiza consistencia en el manejo de datos entre las diferentes capas.

4. **Capa de Configuración**:
   - Centraliza configuraciones como la URL base de la API en archivos como [`environment.ts`](src/environments/environment.ts), lo que facilita la gestión de entornos (desarrollo, producción, etc.).


---

## Licencia

Este proyecto está licenciado bajo los términos de [MIT License](LICENSE).  
¡Siéntete libre de contribuir o utilizar este proyecto como base para tus propias aplicaciones!
   