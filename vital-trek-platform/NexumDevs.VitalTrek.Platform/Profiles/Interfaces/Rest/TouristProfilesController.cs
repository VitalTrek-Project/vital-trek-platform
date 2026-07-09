using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Profiles.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Profiles.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest.Resources;
using NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest.Transform;
using NexumDevs.VitalTrek.Platform.Profiles.Resources;
using Swashbuckle.AspNetCore.Annotations;
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

namespace NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest;

/// <summary>
/// A tourist's personal, travel and health profile. Medical fields and emergency contacts are
/// only ever returned from these detail endpoints — never from a list endpoint anywhere in the
/// platform. Self-access (<c>/tourists/{touristId}/...</c>) requires the caller to be that exact
/// tourist; agency-staff access (<c>/agencies/{agencyId}/tourists/{touristId}</c>) requires the
/// tourist to have a booking with that agency (enforced via TourManagement's ACL facade) and is
/// itself audited in <see cref="Domain.Model.Aggregates.MedicalDataAccessLog"/>.
/// </summary>
[ApiController]
[Route("api/v1/profiles")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available tourist profile endpoints")]
public class TouristProfilesController(
    ITouristProfileCommandService commandService,
    ITouristProfileQueryService queryService,
    IStringLocalizer<ProfilesMessages> localizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet("tourists/{touristId:guid}")]
    [SwaggerOperation(Summary = "Get a tourist's own profile", OperationId = "GetTouristProfile")]
    [SwaggerResponse(StatusCodes.Status200OK, "The profile was found", typeof(TouristProfileResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No profile exists yet for this tourist")]
    public async Task<IActionResult> GetProfile([FromRoute] Guid touristId, CancellationToken cancellationToken)
    {
        var profile = await queryService.Handle(new GetTouristProfileByUserIdQuery(touristId), cancellationToken);
        if (profile is null)
            return ProfilesActionResultAssembler.ToProblemDetails(
                this, new ProfilesError(Domain.Model.Errors.ProfilesErrors.ProfileNotFound), localizer, problemDetailsFactory);

        return Ok(TouristProfileResourceFromEntityAssembler.ToResourceFromEntity(profile));
    }

    [HttpPut("tourists/{touristId:guid}")]
    [SwaggerOperation(Summary = "Create or update a tourist's personal/travel data", OperationId = "UpdateTouristProfile")]
    [SwaggerResponse(StatusCodes.Status200OK, "The profile was saved", typeof(TouristProfileResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid profile data")]
    public async Task<IActionResult> UpdateProfile(
        [FromRoute] Guid touristId, [FromBody] UpdateTouristProfileResource resource, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<ExperienceLevel>(resource.ExperienceLevel, true, out var experienceLevel))
            return InvalidEnum(nameof(resource.ExperienceLevel), resource.ExperienceLevel);

        try
        {
            var profile = await commandService.Handle(
                new UpdateTouristProfileCommand(
                    touristId, resource.FullName, resource.PhotoUrl, resource.DateOfBirth, resource.Nationality,
                    resource.PhoneNumber, resource.PreferredLanguage, experienceLevel),
                cancellationToken);
            return Ok(TouristProfileResourceFromEntityAssembler.ToResourceFromEntity(profile));
        }
        catch (ProfilesError error)
        {
            return ProfilesActionResultAssembler.ToProblemDetails(this, error, localizer, problemDetailsFactory);
        }
    }

    [HttpPut("tourists/{touristId:guid}/identity-document")]
    [SwaggerOperation(Summary = "Set a tourist's identity document", OperationId = "UpdateIdentityDocument")]
    [SwaggerResponse(StatusCodes.Status200OK, "The document was saved", typeof(TouristProfileResource))]
    public async Task<IActionResult> UpdateIdentityDocument(
        [FromRoute] Guid touristId, [FromBody] UpdateIdentityDocumentResource resource, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<IdentityDocumentType>(resource.Type, true, out var type))
            return InvalidEnum(nameof(resource.Type), resource.Type);

        try
        {
            var profile = await commandService.Handle(new UpdateIdentityDocumentCommand(touristId, type, resource.Number), cancellationToken);
            return Ok(TouristProfileResourceFromEntityAssembler.ToResourceFromEntity(profile));
        }
        catch (ProfilesError error)
        {
            return ProfilesActionResultAssembler.ToProblemDetails(this, error, localizer, problemDetailsFactory);
        }
    }

    [HttpPut("tourists/{touristId:guid}/medical-info")]
    [SwaggerOperation(
        Summary = "Set a tourist's optional medical information",
        Description = "Sensitive data — never included in any list response.",
        OperationId = "UpdateMedicalInfo")]
    [SwaggerResponse(StatusCodes.Status200OK, "The medical info was saved", typeof(TouristProfileResource))]
    public async Task<IActionResult> UpdateMedicalInfo(
        [FromRoute] Guid touristId, [FromBody] UpdateMedicalInfoResource resource, CancellationToken cancellationToken)
    {
        BloodType? bloodType = null;
        if (!string.IsNullOrWhiteSpace(resource.BloodType))
        {
            if (!Enum.TryParse<BloodType>(resource.BloodType, true, out var parsed))
                return InvalidEnum(nameof(resource.BloodType), resource.BloodType);
            bloodType = parsed;
        }

        try
        {
            var profile = await commandService.Handle(
                new UpdateMedicalInfoCommand(touristId, bloodType, resource.Allergies, resource.MedicalConditions, resource.Medications),
                cancellationToken);
            return Ok(TouristProfileResourceFromEntityAssembler.ToResourceFromEntity(profile));
        }
        catch (ProfilesError error)
        {
            return ProfilesActionResultAssembler.ToProblemDetails(this, error, localizer, problemDetailsFactory);
        }
    }

    [HttpPost("tourists/{touristId:guid}/emergency-contacts")]
    [SwaggerOperation(Summary = "Add an emergency contact", OperationId = "AddEmergencyContact")]
    [SwaggerResponse(StatusCodes.Status201Created, "The contact was added", typeof(EmergencyContactResource))]
    public async Task<IActionResult> AddEmergencyContact(
        [FromRoute] Guid touristId, [FromBody] EmergencyContactRequestResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var contact = await commandService.Handle(
                new AddEmergencyContactCommand(touristId, resource.Name, resource.Relationship, resource.PhoneNumber), cancellationToken);
            var result = new EmergencyContactResource(contact.Id, contact.Name, contact.Relationship, contact.PhoneNumber);
            return CreatedAtAction(nameof(GetProfile), new { touristId }, result);
        }
        catch (ProfilesError error)
        {
            return ProfilesActionResultAssembler.ToProblemDetails(this, error, localizer, problemDetailsFactory);
        }
    }

    [HttpPut("tourists/{touristId:guid}/emergency-contacts/{contactId:guid}")]
    [SwaggerOperation(Summary = "Update an emergency contact", OperationId = "UpdateEmergencyContact")]
    [SwaggerResponse(StatusCodes.Status200OK, "The contact was updated", typeof(EmergencyContactResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The contact was not found")]
    public async Task<IActionResult> UpdateEmergencyContact(
        [FromRoute] Guid touristId, [FromRoute] Guid contactId, [FromBody] EmergencyContactRequestResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var contact = await commandService.Handle(
                new UpdateEmergencyContactCommand(touristId, contactId, resource.Name, resource.Relationship, resource.PhoneNumber),
                cancellationToken);
            return Ok(new EmergencyContactResource(contact.Id, contact.Name, contact.Relationship, contact.PhoneNumber));
        }
        catch (ProfilesError error)
        {
            return ProfilesActionResultAssembler.ToProblemDetails(this, error, localizer, problemDetailsFactory);
        }
    }

    [HttpDelete("tourists/{touristId:guid}/emergency-contacts/{contactId:guid}")]
    [SwaggerOperation(Summary = "Remove an emergency contact", OperationId = "RemoveEmergencyContact")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The contact was removed")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The contact was not found")]
    public async Task<IActionResult> RemoveEmergencyContact(
        [FromRoute] Guid touristId, [FromRoute] Guid contactId, CancellationToken cancellationToken)
    {
        try
        {
            await commandService.Handle(new RemoveEmergencyContactCommand(touristId, contactId), cancellationToken);
            return NoContent();
        }
        catch (ProfilesError error)
        {
            return ProfilesActionResultAssembler.ToProblemDetails(this, error, localizer, problemDetailsFactory);
        }
    }

    [HttpGet("tourists/{touristId:guid}/completeness")]
    [SwaggerOperation(
        Summary = "Check whether a tourist's profile is complete enough to join an expedition",
        OperationId = "GetProfileCompleteness")]
    [SwaggerResponse(StatusCodes.Status200OK, "The completeness was computed", typeof(ProfileCompletenessResource))]
    public async Task<IActionResult> GetCompleteness([FromRoute] Guid touristId, CancellationToken cancellationToken)
    {
        var result = await queryService.Handle(new GetTouristProfileCompletenessQuery(touristId), cancellationToken);
        return Ok(new ProfileCompletenessResource(result.CanJoinExpedition, result.CompletionPercentage, result.MissingFields));
    }

    [HttpGet("agencies/{agencyId:guid}/tourists/{touristId:guid}")]
    [SwaggerOperation(
        Summary = "Agency staff: read a tourist's full profile",
        Description = "Only tourists with a booking on one of the agency's tours are visible. Reading this " +
                      "logs a MedicalDataAccessLog entry.",
        OperationId = "GetTouristProfileForAgency")]
    [SwaggerResponse(StatusCodes.Status200OK, "The profile was found", typeof(TouristProfileResource))]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "The tourist has no booking with this agency")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No profile exists yet for this tourist")]
    public async Task<IActionResult> GetProfileForAgency(
        [FromRoute] Guid agencyId, [FromRoute] Guid touristId, CancellationToken cancellationToken)
    {
        var requestedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        try
        {
            var profile = await queryService.Handle(new GetTouristProfileForAgencyQuery(agencyId, touristId, requestedBy), cancellationToken);
            if (profile is null)
                return ProfilesActionResultAssembler.ToProblemDetails(
                    this, new ProfilesError(Domain.Model.Errors.ProfilesErrors.ProfileNotFound), localizer, problemDetailsFactory);

            return Ok(TouristProfileResourceFromEntityAssembler.ToResourceFromEntity(profile));
        }
        catch (ProfilesError error)
        {
            return ProfilesActionResultAssembler.ToProblemDetails(this, error, localizer, problemDetailsFactory);
        }
    }

    [HttpGet("agencies/{agencyId:guid}/tourists/{touristId:guid}/medical-access-log")]
    [SwaggerOperation(
        Summary = "Agency staff: see who accessed a tourist's medical profile and when",
        OperationId = "GetMedicalDataAccessLog")]
    [SwaggerResponse(StatusCodes.Status200OK, "The log was found", typeof(IEnumerable<MedicalDataAccessLogResource>))]
    public async Task<IActionResult> GetMedicalDataAccessLog(
        [FromRoute] Guid agencyId, [FromRoute] Guid touristId, CancellationToken cancellationToken)
    {
        try
        {
            var log = await queryService.Handle(new GetMedicalDataAccessLogQuery(agencyId, touristId), cancellationToken);
            return Ok(log.Select(MedicalDataAccessLogResourceFromEntityAssembler.ToResourceFromEntity));
        }
        catch (ProfilesError error)
        {
            return ProfilesActionResultAssembler.ToProblemDetails(this, error, localizer, problemDetailsFactory);
        }
    }

    private IActionResult InvalidEnum(string fieldName, string value) =>
        problemDetailsFactory.CreateProblemDetails(
            this, StatusCodes.Status400BadRequest, (Enum?)null, $"'{value}' is not a valid value for '{fieldName}'.");
}
