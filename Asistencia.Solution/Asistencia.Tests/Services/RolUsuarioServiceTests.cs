using Asistencia.Application.Common;
using Asistencia.Application.Interfaces;
using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace Asistencia.Tests.Services;

public class RolUsuarioServiceTests
{
    private readonly Mock<IRolUsuarioRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateRolUsuarioRequest>> _createValidatorMock;
    private readonly RolUsuarioService _service;

    public RolUsuarioServiceTests()
    {
        _repositoryMock = new Mock<IRolUsuarioRepository>();
        _createValidatorMock = new Mock<IValidator<CreateRolUsuarioRequest>>();
        
        _service = new RolUsuarioService(
            _repositoryMock.Object,
            _createValidatorMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnSuccess_WhenRolUsuariosExist()
    {
        var rolUsuarios = new List<RolUsuarioResponse> { new RolUsuarioResponse { IdUsuario = 1, IdRol = 1 } };
        _repositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(rolUsuarios);

        var result = await _service.GetAllAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal(rolUsuarios, result.Value);
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnSuccess()
    {
        var rolUsuarios = new List<RolUsuarioResponse> { new RolUsuarioResponse { IdUsuario = 1, IdRol = 1 } };
        _repositoryMock.Setup(x => x.GetByUserIdAsync(1)).ReturnsAsync(rolUsuarios);

        var result = await _service.GetByUserIdAsync(1);

        Assert.True(result.IsSuccess);
        Assert.Equal(rolUsuarios, result.Value);
    }

    [Fact]
    public async Task AssignRoleAsync_ShouldReturnSuccess_WhenValid()
    {
        var request = new CreateRolUsuarioRequest { IdUsuario = 1, IdRol = 1 };
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var result = await _service.AssignRoleAsync(request);

        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(x => x.AddAsync(request), Times.Once);
    }

    [Fact]
    public async Task AssignRoleAsync_ShouldReturnFailure_WhenInvalid()
    {
        var request = new CreateRolUsuarioRequest();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("IdUsuario", "Required") });
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var result = await _service.AssignRoleAsync(request);

        Assert.False(result.IsSuccess);
        Assert.Contains("Required", result.Error);
    }

    [Fact]
    public async Task RemoveRoleAsync_ShouldReturnSuccess()
    {
        var result = await _service.RemoveRoleAsync(1, 1);

        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(x => x.DeleteAsync(1, 1), Times.Once);
    }
}