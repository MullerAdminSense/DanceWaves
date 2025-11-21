using System.Collections.Generic;

namespace DanceWaves.Application.Dtos;

/// <summary>
/// DTO simples para expor opções de países à camada de apresentação.
/// </summary>
public record CountryOptionDto(int Id, string Name);

/// <summary>
/// Catálogo imutável com as opções suportadas pela aplicação.
/// Permite que as Razor Pages dependam apenas de DTOs, preservando a arquitetura hexagonal.
/// </summary>
public static class CountryCatalog
{
    public static readonly IReadOnlyList<CountryOptionDto> Defaults = new List<CountryOptionDto>
    {
        new(1, "United States"),
        new(2, "Netherlands"),
        new(3, "France"),
        new(4, "Germany"),
    };
}

