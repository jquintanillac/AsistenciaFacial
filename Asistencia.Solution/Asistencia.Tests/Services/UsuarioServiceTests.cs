using Asistencia.Application.Common;
using Asistencia.Application.Common.Security;
using Asistencia.Application.Interfaces;
using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace Asistencia.Tests.Services;

public class UsuarioServiceTests
{
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateUsuarioRequest>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateUsuarioRequest>> _updateValidatorMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly UsuarioService _service;

    public UsuarioServiceTests()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _createValidatorMock = new Mock<IValidator<CreateUsuarioRequest>>();
        _updateValidatorMock = new Mock<IValidator<UpdateUsuarioRequest>>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        
        _service = new UsuarioService(
            _repositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object,
            _passwordHasherMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenValid()
    {
        var request = new CreateUsuarioRequest { Username = "user", Email = "test@test.com", Password = "password" };
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.GetByUsernameAsync(request.Username)).ReturnsAsync((UsuarioResponse?)null);
        _repositoryMock.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync((UsuarioResponse?)null);
        _passwordHasherMock.Setup(x => x.Hash(request.Password)).Returns("hashed_password");
        _repositoryMock.Setup(x => x.AddAsync(request)).ReturnsAsync(1);

        var result = await _service.CreateAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
        _passwordHasherMock.Verify(x => x.Hash("password"), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnFailure_WhenUsernameExists()
    {
        var request = new CreateUsuarioRequest { Username = "user", Email = "test@test.com", Password = "password" };
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync(new UsuarioResponse { IdUsuario = 1, Username = "user" });

        var result = await _service.CreateAsync(request);

        Assert.False(result.IsSuccess);
        Assert.Equal("Username already exists", result.Error);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnFailure_WhenEmailExists()
    {
        var request = new CreateUsuarioRequest { Username = "user", Email = "test@test.com", Password = "password" };
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.GetByUsernameAsync(request.Username)).ReturnsAsync((UsuarioResponse?)null);
        _repositoryMock.Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync(new UsuarioResponse { IdUsuario = 1, Email = "test@test.com" });

        var result = await _service.CreateAsync(request);

        Assert.False(result.IsSuccess);
        Assert.Equal("Email already exists", result.Error);
    }
}
