using RaceDay.Api.Models;

namespace RaceDay.Api.DTOs;

public record EventDto(
    string Name,
    string Description,
    DateTime EventDate,
    string Location,
    decimal Distance,
    EventType EventType);