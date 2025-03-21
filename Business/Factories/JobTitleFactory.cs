using Business.Models.JobTitle;
using Data.Entities;

namespace Business.Factories;

public class JobTitleFactory
{
    public static JobTitlesEntity Create(JobTitleRegistrationForm form) => new()
    {
        Title = form.Title,
    };

    public static JobTitle Create(JobTitlesEntity entity) => new()
    {
        Id = entity.Id,
        Title = entity.Title,
    };

    public static void Update(JobTitlesEntity entity, JobTitleUpdateForm form)
    {
        entity.Title = form.Title ?? entity.Title; // If form.Title is null, keep the original value. Ja, det är lite konstigt för att det är en value.
    }
}