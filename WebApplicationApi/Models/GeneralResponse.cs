﻿namespace WebApplicationApi.Models
{
    public class GeneralResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Error { get; set; }
        public object? Data { get; set; }
    }
}
