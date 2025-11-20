using DanceWaves.Models;
using DanceWaves.Application.Dtos;

namespace DanceWaves.Adapters.Persistence.Mappers;

/// <summary>
/// Mapper para converter entre Models (EF) e DTOs
/// </summary>
public static class ModelToDtoMapper
{
    // Entry
    public static EntrySimpleDto ToDto(Entry model)
    {
        if (model == null) return null!;
        
        return new EntrySimpleDto
        {
            Id = model.Id,
            CompetitionCategoryId = model.CompetitionCategoryId,
            StartNumber = model.StartNumber,
            StartTime = model.StartTime,
            SchoolId = model.SchoolId,
            Status = model.Status,
            PaymentStatus = model.PaymentStatus,
            Song = model.Song,
            DurationSeconds = model.DurationSeconds
        };
    }

    public static Entry ToModel(EntrySimpleDto dto)
    {
        if (dto == null) return null!;
        
        return new Entry
        {
            Id = dto.Id,
            CompetitionCategoryId = dto.CompetitionCategoryId,
            StartNumber = dto.StartNumber,
            StartTime = dto.StartTime,
            SchoolId = dto.SchoolId,
            Status = dto.Status,
            PaymentStatus = dto.PaymentStatus,
            Song = dto.Song,
            DurationSeconds = dto.DurationSeconds
        };
    }

    // DanceSchool
    public static DanceSchoolDto ToDto(DanceSchool model)
    {
        if (model == null) return null!;
        
        return new DanceSchoolDto
        {
            Id = model.Id,
            LegalName = model.LegalName,
            Address = model.Address,
            City = model.City,
            Zip = model.Zip,
            Province = model.Province,
            VatNumber = model.VatNumber,
            IsPartOfEU = model.IsPartOfEU,
            Email = model.Email,
            DefaultFranchiseId = model.DefaultFranchiseId,
            CountryId = model.CountryId
        };
    }

    public static DanceSchool ToModel(DanceSchoolDto dto)
    {
        if (dto == null) return null!;
        
        return new DanceSchool
        {
            Id = dto.Id,
            LegalName = dto.LegalName,
            Address = dto.Address,
            City = dto.City,
            Zip = dto.Zip,
            Province = dto.Province,
            VatNumber = dto.VatNumber,
            IsPartOfEU = dto.IsPartOfEU,
            Email = dto.Email,
            DefaultFranchiseId = dto.DefaultFranchiseId,
            CountryId = dto.CountryId
        };
    }

    // User
    public static UserSimpleDto ToDto(User model)
    {
        if (model == null) return null!;
        
        return new UserSimpleDto
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Address = model.Address,
            City = model.City,
            Zip = model.Zip,
            Province = model.Province,
            CountryId = model.CountryId,
            Email = model.Email,
            DanceSchoolId = model.DanceSchoolId,
            Phone = model.Phone,
            DefaultFranchiseId = model.DefaultFranchiseId,
            AgeGroupId = model.AgeGroupId,
            RolePermissionId = model.RolePermissionId,
            Password = null // Nunca expor senha armazenada
        };
    }

    public static User ToModel(UserSimpleDto dto)
    {
        if (dto == null) return null!;
        
        return new User
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Address = dto.Address,
            City = dto.City,
            Zip = dto.Zip,
            Province = dto.Province,
            CountryId = dto.CountryId,
            Email = dto.Email,
            DanceSchoolId = dto.DanceSchoolId,
            Phone = dto.Phone,
            DefaultFranchiseId = dto.DefaultFranchiseId,
            AgeGroupId = dto.AgeGroupId,
            RolePermissionId = dto.RolePermissionId,
            Password = dto.Password // Incluir senha ao converter para Model quando fornecida
        };
    }

    // Competition
    public static CompetitionDto ToDto(Competition model)
    {
        if (model == null) return null!;
        
        return new CompetitionDto
        {
            Id = model.Id,
            Name = model.Name,
            Location = model.Location,
            Venue = model.Venue,
            Status = model.Status,
            GeoPoints = model.GeoPoints,
            MaxContestants = model.MaxContestants,
            RegistrationsOpenForMembers = model.RegistrationsOpenForMembers,
            RegistrationsOpenForEveryone = model.RegistrationsOpenForEveryone,
            CheckInUntil = model.CheckInUntil
        };
    }

    public static Competition ToModel(CompetitionDto dto)
    {
        if (dto == null) return null!;
        
        return new Competition
        {
            Id = dto.Id,
            Name = dto.Name,
            Location = dto.Location,
            Venue = dto.Venue,
            Status = dto.Status,
            GeoPoints = dto.GeoPoints,
            MaxContestants = dto.MaxContestants,
            RegistrationsOpenForMembers = dto.RegistrationsOpenForMembers,
            RegistrationsOpenForEveryone = dto.RegistrationsOpenForEveryone,
            CheckInUntil = dto.CheckInUntil
        };
    }

    // Style
    public static StyleDto ToDto(Style model)
    {
        if (model == null) return null!;
        
        return new StyleDto
        {
            Id = model.Id,
            Code = model.Code ?? string.Empty,
            Name = model.Name ?? string.Empty
        };
    }

    public static Style ToModel(StyleDto dto)
    {
        if (dto == null) return null!;
        
        return new Style
        {
            Id = dto.Id,
            Code = dto.Code,
            Name = dto.Name
        };
    }

    // AgeGroup
    public static AgeGroupDto ToDto(AgeGroup model)
    {
        if (model == null) return null!;
        
        return new AgeGroupDto
        {
            Id = model.Id,
            Code = model.Code ?? string.Empty,
            Name = model.Name ?? string.Empty,
            MinAge = model.MinAge,
            MaxAge = model.MaxAge
        };
    }

    public static AgeGroup ToModel(AgeGroupDto dto)
    {
        if (dto == null) return null!;
        
        return new AgeGroup
        {
            Id = dto.Id,
            Code = dto.Code,
            Name = dto.Name,
            MinAge = dto.MinAge,
            MaxAge = dto.MaxAge
        };
    }

    // Level
    public static LevelDto ToDto(Level model)
    {
        if (model == null) return null!;
        
        return new LevelDto
        {
            Id = model.Id,
            Code = model.Code ?? string.Empty,
            Name = model.Name ?? string.Empty
        };
    }

    public static Level ToModel(LevelDto dto)
    {
        if (dto == null) return null!;
        
        return new Level
        {
            Id = dto.Id,
            Code = dto.Code,
            Name = dto.Name
        };
    }

    // EntryType
    public static EntryTypeDto ToDto(EntryType model)
    {
        if (model == null) return null!;
        
        return new EntryTypeDto
        {
            Id = model.Id,
            Name = model.Name ?? string.Empty,
            NumberOfDancers = model.NumberOfDancers
        };
    }

    public static EntryType ToModel(EntryTypeDto dto)
    {
        if (dto == null) return null!;
        
        return new EntryType
        {
            Id = dto.Id,
            Name = dto.Name,
            NumberOfDancers = dto.NumberOfDancers
        };
    }

    // Score
    public static ScoreDto ToDto(Score model)
    {
        if (model == null) return null!;
        
        return new ScoreDto
        {
            Id = model.Id,
            JudgeUserId = model.JudgeUserId,
            EntryId = model.EntryId,
            RawScore = model.RawScore,
            Penalty = model.Penalty,
            Note = model.Note,
            SubmittedDate = model.SubmittedDate
        };
    }

    public static Score ToModel(ScoreDto dto)
    {
        if (dto == null) return null!;
        
        return new Score
        {
            Id = dto.Id,
            JudgeUserId = dto.JudgeUserId,
            EntryId = dto.EntryId,
            RawScore = dto.RawScore,
            Penalty = dto.Penalty,
            Note = dto.Note,
            SubmittedDate = dto.SubmittedDate
        };
    }

    // EntryMember
    public static EntryMemberDto ToDto(EntryMember model)
    {
        if (model == null) return null!;
        
        return new EntryMemberDto
        {
            Id = model.Id,
            EntryId = model.EntryId,
            UserId = model.UserId,
            PaymentStatus = model.PaymentStatus
        };
    }

    public static EntryMember ToModel(EntryMemberDto dto)
    {
        if (dto == null) return null!;
        
        return new EntryMember
        {
            Id = dto.Id,
            EntryId = dto.EntryId,
            UserId = dto.UserId,
            PaymentStatus = dto.PaymentStatus
        };
    }

    // Franchise
    public static FranchiseDto ToDto(Franchise model)
    {
        if (model == null) return null!;
        
        return new FranchiseDto
        {
            Id = model.Id,
            LegalName = model.LegalName,
            Address = model.Address,
            City = model.City,
            Zip = model.Zip,
            Province = model.Province,
            VatNumber = model.VatNumber,
            IsPartOfEU = model.IsPartOfEU,
            ContactEmail = model.ContactEmail,
            SystemEmail = model.SystemEmail,
            CountryId = model.CountryId
        };
    }

    public static Franchise ToModel(FranchiseDto dto)
    {
        if (dto == null) return null!;
        
        return new Franchise
        {
            Id = dto.Id,
            LegalName = dto.LegalName,
            Address = dto.Address,
            City = dto.City,
            Zip = dto.Zip,
            Province = dto.Province,
            VatNumber = dto.VatNumber,
            IsPartOfEU = dto.IsPartOfEU,
            ContactEmail = dto.ContactEmail,
            SystemEmail = dto.SystemEmail,
            CountryId = dto.CountryId
        };
    }

    // UserRolePermission
    public static UserRolePermissionDto ToDto(UserRolePermission model)
    {
        if (model == null) return null!;
        
        return new UserRolePermissionDto
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description
        };
    }

    public static UserRolePermission ToModel(UserRolePermissionDto dto)
    {
        if (dto == null) return null!;
        
        return new UserRolePermission
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        };
    }

    // CompetitionCategory
    public static CompetitionCategoryDto ToDto(CompetitionCategory model)
    {
        if (model == null) return null!;
        
        return new CompetitionCategoryDto
        {
            Id = model.Id,
            CompetitionId = model.CompetitionId,
            StyleId = model.StyleId,
            AgeGroupId = model.AgeGroupId,
            LevelId = model.LevelId,
            MinTeamSize = model.MinTeamSize,
            MaxTeamSize = model.MaxTeamSize,
            GenderMix = model.GenderMix,
            MaxMusicLengthSeconds = model.MaxMusicLengthSeconds,
            FeeAmount = model.FeeAmount,
            Capacity = model.Capacity
        };
    }

    public static CompetitionCategory ToModel(CompetitionCategoryDto dto)
    {
        if (dto == null) return null!;
        
        return new CompetitionCategory
        {
            Id = dto.Id,
            CompetitionId = dto.CompetitionId,
            StyleId = dto.StyleId,
            AgeGroupId = dto.AgeGroupId,
            LevelId = dto.LevelId,
            MinTeamSize = dto.MinTeamSize,
            MaxTeamSize = dto.MaxTeamSize,
            GenderMix = dto.GenderMix,
            MaxMusicLengthSeconds = dto.MaxMusicLengthSeconds,
            FeeAmount = dto.FeeAmount,
            Capacity = dto.Capacity
        };
    }
}
