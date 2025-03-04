using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text;
using System.Text.RegularExpressions;

namespace PersonalWebsite.Helpers
{
    public static class ReadmeMdToHtmlConverter
    {
        public static string Convert(string readmeString, string repoName)
        {
            StringBuilder sb = new StringBuilder("<div class=\"card index\">");
            if (readmeString == null) return "<span>TBD</span>";
            string[] splittedReadme = SplitByLane(readmeString);
            foreach (string line in splittedReadme)
            {
                if (line.Contains("#"))
                {
                    int headCount = CheckForHeader(line);
                    sb.Append($"<h{headCount}>{line.Replace("#", string.Empty)}</h{headCount}>");
                }
                else if (line.Contains("png") || line.Contains("jpg"))
                {
                    sb.Append(CheckForNewImage(line, repoName));
                }
                else
                {
                    sb.Append($"<span>{line}<span>");
                }
            }
            sb.Append("</div>");
            return sb.ToString();
        }

        private static string[] SplitByLane(string readmeString)
        {
            return readmeString.Split('\n');
        }

        private static int CheckForHeader(string readmeLine)
        {
            int i = 0;
            foreach (char c in readmeLine)
            {
                if (c == '#')
                {
                    i++;
                }
                else
                {
                    return i;
                }
            }
            return i;
        }

        private static string CheckForNewImage(string line, string repoName)
        {
            Regex rgx = new Regex("[\\w]+\\.[\"png\"|\"jpg\"]+");
            Match match = rgx.Match(line);
            return string.Format($"<img src=\"https://raw.githubusercontent.com/Tyreyn/{repoName}/main/{match.Value}\" width=20% height=20%>\r\n");
        }
    }
}
