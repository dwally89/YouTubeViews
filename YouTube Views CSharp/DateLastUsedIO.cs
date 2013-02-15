namespace YouTube_Views_CSharp
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Used to read and write the date the program was last used to a file
    /// </summary>
    public class DateLastUsedIO
    {
        /// <summary>
        /// The FILENAME
        /// </summary>
        private const string FILENAME = "dateLastUsed.date";

        /// <summary>
        /// Writes the date to file.
        /// </summary>
        public static void WriteDateToFile()
        {
            XmlSerializer writer = new XmlSerializer(typeof(string));
            StreamWriter file = new StreamWriter(FILENAME);
            writer.Serialize(file, DateTime.Today.ToShortDateString());
            file.Close();
        }

        /// <summary>
        /// Reads the date from file.
        /// </summary>
        /// <returns>The date the program was last used</returns>
        public static string ReadDateFromFile()
        {            
            XmlSerializer reader = new XmlSerializer(typeof(string));
            string dateLastUsed = null;
            StreamReader file = null;
            try
            {
                file = new StreamReader(FILENAME);
                dateLastUsed = (string)reader.Deserialize(file);
                file.Close();
            }
            catch (FileNotFoundException)
            {
                WriteDateToFile();
                return ReadDateFromFile();
            }
            
            return dateLastUsed;
        }
    }
}
