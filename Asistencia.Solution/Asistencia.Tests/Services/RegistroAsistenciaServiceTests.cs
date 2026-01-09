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

public class RegistroAsistenciaServiceTests
{
    private readonly Mock<IRegistroAsistenciaRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateRegistroAsistenciaRequest>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateRegistroAsistenciaRequest>> _updateValidatorMock;
    private readonly RegistroAsistenciaService _service;

    public RegistroAsistenciaServiceTests()
    {
        _repositoryMock = new Mock<IRegistroAsistenciaRepository>();
        _createValidatorMock = new Mock<IValidator<CreateRegistroAsistenciaRequest>>();
        _updateValidatorMock = new Mock<IValidator<UpdateRegistroAsistenciaRequest>>();
        
        _service = new RegistroAsistenciaService(
            _repositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnSuccess_WhenRegistrosExist()
    {
        var registros = new List<RegistroAsistenciaResponse> { new RegistroAsistenciaResponse { IdRegistro = 1, EstadoAsistencia = "P" } };
        _repositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(registros);

        var result = await _service.GetAllAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal(registros, result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSuccess_WhenExists()
    {
        var registro = new RegistroAsistenciaResponse { IdRegistro = 1, EstadoAsistencia = "P" };
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(registro);

        var result = await _service.GetByIdAsync(1);

        Assert.True(result.IsSuccess);
        Assert.Equal(registro, result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenNotFound()
    {
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((RegistroAsistenciaResponse?)null);

        var result = await _service.GetByIdAsync(1);

        Assert.False(result.IsSuccess);
        Assert.Equal("RegistroAsistencia not found", result.Error);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenValid()
    {
        var request = new CreateRegistroAsistenciaRequest { IdEmpleado = 1, EstadoAsistencia = "P" };
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.AddAsync(request)).ReturnsAsync(1);

        var result = await _service.CreateAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnFailure_WhenInvalid()
    {
        var request = new CreateRegistroAsistenciaRequest();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("EstadoAsistencia", "Required") });
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var result = await _service.CreateAsync(request);

        Assert.False(result.IsSuccess);
        Assert.Contains("Required", result.Error);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnSuccess_WhenValidAndExists()
    {
        var request = new UpdateRegistroAsistenciaRequest { IdRegistro = 1, IdEmpleado = 1, EstadoAsistencia = "P" };
        _updateValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new RegistroAsistenciaResponse());

        var result = await _service.UpdateAsync(request);

        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(x => x.UpdateAsync(request), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenExists()
    {
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new RegistroAsistenciaResponse());

        var result = await _service.DeleteAsync(1);

        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(x => x.DeleteAsync(1), Times.Once);
    }
}