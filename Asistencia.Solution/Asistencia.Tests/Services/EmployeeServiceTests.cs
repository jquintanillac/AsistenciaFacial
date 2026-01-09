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

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateEmployeeRequest>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateEmployeeRequest>> _updateValidatorMock;
    private readonly EmployeeService _service;

    public EmployeeServiceTests()
    {
        _repositoryMock = new Mock<IEmployeeRepository>();
        _createValidatorMock = new Mock<IValidator<CreateEmployeeRequest>>();
        _updateValidatorMock = new Mock<IValidator<UpdateEmployeeRequest>>();
        
        _service = new EmployeeService(
            _repositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnSuccess_WhenEmployeesExist()
    {
        // Arrange
        var employees = new List<EmployeeResponse> { new EmployeeResponse { IdEmpleado = 1, Nombres = "Test" } };
        _repositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(employees);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(employees, result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenEmployeeNotFound()
    {
        // Arrange
        _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((EmployeeResponse?)null);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Employee not found", result.Error);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSuccess_WhenEmployeeExists()
    {
        // Arrange
        var employee = new EmployeeResponse { IdEmpleado = 1, Nombres = "Test" };
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(employee);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(employee, result.Value);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var request = new CreateEmployeeRequest();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("Nombres", "Nombres is required") });
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Nombres is required", result.Error);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenValidationPasses()
    {
        // Arrange
        var request = new CreateEmployeeRequest { Nombres = "Test" };
        _createValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.AddAsync(request)).ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var request = new UpdateEmployeeRequest { IdEmpleado = 1 };
        var validationResult = new ValidationResult(new[] { new ValidationFailure("Nombres", "Error") });
        _updateValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.UpdateAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFailure_WhenEmployeeNotFound()
    {
        // Arrange
        var request = new UpdateEmployeeRequest { IdEmpleado = 1 };
        _updateValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.GetByIdAsync(request.IdEmpleado)).ReturnsAsync((EmployeeResponse?)null);

        // Act
        var result = await _service.UpdateAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Employee not found", result.Error);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnSuccess_WhenValid()
    {
        // Arrange
        var request = new UpdateEmployeeRequest { IdEmpleado = 1 };
        _updateValidatorMock.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(x => x.GetByIdAsync(request.IdEmpleado)).ReturnsAsync(new EmployeeResponse());
        _repositoryMock.Setup(x => x.UpdateAsync(request)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdateAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFailure_WhenEmployeeNotFound()
    {
        // Arrange
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((EmployeeResponse?)null);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Employee not found", result.Error);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenEmployeeExists()
    {
        // Arrange
        _repositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new EmployeeResponse());
        _repositoryMock.Setup(x => x.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
