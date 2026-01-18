# JA Seguro que Vuelas – API

## Descripción general
**JA Seguro que Vuelas – API** es una API REST desarrollada en **ASP.NET Core** como parte de un proyecto escolar.  
El objetivo de este proyecto es simular el funcionamiento del backend de una **agencia de viajes digital**, permitiendo gestionar solicitudes de contacto de usuarios interesados en opciones de vuelo.

La API funciona como intermediaria entre una página web y una base de datos SQL, procesando la información enviada por los usuarios y almacenándola de manera estructurada para su posterior consulta o seguimiento.

---

## Objetivo del proyecto
El proyecto busca aplicar conocimientos de:
- Desarrollo de APIs REST
- Arquitectura cliente-servidor
- Manejo de bases de datos relacionales
- Uso de stored procedures
- Buenas prácticas en el desarrollo backend

Todo esto enfocado a un caso de uso realista dentro del contexto de una agencia de viajes.

---

## Funcionalidades principales
- Recepción de solicitudes de contacto desde una página web
- Registro de datos del usuario (nombre, correo, teléfono, origen y destino del viaje)
- Almacenamiento de la información en una base de datos SQL
- Comunicación mediante endpoints REST
- Manejo de errores y respuestas HTTP adecuadas

---

## Tecnologías utilizadas
- **ASP.NET Core**
- **C#**
- **SQL Server**
- **Stored Procedures**
- **Entity Framework Core / ADO.NET**
- **Swagger (OpenAPI)** para documentación y pruebas de la API

---

## Arquitectura del sistema
El sistema sigue una arquitectura básica cliente-servidor:

- **Frontend**: Página web que consume la API
- **Backend**: API REST en ASP.NET Core
- **Base de datos**: SQL Server con stored procedures para la persistencia de datos

---

## Base de datos
La información se almacena en una base de datos relacional SQL.  
El acceso a los datos se realiza mediante **stored procedures**, lo que permite:

