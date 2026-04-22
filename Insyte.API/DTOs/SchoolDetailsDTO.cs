using Insyte.Core.Enums;

namespace Insyte.API.DTOs;

public record SchoolFacilityDto(
    Guid Id,
    Guid SchoolId,
    PhysicalFacility Facility,
    DateTime CreatedAt
);

public record AddSchoolFacilityRequest(PhysicalFacility Facility);

public record SchoolServiceDto(
    Guid Id,
    Guid SchoolId,
    SchoolService Service,
    DateTime CreatedAt
);

public record AddSchoolServiceRequest(SchoolService Service);

public record SchoolActivityDto(
    Guid Id,
    Guid SchoolId,
    Activity Activity,
    DateTime CreatedAt
);

public record AddSchoolActivityRequest(Activity Activity);

public record SchoolLanguageDto(
    Guid Id,
    Guid SchoolId,
    ForeignLanguage Language,
    DateTime CreatedAt
);

public record AddSchoolLanguageRequest(ForeignLanguage Language);

public record SchoolDetailsResponseDto(
    Guid Id,
    string Name,
    SchoolType SchoolType,
    InstitutionType InstitutionType,
    LiseType? LiseType,
    EducationSystem? EducationSystem,
    List<SchoolFacilityDto> Facilities,
    List<SchoolServiceDto> Services,
    List<SchoolActivityDto> Activities,
    List<SchoolLanguageDto> Languages
);
