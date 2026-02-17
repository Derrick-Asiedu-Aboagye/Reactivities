using System;
using Domain;

namespace Application.Activities.DTOs;

public class EditActivityDto : BaseActivityDto
{
    public string Id { get; set; } = "";

    public static implicit operator EditActivityDto(Activity v)
    {
        throw new NotImplementedException();
    }
}
