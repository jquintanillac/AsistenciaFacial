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

public class UbicacionPermitidaServiceTests
{
    private readonly Mock<IUbicacionPermitidaRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateUbicacionPermitidaRequest>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateUbicacionPermitidaRequest>> _updateValidatorMock;
    private readonly UbicacionPermitidaService _service;

    public UbicacionPermitidaServiceTests()
    {
        _repositoryMock = new Mock<IUbicacionPermitidaRepository>();
        _createValidatorMock = new Mock<IValidator<CreateUbicacionPermitidaRequest>>();
        _updateValidatorMock = new Mock<IValidator<UpdateUbicacionPermitidaRequest>>();
        
        _service = new UbicacionPermitidaService(
            _repositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnSuccess_WhenUbicacionesExist()
    {
        var ubicaciones = new List<UbicacionPermitidaResponse> { new UbicacionPermitidaResponse { IdUbicacion = 1, Nombre = "Oficina" } };
        _repositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(ubicaciones);

        var result = await _service.GetAllAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal(ubicaciones, result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSuccess_WhenExists()
    {
        var ubicacion = new UbicacionPermitidaResponse { IdUbicacion = 1, Nombre = "Oficina" };
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(ubicacion);

        var result = await _service.GetByIdAsync(1);

        Assert.True(result.IsSuccess);
        Assert.Equal(ubicacion, result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenNotFound()
    {
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((UbicacionPermitidaResponse?)null);

        var result = await _service.GetByIdAsync(1);

        Assert.False(result.IsSuccess);
        Assert.Equal("UbicacionPermitida not found", result.Error);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenValid()
    {
        var request = new CreateUbicacionPermitidaRequest { Nombre = "Oficina", Latitud = -12.0m, Longitud = -77.0m, RadioMetros = 100 };
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
        var request = new CreateUbicacionPermitidaRequest();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("Nombre", "Required") });
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var result = await _service.CreateAsync(request);

        Assert.False(result.IsSuccess);
        Assert.Contains("Required", result.Error);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnSuccess_WhenValidAndExists()
    {
        var request = new UpdateUbicacionPermitidaRequest { IdUbicacion = 1, Nombre = "Oficina Updated", Latitud = -12.0m, Longitud = -77.0m, RadioMetros = 100 };
        _updateValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new UbicacionPermitidaResponse());

        var result = await _service.UpdateAsync(request);

        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(x => x.UpdateAsync(request), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenExists()
    {
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new UbicacionPermitidaResponse());

        var result = await _service.DeleteAsync(1);

        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(x => x.DeleteAsync(1), Times.Once);
    }
}