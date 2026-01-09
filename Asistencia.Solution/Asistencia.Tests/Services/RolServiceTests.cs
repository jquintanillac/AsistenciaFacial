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

public class RolServiceTests
{
    private readonly Mock<IRolRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateRolRequest>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateRolRequest>> _updateValidatorMock;
    private readonly RolService _service;

    public RolServiceTests()
    {
        _repositoryMock = new Mock<IRolRepository>();
        _createValidatorMock = new Mock<IValidator<CreateRolRequest>>();
        _updateValidatorMock = new Mock<IValidator<UpdateRolRequest>>();
        
        _service = new RolService(
            _repositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnSuccess_WhenRolesExist()
    {
        var roles = new List<RolResponse> { new RolResponse { IdRol = 1, Nombre = "Admin" } };
        _repositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(roles);

        var result = await _service.GetAllAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal(roles, result.Value);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenValid()
    {
        var request = new CreateRolRequest { Nombre = "Admin" };
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.AddAsync(request)).ReturnsAsync(1);

        var result = await _service.CreateAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
    }
}
