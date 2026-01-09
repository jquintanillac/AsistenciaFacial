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

public class ConfiguracionServiceTests
{
    private readonly Mock<IConfiguracionRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateConfiguracionRequest>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateConfiguracionRequest>> _updateValidatorMock;
    private readonly ConfiguracionService _service;

    public ConfiguracionServiceTests()
    {
        _repositoryMock = new Mock<IConfiguracionRepository>();
        _createValidatorMock = new Mock<IValidator<CreateConfiguracionRequest>>();
        _updateValidatorMock = new Mock<IValidator<UpdateConfiguracionRequest>>();
        
        _service = new ConfiguracionService(
            _repositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnSuccess_WhenConfiguracionesExist()
    {
        var configuraciones = new List<ConfiguracionResponse> { new ConfiguracionResponse { IdConfiguracion = 1, Clave = "Test", Valor = "Value" } };
        _repositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(configuraciones);

        var result = await _service.GetAllAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal(configuraciones, result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSuccess_WhenExists()
    {
        var configuracion = new ConfiguracionResponse { IdConfiguracion = 1, Clave = "Test", Valor = "Value" };
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(configuracion);

        var result = await _service.GetByIdAsync(1);

        Assert.True(result.IsSuccess);
        Assert.Equal(configuracion, result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenNotFound()
    {
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((ConfiguracionResponse?)null);

        var result = await _service.GetByIdAsync(1);

        Assert.False(result.IsSuccess);
        Assert.Equal("Configuracion not found", result.Error);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenValid()
    {
        var request = new CreateConfiguracionRequest { Clave = "Test", Valor = "Value" };
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
        var request = new CreateConfiguracionRequest();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("Clave", "Required") });
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var result = await _service.CreateAsync(request);

        Assert.False(result.IsSuccess);
        Assert.Contains("Required", result.Error);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnSuccess_WhenValidAndExists()
    {
        var request = new UpdateConfiguracionRequest { IdConfiguracion = 1, Clave = "Updated", Valor = "Value" };
        _updateValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new ConfiguracionResponse());

        var result = await _service.UpdateAsync(request);

        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(x => x.UpdateAsync(request), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenExists()
    {
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new ConfiguracionResponse());

        var result = await _service.DeleteAsync(1);

        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(x => x.DeleteAsync(1), Times.Once);
    }
}