﻿namespace UserApi.Core.Exceptions;

public abstract class NotFoundException(string message) : Exception(message);