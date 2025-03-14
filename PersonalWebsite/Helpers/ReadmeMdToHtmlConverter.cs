using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.Primitives;
using System.Text;
using System.Text.RegularExpressions;

namespace PersonalWebsite.Helpers
{
    public static class ReadmeMdToHtmlConverter
    {
        public static string Convert(string readmeString, string repoName)
        {
            bool isCode = false;
            StringBuilder sb = new StringBuilder("<div class=\"card index\">");
            if (readmeString == null) return "<span></span>";
            string[] splittedReadme = SplitByLane(readmeString);
            foreach (string line in splittedReadme)
            {
                if (line == "") continue;

                if (line.StartsWith("#"))
                {
                    int headCount = CheckForHeader(line);
                    sb.Append($"<h{headCount}>{line.Replace("#", string.Empty)}</h{headCount}>");
                }
                else if (line.Contains("png") || line.Contains("jpg"))
                {
                    sb.Append(CheckForNewImage(line, repoName));
                }
                else if (line.StartsWith("```"))
                {
                    isCode = !isCode;
                    sb.Append(isCode ? "<pre><code>" : "</pre></code>\r\n");
                }
                else
                {
                    sb.Append(isCode ? $"{line}\r\n" : $"<span>{line}</span>");
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
            return string.Format($"<img class=\"readmeImg\" src=\"https://raw.githubusercontent.com/Tyreyn/{repoName}/main/{match.Value}\">");
        }
    }
}
