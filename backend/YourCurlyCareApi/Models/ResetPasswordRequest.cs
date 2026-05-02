using System;

namespace YourCurlyCareApi.Models;

public class ResetPasswordRequest
{
    public string Email { get; set; }= null!;
    public string Codigo { get; set; }= null!;
    public string NuevaPassword { get; set; }= null!;
}

public class OlvideRequest
{
    public string Email { get; set; } = null!;
}