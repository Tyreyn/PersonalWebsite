namespace PersonalWebsite.Models.DataObjects
{
    public class ProfessionalExperience
    {
        public required string Position { get; set; }

        public required string StartDate { get; set; }

        public required string EndDate { get; set; }

        public required string Company { get; set; }

        public virtual IList<string>? BPoints { get; set; }
    }
}
