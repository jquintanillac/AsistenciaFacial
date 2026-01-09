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

public class HorarioServiceTests
{
    private readonly Mock<IHorarioRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateHorarioRequest>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateHorarioRequest>> _updateValidatorMock;
    private readonly HorarioService _service;

    public HorarioServiceTests()
    {
        _repositoryMock = new Mock<IHorarioRepository>();
        _createValidatorMock = new Mock<IValidator<CreateHorarioRequest>>();
        _updateValidatorMock = new Mock<IValidator<UpdateHorarioRequest>>();
        
        _service = new HorarioService(
            _repositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenValid()
    {
        var request = new CreateHorarioRequest { Nombre = "Turno Mañana" };
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.AddAsync(request)).ReturnsAsync(1);

        var result = await _service.CreateAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
    }
}
