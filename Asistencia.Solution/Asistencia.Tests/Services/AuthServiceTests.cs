using Asistencia.Application.Common.Security;
using Asistencia.Application.Interfaces;
using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Asistencia.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IRolUsuarioRepository> _rolUsuarioRepositoryMock;
    private readonly Mock<IRolRepository> _rolRepositoryMock;
    private readonly Mock<ITokenBlacklistService> _tokenBlacklistServiceMock;
    private readonly IMemoryCache _cache;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _configurationMock = new Mock<IConfiguration>();
        _rolUsuarioRepositoryMock = new Mock<IRolUsuarioRepository>();
        _rolRepositoryMock = new Mock<IRolRepository>();
        _tokenBlacklistServiceMock = new Mock<ITokenBlacklistService>();
        _cache = new MemoryCache(new MemoryCacheOptions());

        // Setup configuration for JWT (needed for Login, but good to have for constructor)
        _configurationMock.Setup(c => c["Jwt:Key"]).Returns("SuperSecretKey12345678901234567890");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("AsistenciaApi");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("AsistenciaUsers");
        _configurationMock.Setup(c => c["Jwt:DurationInMinutes"]).Returns("60");

        _authService = new AuthService(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _configurationMock.Object,
            _rolUsuarioRepositoryMock.Object,
            _rolRepositoryMock.Object,
            _tokenBlacklistServiceMock.Object,
            _cache
        );
    }

    [Fact]
    public async Task LoginAsync_Should_Return_Failure_When_User_Not_Found()
    {
        // Arrange
        var request = new LoginRequest { Username = "nonexistent", Password = "password" };
        _userRepositoryMock.Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync((UsuarioResponse?)null);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Usuario o contraseña incorrectos", result.Error);
    }

    [Fact]
    public async Task LoginAsync_Should_Return_Failure_When_Password_Invalid()
    {
        // Arrange
        var request = new LoginRequest { Username = "user", Password = "wrongpassword" };
        var user = new UsuarioResponse { IdUsuario = 1, Username = "user" };
        
        _userRepositoryMock.Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync(user);
        _userRepositoryMock.Setup(x => x.GetPasswordHashAsync(request.Username))
            .ReturnsAsync("hash");
        _passwordHasherMock.Setup(x => x.Verify(request.Password, "hash"))
            .Returns(false);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Usuario o contraseña incorrectos", result.Error);
    }

    [Fact]
    public async Task LoginAsync_Should_Return_Success_When_Credentials_Valid()
    {
        // Arrange
        var request = new LoginRequest { Username = "user", Password = "password" };
        var user = new UsuarioResponse { IdUsuario = 1, Username = "user", IdEmpleado = 10 };
        
        _userRepositoryMock.Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync(user);
        _userRepositoryMock.Setup(x => x.GetPasswordHashAsync(request.Username))
            .ReturnsAsync("hash");
        _passwordHasherMock.Setup(x => x.Verify(request.Password, "hash"))
            .Returns(true);
        _rolUsuarioRepositoryMock.Setup(x => x.GetByUserIdAsync(user.IdUsuario))
            .ReturnsAsync(new List<RolUsuarioResponse>());
        _rolRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<RolResponse>());

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value.Token);
        Assert.Equal(request.Username, result.Value.Username);
    }

    [Fact]
    public async Task RegisterAsync_Should_Return_Failure_When_Username_Exists()
    {
        // Arrange
        var request = new RegisterRequest { Username = "existingUser", Email = "test@test.com", Password = "Password123!" };
        _userRepositoryMock.Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync(new UsuarioResponse { IdUsuario = 1, Username = "existingUser" });

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("El nombre de usuario ya existe", result.Error);
    }

    [Fact]
    public async Task RegisterAsync_Should_Return_Failure_When_Email_Exists()
    {
        // Arrange
        var request = new RegisterRequest { Username = "newUser", Email = "existing@test.com", Password = "Password123!" };
        _userRepositoryMock.Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync((UsuarioResponse?)null);
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync(new UsuarioResponse { IdUsuario = 2, Username = "otherUser", Email = "existing@test.com" });

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("El correo electrónico ya está registrado", result.Error);
    }

    [Fact]
    public async Task RegisterAsync_Should_Return_Success_When_User_Is_New()
    {
        // Arrange
        var request = new RegisterRequest { Username = "newUser", Email = "new@test.com", Password = "Password123!" };
        _userRepositoryMock.Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync((UsuarioResponse?)null);
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync((UsuarioResponse?)null);
        _passwordHasherMock.Setup(x => x.Hash(request.Password)).Returns("hashedPassword");
        _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CreateUsuarioRequest>()))
            .ReturnsAsync(10);

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(10, result.Value);
        _userRepositoryMock.Verify(x => x.AddAsync(It.Is<CreateUsuarioRequest>(r => 
            r.Username == request.Username && 
            r.Email == request.Email && 
            r.IdEmpleado == 0)), Times.Once);
    }
}
