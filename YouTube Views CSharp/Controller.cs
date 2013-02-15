namespace YouTube_Views_CSharp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Google.GData.Client;
    using Google.YouTube;

    /// <summary>
    /// The controller class
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// The default username
        /// </summary>
        public const string DEFAULT_USERNAME = "dwally89";

        /// <summary>
        /// Initializes a new instance of the <see cref="Controller" /> class.
        /// </summary>
        /// <param name="worker">The worker.</param>
        public Controller(BackgroundWorker worker)
        {
            worker.ReportProgress(99);            

            Username = DEFAULT_USERNAME;
            Videos = new ExtendedObservableCollection<Video>();
            List<Video> list = DatabaseActions.ReadVideos(Username);
            list.Sort(new SortByTotalViews());
            foreach (Video video in list)
            {
                Videos.Add(video);
            }

            // Videos = DatabaseActions.ReadVideos(Username);            
            // CalculateAllViewsPerDay();
            worker.ReportProgress(100);            
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the videos.
        /// </summary>
        /// <value>
        /// The videos.
        /// </value>
        public ExtendedObservableCollection<Video> Videos       
        {
            get;
            private set;
        }

        /// <summary>
        /// Sets the videos to you tube user.
        /// </summary>
        /// <param name="worker">The worker.</param>
        public void SetVideosToYouTubeUser(BackgroundWorker worker)
        {
            worker.ReportProgress(99);

            List<Video> videosFromYouTube = GetDataFromYouTube();            
            Videos.Clear();            
            foreach (Video video in videosFromYouTube)
            {
                Videos.Add(video);
            }

            worker.ReportProgress(100);
        }

        /// <summary>
        /// Adds the video.
        /// </summary>
        /// <param name="video">The video.</param>
        public void AddVideo(Video video)
        {
            Videos.Add(video);
            DatabaseActions.AddVideo(video, Username);                

            // CalculateAllViewsPerDay();
        }

        /// <summary>
        /// Edits the video.
        /// </summary>
        /// <param name="newVersionOfVideo">The new version of video.</param>
        /// <param name="oldName">The old name.</param>
        public void EditVideo(Video newVersionOfVideo, string oldName)
        {
            foreach (Video video in Videos)
            {
                if (video.Name.Equals(oldName))
                {
                    video.Name = newVersionOfVideo.Name;
                    video.Views = newVersionOfVideo.Views;
                    video.HiddenNumbers = newVersionOfVideo.HiddenNumbers;
                    video.DateAdded = newVersionOfVideo.DateAdded;

                    DatabaseActions.EditVideo(newVersionOfVideo, oldName, Username);

                    // CalculateAllViewsPerDay();
                }
            }
        }

        /// <summary>
        /// Deletes the video.
        /// </summary>
        /// <param name="video">The video.</param>
        public void DeleteVideo(Video video)
        {
            DatabaseActions.DeleteVideo(video, Username);
            Videos.Remove(video);

            // CalculateAllViewsPerDay();
        }

        /// <summary>
        /// Deletes all videos.
        /// </summary>
        /// <param name="worker">The worker.</param>
        public void DeleteAllVideos(BackgroundWorker worker)
        {
            worker.ReportProgress(99);            
            while (Videos.Count > 0)            
            {
                DeleteVideo(Videos[0]);
            }

            worker.ReportProgress(100);
        }

        /// <summary>
        /// Totals the views.
        /// </summary>
        /// <returns>The total number of views</returns>
        public int TotalViews()
        {
            int total = 0;            
            foreach (Video video in Videos)
            {
                total += video.TotalViews;
            }

            return total;
        }

        /// <summary>
        /// Averages the views.
        /// </summary>
        /// <returns>The average number of views</returns>
        public int AverageViews()
        {
            return TotalViews() / Videos.Count;
        }

        /// <summary>
        /// Updates the videos.
        /// </summary>
        /// <param name="updatedVideos">The updated videos.</param>
        /// <returns>The videos who gained new views</returns>
        public List<Video> UpdateVideos(List<Video> updatedVideos)
        {
            List<Video> videosThatWereUpdated = new List<Video>();
            bool videoAlreadyExists = false;
            foreach (Video updatedVideo in updatedVideos)            
            {
                videoAlreadyExists = false;
                foreach (Video video in Videos) 
                {
                    if (video.Name.Equals(updatedVideo.Name))
                    {
                        videoAlreadyExists = true;
                        if (video.Views != updatedVideo.Views)
                        {
                            videosThatWereUpdated.Add(new Video(video.Name, updatedVideo.Views - video.Views, video.DateAdded, video.HiddenNumbers));
                            video.Views = updatedVideo.Views;                            
                        }                        

                        break;
                    }
                }

                if (!videoAlreadyExists)
                {
                    AddVideo(updatedVideo);
                }
            }

            if (Videos.Count > updatedVideos.Count)
            {
                bool videoFound;
                for (int i = 0; i < Videos.Count; i++)                
                {
                    videoFound = false;
                    foreach (Video updatedVideo in updatedVideos)
                    {
                        if (Videos[i].Name.Equals(updatedVideo.Name))
                        {
                            videoFound = true;
                            break;
                        }
                    }

                    if (!videoFound)
                    {
                        DeleteVideo(Videos[i]);
                    }
                }
            }

            DatabaseActions.UpdateVideos(Videos, Username);
            return videosThatWereUpdated;
        }

        /// <summary>
        /// Gets the copy of videos.
        /// </summary>
        /// <returns>A copy of the videos</returns>
        public List<Video> GetCopyOfVideos()
        {
            return DatabaseActions.ReadVideos(Username);
        }

        /// <summary>
        /// Gets the data from you tube.
        /// </summary>
        /// <returns>The videos for the YouTube user</returns>
        public List<Video> GetDataFromYouTube()
        {   
            List<Video> videosFromYouTube = new List<Video>();

            YouTubeRequestSettings settings = new YouTubeRequestSettings(
                "YouTube Views", 
                "AI39si55DTLsxYRKqgbCT4oN_j3gAcWaujkx7-akcjn-TJD60YLeIHFjrTKyN-8Rxy7fjDHL9Ly1Zy15eVlD4oQazPMMFrreSA");
            YouTubeRequest request = new YouTubeRequest(settings);

            int startIndex = 1;
            const int MAX_RESULTS = 50;
            bool videosAdded = true;

            while (videosAdded) 
            {
                // Uri uri = new Uri("https://gdata.youtube.com/feeds/api/users/" + username + "/uploads");            
                Uri uri = new Uri("https://gdata.youtube.com/feeds/api/videos?q=&author=" + Username +
                    "&start-index=" + startIndex + "&max-results=" + MAX_RESULTS);                
                Feed<Google.YouTube.Video> videoFeed = request.Get<Google.YouTube.Video>(uri);

                videosAdded = false;
                foreach (Google.YouTube.Video video in videoFeed.Entries)
                {
                    videosFromYouTube.Add(new Video(video.Title, video.ViewCount, video.YouTubeEntry.Published, 0));
                    videosAdded = true;         
                }

                startIndex += MAX_RESULTS;
            }

            videosFromYouTube.Sort(new SortByTotalViews());
            return videosFromYouTube;
        }

        /// <summary>
        /// Writes all videos to database.
        /// </summary>
        public void AddAllVideos()
        {
            DatabaseActions.AddNewVideosToDatabase(Videos, Username);
        }

        /// <summary>
        /// Users the has videos in database.
        /// </summary>
        /// <returns>Whether or not the user has videos in the database</returns>
        public bool UserHasVideosInDatabase()
        {
            return DatabaseActions.UserHasVideosInDatabase(Username);
        }

        /// <summary>
        /// Totals the views per day.
        /// </summary>
        /// <returns>The expected number of views per day</returns>
        public int TotalViewsPerDay()
        {
            double totalViewsPerDay = 0;
            foreach (Video video in Videos)
            {
                totalViewsPerDay += video.ViewsPerDay;
            }

            return (int)totalViewsPerDay;
        }

        /*
        /// <summary>
        /// Calculates the views per day.
        /// </summary>
        /// <param name="video">The video.</param>
        /// <param name="dateOfVideoWithMaxViews">The date of song with max views.</param>
        /// <returns>The number of views the video has perday</returns>
        private int calculateViewsPerDay(Video video, DateTime dateOfVideoWithMaxViews)
        {
            int factor = (dateOfVideoWithMaxViews - DateTime.Today).Days;
            double views = video.TotalViews;
            // double daysBetween = (video.DateAdded - DateTime.Today).Days;
            double daysBetween = Math.Abs((video.DateAdded - DateTime.Today).Days);
            // double answer = (views / daysBetween) * factor;
            double answer = views / daysBetween;
            return (int)answer;
        }         
/// <summary>
/// Calculates all views per day.
/// </summary>        
public void CalculateAllViewsPerDay()
{
    DateTime dateOfSongWithMaxViews = new DateTime();
    int maxViews = 0;
    foreach (Video video in Videos)
    {
        if (video.Views > maxViews)
        {
            maxViews = video.Views;
            dateOfSongWithMaxViews = video.DateAdded;
        }
    }

    foreach (Video video in Videos)
    {
        video.ViewsPerDay = calculateViewsPerDay(video, dateOfSongWithMaxViews);
    }            
}
 */ 
    }
}
