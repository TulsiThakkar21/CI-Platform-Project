using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CIPlatform.Entities.Models;

public partial class Story
{
    public long StoryId { get; set; }

    public long MissionId { get; set; }

    public long UserId { get; set; }

    [Required(ErrorMessage = "Please enter your story title.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Please enter the story description.")]
    public string Description { get; set; }

    public string? Status { get; set; }

    [Required(ErrorMessage = "Please enter a date.")]
    public DateTime PublishedAt { get; set; }

    [DataType(DataType.Url)]
    public string? VidUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Mission Mission { get; set; } = null!;

    public virtual ICollection<StoryInvite> StoryInvites { get; } = new List<StoryInvite>();

    public virtual ICollection<StoryMedium> StoryMedia { get; } = new List<StoryMedium>();

    public virtual User User { get; set; } = null!;
}
