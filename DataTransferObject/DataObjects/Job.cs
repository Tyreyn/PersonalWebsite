namespace PersonalWebsite.DataTransferObject.DataObjects
{
    public class Job
    {
        public string Position { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string Company { get; set; }

        public virtual BPoints BPoints { get; set; }

    }
}
