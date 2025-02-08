namespace PersonalWebsite.DataTransferObject.DataObjects
{
    public class PersonalInformation
    {
        public string Name { get; set; }

        public string Surname {  get; set; }

        public string Email { get; set; }

        public string LinkedinUrl { get; set; }

        public string GithubUrl { get; set; }

        public virtual Skills Skills { get; set; }

        public virtual IList<Job> Jobs { get; set; }

        public virtual Education Education { get; set; }
    }
}
