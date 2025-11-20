using DanceWaves.Models;

namespace DanceWaves.Extensions;

/// <summary>
/// Extension methods for enums to get display descriptions
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the display description for CompetitionStatus enum
    /// </summary>
    public static string GetDescription(this CompetitionStatus status)
    {
        return status switch
        {
            CompetitionStatus.OpenForRegistration => "Aberto para Registro",
            CompetitionStatus.Closed => "Fechado",
            CompetitionStatus.Completed => "Concluído",
            _ => status.ToString()
        };
    }

    /// <summary>
    /// Gets the display description for EntryStatus enum (non-nullable)
    /// </summary>
    public static string GetDescription(this EntryStatus status)
    {
        return status switch
        {
            EntryStatus.Pending => "Pendente",
            EntryStatus.Accepted => "Aceito",
            EntryStatus.NotAccepted => "Não Aceito",
            _ => status.ToString()
        };
    }

    /// <summary>
    /// Gets the display description for EntryStatus enum (nullable)
    /// </summary>
    public static string GetDescription(this EntryStatus? status)
    {
        if (!status.HasValue)
            return string.Empty;

        return status.Value.GetDescription();
    }

    /// <summary>
    /// Gets the display description for PaymentStatus enum (non-nullable)
    /// </summary>
    public static string GetDescription(this PaymentStatus status)
    {
        return status switch
        {
            PaymentStatus.Pending => "Pendente",
            PaymentStatus.Paid => "Pago",
            PaymentStatus.Failed => "Falhou",
            _ => status.ToString()
        };
    }

    /// <summary>
    /// Gets the display description for PaymentStatus enum (nullable)
    /// </summary>
    public static string GetDescription(this PaymentStatus? status)
    {
        if (!status.HasValue)
            return string.Empty;

        return status.Value.GetDescription();
    }
}
