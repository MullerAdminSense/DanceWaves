using DanceWaves.Application.Dtos;
using DanceWaves.Application.Ports;

namespace DanceWaves.Application.UseCases;

public class LoginUseCase(IAuthenticationPort authenticationPort)
{
    private readonly IAuthenticationPort _authenticationPort = authenticationPort;

    public async Task<AuthenticationResponse> ExecuteAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "Email and password are required"
            };
        }

        return await _authenticationPort.LoginAsync(request);
    }
}

public class RegisterUseCase
{
    private readonly IAuthenticationPort _authenticationPort;

    public RegisterUseCase(IAuthenticationPort authenticationPort)
    {
        _authenticationPort = authenticationPort;
    }

    public async Task<AuthenticationResponse> ExecuteAsync(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || 
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.FirstName) ||
            string.IsNullOrWhiteSpace(request.LastName))
        {
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "All required fields must be provided"
            };
        }

        if (request.Password != request.ConfirmPassword)
        {
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "Passwords do not match"
            };
        }

        if (request.Password.Length < 8)
        {
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "Password must be at least 8 characters long"
            };
        }

        if (!request.AcceptTerms)
        {
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "You must accept the terms and conditions"
            };
        }

        return await _authenticationPort.RegisterAsync(request);
    }
}

public class FederatedLoginUseCase
{
    private readonly IAuthenticationPort _authenticationPort;

    public FederatedLoginUseCase(IAuthenticationPort authenticationPort)
    {
        _authenticationPort = authenticationPort;
    }

    public async Task<AuthenticationResponse> ExecuteAsync(FederatedLoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Provider) || 
            string.IsNullOrWhiteSpace(request.ExternalUserId) ||
            string.IsNullOrWhiteSpace(request.Email))
        {
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "Provider, external user ID, and email are required"
            };
        }

        return await _authenticationPort.FederatedLoginAsync(request);
    }
}

public class GetCurrentUserUseCase
{
    private readonly IAuthenticationPort _authenticationPort;

    public GetCurrentUserUseCase(IAuthenticationPort authenticationPort)
    {
        _authenticationPort = authenticationPort;
    }

    public async Task<UserDto?> ExecuteAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return null;

        return await _authenticationPort.GetCurrentUserAsync(userId);
    }
}

public class UpdateProfileUseCase
{
    private readonly IAuthenticationPort _authenticationPort;

    public UpdateProfileUseCase(IAuthenticationPort authenticationPort)
    {
        _authenticationPort = authenticationPort;
    }

    public async Task<AuthenticationResponse> ExecuteAsync(string userId, UserDto updatedProfile)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "User ID is required"
            };
        }

        return await _authenticationPort.UpdateProfileAsync(userId, updatedProfile);
    }
}

public class ChangePasswordUseCase
{
    private readonly IAuthenticationPort _authenticationPort;

    public ChangePasswordUseCase(IAuthenticationPort authenticationPort)
    {
        _authenticationPort = authenticationPort;
    }

    public async Task<AuthenticationResponse> ExecuteAsync(string userId, ChangePasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "User ID is required"
            };
        }

        if (request.NewPassword != request.ConfirmPassword)
        {
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "Passwords do not match"
            };
        }

        if (request.NewPassword.Length < 8)
        {
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "Password must be at least 8 characters long"
            };
        }

        return await _authenticationPort.ChangePasswordAsync(userId, request);
    }
}
