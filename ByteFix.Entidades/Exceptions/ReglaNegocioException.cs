namespace ByteFix.Entidades.Exceptions;

/// <summary>
/// Excepción de negocio. El controlador la traduce a HTTP 409 Conflict.
/// </summary>
public class ReglaNegocioException : Exception
{
    public ReglaNegocioException(string mensaje) : base(mensaje)
    {
    }
}
