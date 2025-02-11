namespace PersonalWebsite.Models.DataObjects
{
    public class Skills
    {
        public virtual IList<ProgrammingLanguage> ProgrammingLanguages { get; set; }

        public virtual IList<ToolFramework> ToolsFrameworks { get; set; }
    }
}
