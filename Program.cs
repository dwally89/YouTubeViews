using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace YouTube_Views_CSharp
{
    class Program
    {
        private static String filename = "songs.song";
        static void Main(string[] args)
        {
            string dateLastUsed = ReadDateFromFile();            
            WriteDateToFile();
            ArrayList songs = ReadFromFile();
            WriteToFile(songs);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MessageBox.Show("Program was last used on " + dateLastUsed, "Date last used", MessageBoxButtons.OK, MessageBoxIcon.Information);            
            Application.Run(new DisplaySongs(songs));
            //ExitCode();
        }
        public static void ExitCode()
        {
            string command = "no";
            bool close = false;
            while (!close)
            {
                Console.WriteLine("Enter 'q' to exit or 's' to re-open the program:");
                command = Console.ReadLine();
                if (command.Equals("s"))
                {
                    ArrayList songs = ReadFromFile();
                    Application.EnableVisualStyles();
                    Application.Run(new DisplaySongs(songs));
                }
                else if (command.Equals("q"))
                {
                    close = true;
                }
            }
        }
        private static void WriteDateToFile()
        {
            Console.WriteLine("Write today's date to file");
            XmlSerializer writer = new XmlSerializer(typeof(String));
            StreamWriter file = new StreamWriter("dateLastUsed.date");
            writer.Serialize(file, DateTime.Today.ToShortDateString());
        }
        private static string ReadDateFromFile(){                        
            Console.WriteLine("Read previous date from file");
            XmlSerializer reader = new XmlSerializer(typeof(String));
            StreamReader file = new StreamReader("dateLastUsed.date");
            string dateLastUsed = (string)reader.Deserialize(file);
            file.Close();
            return dateLastUsed;
        }
        public static void WriteToFile(ArrayList songs)
        {
            Console.WriteLine("Write to file");
                Type[] extraTypes = new Type[1];
                extraTypes[0] = typeof(Song);
                XmlSerializer writer = new XmlSerializer(typeof(ArrayList), extraTypes);
                StreamWriter file = new StreamWriter(filename);
                writer.Serialize(file, songs);
                file.Close();                            
        }
        public static ArrayList ReadFromFile()
        {
            Type[] extraTypes = new Type[1];
            extraTypes[0] = typeof(Song);
            Console.WriteLine("Read from file");
            XmlSerializer reader = new XmlSerializer(typeof(ArrayList), extraTypes);
            StreamReader file = new StreamReader(filename);
            ArrayList songs = (ArrayList)reader.Deserialize(file);
            file.Close();            
            Console.WriteLine("Sort songs");
            songs.Sort(new sortByTotalViews());            
            songs = calculateAllViewsPerDay(songs);
            return songs;
        }
        public class sortByTotalViews : IComparer
        {
            int IComparer.Compare(Object x, Object y)
            {
                Song sx = (Song)x;
                Song sy = (Song)y;
                if (sx.GetTotalViews() > sy.GetTotalViews())
                {
                    return -1;
                }
                else if (sx.GetTotalViews() < sy.GetTotalViews())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        public class sortByDateAdded : IComparer
        {
            bool ascending;
            public sortByDateAdded(bool ascending)
            {
                this.ascending = ascending;
            }
            int IComparer.Compare(Object x, Object y)
            {
                Song sx = (Song)x;
                Song sy = (Song)y;
                if (sx.DateAdded > sy.DateAdded)
                {
                    if (ascending)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (sx.DateAdded < sy.DateAdded)
                {
                    if (ascending)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }


        public class sortByViews : IComparer
        {
            int IComparer.Compare(Object x, Object y)
            {
                Song sx = (Song)x;
                Song sy = (Song)y;
                if (sx.Views > sy.Views)
                {
                    return -1;
                }
                else if (sx.Views < sy.Views)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        public static ArrayList calculateAllViewsPerDay(ArrayList songs)
        {
            Console.WriteLine("Calculate views per day");
            DateTime dateOfSongWithMaxViews = new DateTime();
            int maxViews = 0;
            for (int i = 0; i < songs.Count; i++)
            {
                if (((Song)songs[i]).Views > maxViews)
                {
                    maxViews = ((Song)songs[i]).Views;
                    dateOfSongWithMaxViews = ((Song)songs[i]).DateAdded;
                }
            }
            for (int i = 0; i < songs.Count; i++)
            {
                ((Song)songs[i]).ViewsPerDay = calculateViewsPerDay((Song)songs[i], dateOfSongWithMaxViews);
            }            

            return songs;
        }
        private static int calculateViewsPerDay(Song song, DateTime dateOfSongWithMaxViews)
        {
            int factor = (dateOfSongWithMaxViews - DateTime.Today).Days;
            double views = song.GetTotalViews();
            double daysBetween = (song.DateAdded - DateTime.Today).Days;
            double answer = (views / daysBetween) * factor;
            return (int)answer;
        }
    }
}
