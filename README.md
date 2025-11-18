API RESTful para manejar tareas.
URL de la pagina del proyecto: https://roadmap.sh/projects/todo-list-api

# Requisitos
- Registro de usuario para crear un nuevo usuario
- Punto de conexión de inicio de sesión para autenticar al usuario y generar un token
- Operaciones CRUD para gestionar la lista de tareas
- Implemente la autenticación de usuarios para permitir que solo los usuarios autorizados accedan a la lista de tareas.
- Implementar medidas de seguridad y manejo de errores
- Utilice una base de datos para almacenar los datos del usuario y la lista de tareas (puede utilizar cualquier base de datos de su elección).
- Implementar una validación de datos adecuada
- Implementar paginación y filtrado para la lista de tareas.

# Instalacion
Para usar la aplicacion, seguir los siuientes pasos:
1. Clonar el repositorio:

        git clone https://github.com/chirogod/TodoListApi.git 

2. Navegar a la raiz del proyecto

		cd TodoList

3. Abrir el IDE de preferencia.
4. Configurar ConnectionStrings a su servidro en appsettings.json.

		"ConnectionStrings": {
        "DefaultConnection": "Server=ServerName;Database=DbName;Trusted_Connection=True;TrustServerCertificate=True"
        },
   
5. Ejecutar las migraciones de EF Core correspondientes asegurandose de tener las herramientas instaladas.
6. En cada peticion debera enviar el token obtenido en el login.
