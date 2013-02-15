namespace YouTube_Views_CSharp
{
    using System.Collections.Generic;
    using System.Data.SqlClient;

    /// <summary>
    /// The main program
    /// </summary>
    public class DatabaseActions
    {
        /// <summary>
        /// Mains this instance.
        /// </summary>
        public static void Main()
        {
        }

        /// <summary>
        /// Updates the videos.
        /// </summary>
        /// <param name="videos">The videos.</param>
        /// <param name="username">The username.</param>
        public static void UpdateVideos(ExtendedObservableCollection<Video> videos, string username)
        {            
            foreach (Video video in videos)
            {
                EditVideo(video, video.Name, username);
            }
        }

        /// <summary>
        /// Updates the video.
        /// </summary>
        /// <param name="video">The video.</param>
        /// <param name="oldName">The old name.</param>
        /// <param name="username">The username.</param>
        public static void EditVideo(Video video, string oldName, string username)
        {            
            using (SqlConnection con = CreateNewSqlConnection())
            {
                con.Open();
                using (SqlCommand update = new SqlCommand("UPDATE Videos SET Name=@Name, Views=@Views, [Date Added]=@DateAdded, [Hidden Numbers]=@HiddenNumbers WHERE Name=@OldName AND Username=@Username", con))
                {                    
                    update.Parameters.Clear();
                    update.Parameters.Add(new SqlParameter("OldName", oldName));
                    update.Parameters.Add(new SqlParameter("Username", username.ToLower()));
                    update.Parameters.Add(new SqlParameter("Name", video.Name));
                    update.Parameters.Add(new SqlParameter("Views", video.Views));
                    update.Parameters.Add(new SqlParameter("DateAdded", video.DateAdded));
                    update.Parameters.Add(new SqlParameter("HiddenNumbers", video.HiddenNumbers));                    
                    update.ExecuteNonQuery();                 
                }
            }
        }

        /// <summary>
        /// Adds the video.
        /// </summary>
        /// <param name="video">The video.</param>
        /// <param name="username">The username.</param>
        public static void AddVideo(Video video, string username)
        {            
            using (SqlConnection con = CreateNewSqlConnection())
            {
                con.Open();
                using (SqlCommand insert = new SqlCommand("INSERT INTO Videos VALUES(@Name, @Views, @DateAdded, @HiddenNumbers, @Username)", con))
                {                        
                    insert.Parameters.Clear();
                    insert.Parameters.Add(new SqlParameter("Name", video.Name));
                    insert.Parameters.Add(new SqlParameter("Views", video.Views));
                    insert.Parameters.Add(new SqlParameter("DateAdded", video.DateAdded));
                    insert.Parameters.Add(new SqlParameter("HiddenNumbers", video.HiddenNumbers));                    
                    insert.Parameters.Add(new SqlParameter("Username", username.ToLower()));
                    insert.ExecuteNonQuery();                        
                }
            }
        }        

        /*
        /// <summary>
        /// Sorts the videos.
        /// </summary>
        /// <param name="sorter">The sorter.</param>
        public void SortVideos(IComparer sorter)
        {            
            Videos.Sort(sorter);
        }*/

        /// <summary>
        /// Deletes the video from database.
        /// </summary>
        /// <param name="video">The video.</param>
        /// <param name="username">The username.</param>
        public static void DeleteVideo(Video video, string username)
        {            
            using (SqlConnection con = CreateNewSqlConnection())
            {
                con.Open();
                using (SqlCommand delete = new SqlCommand("DELETE FROM Videos WHERE Name=@Name AND Username=@Username", con))
                {
                    delete.Parameters.Add(new SqlParameter("Name", video.Name));
                    delete.Parameters.Add(new SqlParameter("Username", username.ToLower()));
                    delete.ExecuteNonQuery();
                }
            }                
        }

        /// <summary>
        /// Creates the new SQL connection.
        /// </summary>
        /// <returns>A new SQL connection</returns>
        public static SqlConnection CreateNewSqlConnection()
        {            
            return new SqlConnection(YouTube_Views_CSharp.Properties.Settings.Default.YouTubeViewsDatabaseConnectionString);
        }

        /// <summary>
        /// Users the has videos in database.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Whether or not the user has videos in the database</returns>
        public static bool UserHasVideosInDatabase(string username)
        {
            return ReadVideos(username).Count > 0;
        }

        /// <summary>
        /// Reads the videos.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// The videos from the database
        /// </returns>
        public static List<Video> ReadVideos(string username)
        {
            List<Video> videos = new List<Video>();
            Video video;            
            using (SqlConnection con = CreateNewSqlConnection())
            {
                con.Open();
                using (SqlCommand selectAll = new SqlCommand("SELECT * FROM Videos WHERE Username=@Username", con))
                {
                    selectAll.Parameters.Add(new SqlParameter("Username", username.ToLower()));
                    SqlDataReader reader = selectAll.ExecuteReader();
                    while (reader.Read())
                    {                        
                        video = new Video(reader.GetString(0), reader.GetInt32(1), reader.GetDateTime(2), reader.GetInt32(3));
                        videos.Add(video);
                    }
                }
            }           

            return videos;
        }       

        /*
        private void setUpDatabase()
        {
            readVideosFromFile();            
            writeVideosToDatabase();
        }
         */

        /// <summary>
        /// Writes the videos to database.
        /// </summary>
        /// <param name="videos">The videos.</param>
        /// <param name="username">The username.</param>
        public static void AddNewVideosToDatabase(ExtendedObservableCollection<Video> videos, string username)
        {
            SqlConnection con;
            using (con = CreateNewSqlConnection())
            {
                con.Open();
                using (SqlCommand insert = new SqlCommand("INSERT INTO Videos VALUES(@Name, @Views, @DateAdded, @HiddenNumbers, @Username)", con))
                {                    
                    foreach (Video video in videos)
                    {
                        insert.Parameters.Clear();
                        insert.Parameters.Add(new SqlParameter("Name", video.Name));
                        insert.Parameters.Add(new SqlParameter("Views", video.Views));
                        insert.Parameters.Add(new SqlParameter("DateAdded", video.DateAdded));
                        insert.Parameters.Add(new SqlParameter("HiddenNumbers", video.HiddenNumbers));
                        insert.Parameters.Add(new SqlParameter("Username", username));
                        insert.ExecuteNonQuery();
                    }                    
                }                
            }
        }        
    }
}
