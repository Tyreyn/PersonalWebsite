namespace PersonalWebsite.Models.DataObjects
{
    public class ProfessionalExperience
    {
        public string Position { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string Company { get; set; }

        public virtual IList<string> BPoints { get; set; }
    }
}
