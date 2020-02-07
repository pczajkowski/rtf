using System.IO;

namespace RTF
{
    public static class RTF
    {
        /// <summary>
        /// Adds "revisions" to document options
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static (bool changed, string line) AddRevisions(string line)
        {
            if (!line.Contains(@"\aenddoc\")) return (false, null);
            if (line.Contains(@"\revisions")) return (false, null);

            line = line.Replace(@"\aenddoc\", @"\aenddoc\revisions\");
            return (true, line);

        }

        /// <summary>
        /// Enables Tracked Changes for given RTF file.
        /// </summary>
        /// <param name="path"></param>
        public static bool EnableTrackedChanges(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            var check = true;

            var workingDirectory = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(workingDirectory))
                return false;

            var tempFile = Path.Combine(workingDirectory, "tempFile");

            using (var sr = File.OpenText(path))
            using (var sw = new StreamWriter(tempFile))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (check)
                    {
                        var (changed, newLine) = AddRevisions(line);
                        if (changed && !string.IsNullOrEmpty(newLine))
                        {
                            check = false;
                            line = newLine;
                        }
                    }                  

                    sw.WriteLine(line);
                }
            }

            File.Delete(path);
            File.Move(tempFile, path);

            return true;
        }
    }
}
