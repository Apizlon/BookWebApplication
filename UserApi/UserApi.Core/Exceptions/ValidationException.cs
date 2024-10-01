namespace UserApi.Core.Exceptions;

public class ValidationException(string message) : BadRequestException(message);