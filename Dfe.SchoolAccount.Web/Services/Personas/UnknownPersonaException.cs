namespace Dfe.SchoolAccount.Web.Services.Personas;

/// <summary>
/// Occurs when attempting an operation with an unknown persona.
/// </summary>
public class UnknownPersonaException : InvalidOperationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnknownPersonaException"/> class.
    /// </summary>
    public UnknownPersonaException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnknownPersonaException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public UnknownPersonaException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnknownPersonaException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public UnknownPersonaException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
