# FundManager

Paso a paso para que el Web API Core y la Aplicación Web puedan ejecutarse desde Docker.

## Requisitos Previos

Antes de instalar FundManager, asegúrese de contar con lo siguiente:

- Credenciales válidas de AWS
- Tema de AWS SNS con dos suscripciones:
  - Notificaciones por EMAIL
  - Notificaciones por SMS
- Tablas AWS DynamoDB creadas utilizando los archivos JSON proporcionados
- Docker Desktop instalado en su sistema
- Git para clonar el repositorio

## Configuración de AWS

### Configuración de SNS
Cree un tema en AWS SNS con suscripciones para notificaciones por EMAIL y SMS.

### Configuración de DynamoDB
Cree las siguientes tablas utilizando los archivos de esquema JSON ubicados en `Entregables Prueba Técnica\Punto 1 - Aplicación FundManager\Tablas de DynamoDB`:
- ActiveLinkages.json
- Customers.json
- Funds.json
- Transactions.json

Para cada archivo, ejecute:
```bash
aws dynamodb create-table --cli-input-json file://esquema_TuTabla.json
```

## Guía de Instalación

### 1. Clonar el Repositorio
```bash
git clone http://github.com/crimauro/FundManager
cd FundManager
```

### 2. Configuración del Entorno
Cree un archivo `.env` en el directorio `FundCoreAPI` con el siguiente contenido:
```
AWS_ACCESS_KEY_ID=su_aws_access_key
AWS_SECRET_ACCESS_KEY=su_aws_secret_key
AWS_REGION=su_aws_region
AWS_TOPIC_ARN=su_sns_topic_arn
```

### 3. Configuración de Red Docker
Cree una red Docker para la comunicación entre contenedores:
```bash
docker network create fundmanager-network
```

### 4. Configuración de la API Core
Navegue al directorio de la API Core:
```bash
cd FundCoreAPI
```

Construya la imagen Docker:
```bash
docker build -t fundcoreapi .
```

Ejecute el contenedor:
```bash
docker run -d -p 60721:60721 --name fundcoreapi-container --network fundmanager-network --env-file .env fundcoreapi
```

### 5. Configuración de la Aplicación Web
Navegue al directorio de la Aplicación Web:
```bash
cd ../FundWeb
```

Construya la imagen Docker:
```bash
docker build -t fundweb-dev .
```

Ejecute el contenedor:
```bash
docker run -d -p 4200:80 --name fundweb-dev-container --network fundmanager-network fundweb-dev
```

## Acceso a la Aplicación
Abra su navegador web y navegue a:
```
http://localhost:4200/
```

La aplicación FundManager debería estar ahora en funcionamiento y accesible.

## Solución de Problemas

Si encuentra problemas durante la instalación o ejecución, verifique:

- Que las credenciales de AWS sean correctas
- Que los puertos 60721 y 4200 estén disponibles en su sistema
- Que Docker Desktop esté ejecutándose correctamente
- Que el archivo `.env` contenga los valores correctos
- Que todas las tablas de DynamoDB se hayan creado correctamente

## Estructura del Proyecto

- **FundCoreAPI**: Servicio de API backend que gestiona la lógica de negocio
- **FundWeb**: Aplicación frontend Angular para la interfaz de usuario

## Licencia

Este proyecto está licenciado bajo los términos de [MIT License](LICENSE).  
¡Siéntete libre de contribuir o utilizar este proyecto como base para tus propias aplicaciones!

