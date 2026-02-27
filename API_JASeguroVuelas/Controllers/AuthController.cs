using Microsoft.AspNetCore.Mvc;
using API_JASeguroVuelas.Models;
using API_JASeguroVuelas.Services;
using BCrypt.Net;

namespace API_JASeguroVuelas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UsuarioService _usuarioService;
        private readonly JwtService _jwtService;

        public AuthController(
            ILogger<AuthController> logger,
            UsuarioService usuarioService,
            JwtService jwtService)
        {
            _logger = logger;
            _usuarioService = usuarioService;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Registra un nuevo usuario
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UsuarioRequest request)
        {
            try
            {
                _logger.LogInformation("Intentando registrar usuario: {Email}", request.Email);

                // Validación
                if (string.IsNullOrWhiteSpace(request.Email) ||
                    string.IsNullOrWhiteSpace(request.Nombre) ||
                    string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new { message = "Email, Nombre y Contraseña son requeridos" });
                }

                if (request.Password.Length < 6)
                {
                    return BadRequest(new { message = "La contraseña debe tener al menos 6 caracteres" });
                }

                // Validar rol
                var rol = request.Rol?.Trim() ?? "Cliente";
                if (rol != "Admin" && rol != "Cliente")
                {
                    return BadRequest(new { message = "El rol debe ser 'Admin' o 'Cliente'" });
                }

                // Verificar si el email ya existe
                var existingUser = await _usuarioService.GetByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "El email ya está registrado" });
                }

                // Crear nuevo usuario
                var ahora = DateTime.UtcNow;
                var nuevoUsuario = new Usuario
                {
                    Email = request.Email.ToLowerInvariant(),
                    Nombre = request.Nombre,
                    Telefono = request.Telefono,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Rol = rol,
                    FechaCreacion = ahora,
                    FechaActualizacion = ahora
                };

                await _usuarioService.CreateAsync(nuevoUsuario);

                // Generar token
                var token = _jwtService.GenerateToken(nuevoUsuario);

                _logger.LogInformation("Usuario registrado exitosamente: {Email}", request.Email);

                var response = new AuthResponse
                {
                    Token = token,
                    Usuario = new UsuarioDto
                    {
                        Id = nuevoUsuario.Id ?? string.Empty,
                        Email = nuevoUsuario.Email,
                        Nombre = nuevoUsuario.Nombre,
                        Telefono = nuevoUsuario.Telefono,
                        Rol = nuevoUsuario.Rol
                    }
                };

                return CreatedAtAction(nameof(GetCurrentUser), new { id = nuevoUsuario.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario");
                return StatusCode(500, new { message = "Error al registrar el usuario", error = ex.Message });
            }
        }

        /// <summary>
        /// Inicia sesión con email y contraseña
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Intentando login: {Email}", request.Email);

                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new { message = "Email y contraseña son requeridos" });
                }

                // Buscar usuario por email
                var usuario = await _usuarioService.GetByEmailAsync(request.Email);
                if (usuario == null)
                {
                    return Unauthorized(new { message = "Email o contraseña incorrectos" });
                }

                // Verificar contraseña
                if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
                {
                    return Unauthorized(new { message = "Email o contraseña incorrectos" });
                }

                // Generar token
                var token = _jwtService.GenerateToken(usuario);

                _logger.LogInformation("Login exitoso: {Email}", request.Email);

                var response = new AuthResponse
                {
                    Token = token,
                    Usuario = new UsuarioDto
                    {
                        Id = usuario.Id ?? string.Empty,
                        Email = usuario.Email,
                        Nombre = usuario.Nombre,
                        Telefono = usuario.Telefono,
                        Rol = usuario.Rol
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al hacer login");
                return StatusCode(500, new { message = "Error al iniciar sesión", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene la información del usuario actual (requiere autenticación)
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "No autorizado" });
                }

                var usuario = await _usuarioService.GetByIdAsync(userId);
                if (usuario == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                var usuarioDto = new UsuarioDto
                {
                    Id = usuario.Id ?? string.Empty,
                    Email = usuario.Email,
                    Nombre = usuario.Nombre,
                    Telefono = usuario.Telefono,
                    Rol = usuario.Rol
                };

                return Ok(usuarioDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario actual");
                return StatusCode(500, new { message = "Error al obtener información del usuario", error = ex.Message });
            }
        }
    }
}
