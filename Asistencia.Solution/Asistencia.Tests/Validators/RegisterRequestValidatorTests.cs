using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Asistencia.Tests.Validators;

public class RegisterRequestValidatorTests
{
    private readonly RegisterRequestValidator _validator;

    public RegisterRequestValidatorTests()
    {
        _validator = new RegisterRequestValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Username_Is_Empty()
    {
        var model = new RegisterRequest { Username = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new RegisterRequest { Email = "invalid-email" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Weak()
    {
        var model = new RegisterRequest { Password = "weak" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Password_Is_Strong()
    {
        var model = new RegisterRequest 
        { 
            Password = "StrongPassword1!", 
            ConfirmPassword = "StrongPassword1!" 
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Passwords_Do_Not_Match()
    {
        var model = new RegisterRequest 
        { 
            Password = "Password1!", 
            ConfirmPassword = "Password2!" 
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
    }
}
