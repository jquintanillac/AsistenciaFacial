using Asistencia.Application.Common;
using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs;
using FluentValidation;

namespace Asistencia.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly IValidator<EmployeeDto> _validator;

    public EmployeeService(IEmployeeRepository repository, IValidator<EmployeeDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<IEnumerable<EmployeeDto>>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        return Result<IEnumerable<EmployeeDto>>.Success(result);
    }

    public async Task<Result<EmployeeDto>> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null) return Result<EmployeeDto>.Failure("Employee not found");
        return Result<EmployeeDto>.Success(result);
    }

    public async Task<Result<int>> CreateAsync(EmployeeDto employee)
    {
        var validationResult = await _validator.ValidateAsync(employee);
        if (!validationResult.IsValid)
            return Result<int>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var id = await _repository.AddAsync(employee);
        return Result<int>.Success(id);
    }
}
