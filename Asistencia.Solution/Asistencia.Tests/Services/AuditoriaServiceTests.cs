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

public class AuditoriaServiceTests
{
    private readonly Mock<IAuditoriaRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateAuditoriaRequest>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateAuditoriaRequest>> _updateValidatorMock;
    private readonly AuditoriaService _service;

    public AuditoriaServiceTests()
    {
        _repositoryMock = new Mock<IAuditoriaRepository>();
        _createValidatorMock = new Mock<IValidator<CreateAuditoriaRequest>>();
        _updateValidatorMock = new Mock<IValidator<UpdateAuditoriaRequest>>();
        
        _service = new AuditoriaService(
            _repositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnSuccess_WhenAuditoriasExist()
    {
        var auditorias = new List<AuditoriaResponse> { new AuditoriaResponse { IdAuditoria = 1, Accion = "Test" } };
        _repositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(auditorias);

        var result = await _service.GetAllAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal(auditorias, result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSuccess_WhenExists()
    {
        var auditoria = new AuditoriaResponse { IdAuditoria = 1, Accion = "Test" };
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(auditoria);

        var result = await _service.GetByIdAsync(1);

        Assert.True(result.IsSuccess);
        Assert.Equal(auditoria, result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenNotFound()
    {
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((AuditoriaResponse?)null);

        var result = await _service.GetByIdAsync(1);

        Assert.False(result.IsSuccess);
        Assert.Equal("Auditoria not found", result.Error);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenValid()
    {
        var request = new CreateAuditoriaRequest { Accion = "Test" };
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
        var request = new CreateAuditoriaRequest();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("Accion", "Required") });
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var result = await _service.CreateAsync(request);

        Assert.False(result.IsSuccess);
        Assert.Contains("Required", result.Error);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnSuccess_WhenValidAndExists()
    {
        var request = new UpdateAuditoriaRequest { IdAuditoria = 1, Accion = "Updated" };
        _updateValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new AuditoriaResponse());

        var result = await _service.UpdateAsync(request);

        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(x => x.UpdateAsync(request), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenExists()
    {
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new AuditoriaResponse());

        var result = await _service.DeleteAsync(1);

        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(x => x.DeleteAsync(1), Times.Once);
    }
}