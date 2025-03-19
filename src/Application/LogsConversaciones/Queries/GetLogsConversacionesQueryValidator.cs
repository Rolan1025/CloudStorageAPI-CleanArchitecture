using FluentValidation;
using System.Globalization;

namespace CloudStorageAPICleanArchitecture.Application.LogsConversaciones.Queries;

public class GetLogsConversacionesQueryValidator : AbstractValidator<GetLogsConversacionesQuery>
{
    public GetLogsConversacionesQueryValidator()
    {
        RuleFor(x => x.PartitionKey)
            .NotEmpty().WithMessage("El Documento de Identidad es obligatorio.")
            .MinimumLength(7).WithMessage("El Documento de Identidad debe tener al menos 7 caracteres.")
            .Matches("^[0-9]+$").WithMessage("El Documento de Identidad debe contener únicamente números.");

        RuleFor(x => x.FechaDesde)
            .Must(BeValidDateFormat)
            .When(x => !string.IsNullOrEmpty(x.FechaDesde))
            .WithMessage("El formato de fechaDesde es incorrecto. Usa 'yyyy-MM-ddTHH:mm:ss'. Ejemplo: '2025-02-19T16:05:58'.");

        RuleFor(x => x.FechaHasta)
            .Must(BeValidDateFormat)
            .When(x => !string.IsNullOrEmpty(x.FechaHasta))
            .WithMessage("El formato de fechaHasta es incorrecto. Usa 'yyyy-MM-ddTHH:mm:ss'. Ejemplo: '2025-02-19T16:07:00'.");

        RuleFor(x => x)
            .Must(x => BeValidRange(x.FechaDesde, x.FechaHasta))
            .When(x => !string.IsNullOrEmpty(x.FechaDesde) && !string.IsNullOrEmpty(x.FechaHasta))
            .WithMessage("La fechaDesde no puede ser mayor que la fechaHasta.");
    }

    private static bool BeValidDateFormat(string? date)
    {
        return DateTime.TryParseExact(date, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }

    private static bool BeValidRange(string? fechaDesde, string? fechaHasta)
    {
        if (DateTime.TryParseExact(fechaDesde, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var desde) &&
            DateTime.TryParseExact(fechaHasta, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var hasta))
        {
            return desde <= hasta;
        }

        return true; // Si una fecha no es válida, el formato individual fallará antes
    }
}
