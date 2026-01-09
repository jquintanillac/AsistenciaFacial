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

public class EmpresaServiceTests
{
    private readonly Mock<IEmpresaRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateEmpresaRequest>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateEmpresaRequest>> _updateValidatorMock;
    private readonly EmpresaService _service;

    public EmpresaServiceTests()
    {
        _repositoryMock = new Mock<IEmpresaRepository>();
        _createValidatorMock = new Mock<IValidator<CreateEmpresaRequest>>();
        _updateValidatorMock = new Mock<IValidator<UpdateEmpresaRequest>>();
        
        _service = new EmpresaService(
            _repositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnSuccess_WhenEmpresasExist()
    {
        var empresas = new List<EmpresaResponse> { new EmpresaResponse { IdEmpresa = 1, RazonSocial = "Test" } };
        _repositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(empresas);

        var result = await _service.GetAllAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal(empresas, result.Value);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenValid()
    {
        var request = new CreateEmpresaRequest { RazonSocial = "Test" };
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.AddAsync(request)).ReturnsAsync(1);

        var result = await _service.CreateAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
    }
}
