﻿namespace GeoDistance.Api.Exceptions;

public class ConfigurationException : ApplicationException
{
    public ConfigurationException(string message) : base(message)
    {
    }
}