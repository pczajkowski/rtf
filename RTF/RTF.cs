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
        public static void EnableTrackedChanges(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;

            bool check = true;

            string workingDirectory = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(workingDirectory))
                return;
            string tempFile = Path.Combine(workingDirectory, "tempFile");

            using (StreamReader sr = File.OpenText(path))
            using (StreamWriter sw = new StreamWriter(tempFile))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (check)
                    {
                        var result = AddRevisions(line);
                        if (result.changed && !string.IsNullOrEmpty(result.line))
                        {
                            check = false;
                            line = result.line;
                        }
                    }                  

                    sw.WriteLine(line);
                }
            }

            File.Delete(path);
            File.Move(tempFile, path);
        }
    }
}
