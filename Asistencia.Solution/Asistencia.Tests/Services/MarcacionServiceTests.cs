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

public class MarcacionServiceTests
{
    private readonly Mock<IMarcacionRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateMarcacionRequest>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateMarcacionRequest>> _updateValidatorMock;
    private readonly MarcacionService _service;

    public MarcacionServiceTests()
    {
        _repositoryMock = new Mock<IMarcacionRepository>();
        _createValidatorMock = new Mock<IValidator<CreateMarcacionRequest>>();
        _updateValidatorMock = new Mock<IValidator<UpdateMarcacionRequest>>();
        
        _service = new MarcacionService(
            _repositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenValid()
    {
        var request = new CreateMarcacionRequest { IdEmpleado = 1 };
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.AddAsync(request)).ReturnsAsync(100);

        var result = await _service.CreateAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(100, result.Value);
    }
}
