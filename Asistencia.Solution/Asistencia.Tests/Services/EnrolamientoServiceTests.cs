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

public class EnrolamientoServiceTests
{
    private readonly Mock<IEnrolamientoRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateEnrolamientoRequest>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateEnrolamientoRequest>> _updateValidatorMock;
    private readonly EnrolamientoService _service;

    public EnrolamientoServiceTests()
    {
        _repositoryMock = new Mock<IEnrolamientoRepository>();
        _createValidatorMock = new Mock<IValidator<CreateEnrolamientoRequest>>();
        _updateValidatorMock = new Mock<IValidator<UpdateEnrolamientoRequest>>();
        
        _service = new EnrolamientoService(
            _repositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnSuccess_WhenEnrolamientosExist()
    {
        var enrolamientos = new List<EnrolamientoResponse> { new EnrolamientoResponse { IdEnrolamiento = 1 } };
        _repositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(enrolamientos);

        var result = await _service.GetAllAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal(enrolamientos, result.Value);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenValid()
    {
        var request = new CreateEnrolamientoRequest { IdEmpleado = 1 };
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.AddAsync(request)).ReturnsAsync(1);

        var result = await _service.CreateAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
    }
}
