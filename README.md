# api-website-registration-desktop-part-2
Este proyecto corresponde a la segunda parte de un sistema de registro multiplataforma. Consiste en una aplicación de escritorio desarrollada en C# que consume una API REST, permitiendo realizar operaciones CRUD (crear, listar, modificar y eliminar registros) mediante una arquitectura basada en servicios.

## Características
- Crear, mostrar, editar y eliminar registros
- Consumo de API REST
- Validaciones de datos
- Interfaz gráfica en modo oscuro
- DataGridView solo lectura

## Tecnologías
- PHP
- MySQL
- C#
- Windows Forms
- Git / GitHub

## Base de datos
El repositorio incluye únicamente el script SQL para crear la base de datos.
No se incluyen datos reales por motivos de seguridad.

## Uso
1. Importar el archivo SQL en MySQL
2. Configurar la URL de la API
3. Ejecutar el cliente

Toda la comunicación se realiza mediante peticiones HTTP hacia un servicio backend alojado en:

http://localhost/serv/index.php

## Arquitectura del Sistema

El sistema está dividido en dos componentes principales:

Cliente de Escritorio (Frontend)

- Desarrollado en C#

- Framework: .NET Windows Forms

- Consumo de API con HttpClient

- Deserialización JSON con Newtonsoft.Json

- Interfaz moderna con diseño oscuro personalizado

API REST (Backend)

- PHP

- MySQL

- Respuestas en formato JSON

- Control por parámetro tipo

- Protección mediante clave (llave)

## Lógica del Sistema
Consulta Automática al Iniciar

Cuando el formulario carga (Form1_Load):

- Se bloquea edición manual del DataGridView

- Se configura selección completa de filas

- Se ejecuta automáticamente btnConsultaAsync()

- Se cargan los registros desde la API

Crear Registro

Método: btnAcrear()

Validaciones:

- No permite campos vacíos

- Envía datos a la API

- Refresca automáticamente la tabla

- Limpia campos

- Muestra mensaje de éxito o error

Proceso interno:

- Captura datos de los TextBox

- Construye URL dinámica

- Envía petición con HttpClient

Recarga DataGridViewLógica del Sistema
🔄 Consulta Automática al Iniciar

Cuando el formulario carga (Form1_Load):

Se bloquea edición manual del DataGridView

Se configura selección completa de filas

Se ejecuta automáticamente btnConsultaAsync()

Se cargan los registros desde la API

➕ Crear Registro

Método: btnAcrear()

Validaciones:

No permite campos vacíos

Envía datos a la API

Refresca automáticamente la tabla

Limpia campos

Muestra mensaje de éxito o error

Proceso interno:

Captura datos de los TextBox

Construye URL dinámica

Envía petición con HttpClient

Recarga DataGridView
