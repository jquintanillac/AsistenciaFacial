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

public class HorarioEmpleadoServiceTests
{
    private readonly Mock<IHorarioEmpleadoRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateHorarioEmpleadoRequest>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateHorarioEmpleadoRequest>> _updateValidatorMock;
    private readonly HorarioEmpleadoService _service;

    public HorarioEmpleadoServiceTests()
    {
        _repositoryMock = new Mock<IHorarioEmpleadoRepository>();
        _createValidatorMock = new Mock<IValidator<CreateHorarioEmpleadoRequest>>();
        _updateValidatorMock = new Mock<IValidator<UpdateHorarioEmpleadoRequest>>();
        
        _service = new HorarioEmpleadoService(
            _repositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnSuccess_WhenHorarioEmpleadosExist()
    {
        var horarioEmpleados = new List<HorarioEmpleadoResponse> { new HorarioEmpleadoResponse { IdHorarioEmpleado = 1 } };
        _repositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(horarioEmpleados);

        var result = await _service.GetAllAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal(horarioEmpleados, result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSuccess_WhenExists()
    {
        var horarioEmpleado = new HorarioEmpleadoResponse { IdHorarioEmpleado = 1 };
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(horarioEmpleado);

        var result = await _service.GetByIdAsync(1);

        Assert.True(result.IsSuccess);
        Assert.Equal(horarioEmpleado, result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenNotFound()
    {
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((HorarioEmpleadoResponse?)null);

        var result = await _service.GetByIdAsync(1);

        Assert.False(result.IsSuccess);
        Assert.Equal("HorarioEmpleado not found", result.Error);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenValid()
    {
        var request = new CreateHorarioEmpleadoRequest { IdEmpleado = 1, IdHorario = 1 };
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
        var request = new CreateHorarioEmpleadoRequest();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("IdEmpleado", "Required") });
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var result = await _service.CreateAsync(request);

        Assert.False(result.IsSuccess);
        Assert.Contains("Required", result.Error);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnSuccess_WhenValidAndExists()
    {
        var request = new UpdateHorarioEmpleadoRequest { IdHorarioEmpleado = 1, IdEmpleado = 1, IdHorario = 1 };
        _updateValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new HorarioEmpleadoResponse());

        var result = await _service.UpdateAsync(request);

        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(x => x.UpdateAsync(request), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenExists()
    {
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new HorarioEmpleadoResponse());

        var result = await _service.DeleteAsync(1);

        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(x => x.DeleteAsync(1), Times.Once);
    }
}