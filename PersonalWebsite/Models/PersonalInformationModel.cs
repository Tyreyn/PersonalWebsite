using PersonalWebsite.Models.DataObjects;

namespace PersonalWebsite.Models
{
    public class PersonalInformationModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string LinkedinUrl { get; set; }

        public string GithubUrl { get; set; }

        public virtual Skills Skills { get; set; }

        public virtual IList<ProfessionalExperience> ProfessionalExperience { get; set; }

        public virtual IList<Education> Education { get; set; }
    }
}
