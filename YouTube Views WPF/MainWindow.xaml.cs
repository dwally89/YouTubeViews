namespace YouTube_Views_WPF
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using YouTube_Views_CSharp;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The THOUSAND s_ FORMAT
        /// </summary>
        public const string THOUSANDS_FORMAT = "{0,6:N0}";
        //test
        /// <summary>
        /// The videos
        /// </summary>
        private ExtendedObservableCollection<Video> videos;        
        
        /// <summary>
        /// The controller
        /// </summary>
        private Controller controller;

        /// <summary>
        /// The progress window
        /// </summary>
        private ProgressWindow progressWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                createBackgroundWorkerAndDisplayProgress(new DoWorkDelegate(LoaderDoWork), "Loading...");
                mnuAddAll.IsEnabled = !controller.UserHasVideosInDatabase();
                mnuDeleteAll.IsEnabled = !mnuAddAll.IsEnabled;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Used to create DoWorkDelegates
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
        public delegate void DoWorkDelegate(object sender, DoWorkEventArgs e);

        /// <summary>
        /// Loaders the do work.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
        public void LoaderDoWork(object sender, DoWorkEventArgs e)
        {
            menu.Dispatcher.Invoke((Action)(() =>
            {
                menu.IsEnabled = false;
            }));            

            BackgroundWorker worker = sender as BackgroundWorker;
            controller = new Controller(worker);
            videos = controller.Videos;
            
            dataGrid.Dispatcher.Invoke((Action)(() =>
            {
                dataGrid.ItemsSource = videos;
            }));
            
            this.Dispatcher.Invoke((Action)(() =>
            {
                Title = "YouTube Views - " + Controller.DEFAULT_USERNAME;
            }));                            
        }

        /// <summary>
        /// Progresses the changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ProgressChangedEventArgs" /> instance containing the event data.</param>
        public void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {            
            progressWindow.SetProgress(e.ProgressPercentage);
        }       

        /// <summary>
        /// Workers the completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs" /> instance containing the event data.</param>
        public void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            menu.IsEnabled = true;            
        }

        /// <summary>
        /// Updates the do work.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
        public void UpdateDoWork(object sender, DoWorkEventArgs e)
        {
            menu.Dispatcher.Invoke((Action)(() =>
            {
                menu.IsEnabled = false;
            }));

            BackgroundWorker worker = sender as BackgroundWorker;
            worker.ReportProgress(99);

            int previousNumberOfVideos = videos.Count;
            List<Video> videosFromYouTube = controller.GetDataFromYouTube();
            int currentNumberOfVideos = videosFromYouTube.Count;
            List<Video> videosThatWereUpdated = controller.UpdateVideos(videosFromYouTube);
            
            int numberOfNewVideos = currentNumberOfVideos - previousNumberOfVideos;               
            displayVideosAddedOrDeleted(numberOfNewVideos);

            worker.ReportProgress(100);

            displayUpdatesOrDeletions(videosThatWereUpdated, numberOfNewVideos);
        }

        /// <summary>
        /// Displays the updates or deletions.
        /// </summary>
        /// <param name="videosThatWereUpdated">The videos that were updated.</param>
        /// <param name="numberOfVideosAddedOrDeleted">The number of videos added or deleted.</param>
        private void displayUpdatesOrDeletions(List<Video> videosThatWereUpdated, int numberOfVideosAddedOrDeleted)
        {
            int totalViews = 0;
            string message;
            foreach (Video updatedVideo in videosThatWereUpdated)
            {
                message = string.Empty;
                Video video = updatedVideo;
                for (int i = 0; i < controller.Videos.Count; i++)
                {
                    if (controller.Videos[i].Name.Equals(updatedVideo.Name))
                    {
                        video = controller.Videos[i];
                    }
                }

                message = updatedVideo.Name + " gained " + String.Format(THOUSANDS_FORMAT, updatedVideo.Views).Trim() + " new view";
                totalViews += updatedVideo.Views;
                if (updatedVideo.Views > 1)
                {
                    message += "s";
                }

                message += ". Its total views are now " + String.Format(THOUSANDS_FORMAT, video.TotalViews).Trim() + ".";
                MessageBox.Show(message, "Update", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (totalViews > 1)
            {
                MessageBox.Show(string.Format(THOUSANDS_FORMAT, totalViews) + " new views in total", "Total Number Of New Views", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if ((videosThatWereUpdated.Count == 0) && (numberOfVideosAddedOrDeleted == 0))
            {
                MessageBox.Show("No updates occurred since " + DateLastUsedIO.ReadDateFromFile(), "Update Completed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Displays the videos added or deleted.
        /// </summary>
        /// <param name="numberOfNewVideos">The number of new videos.</param>
        private void displayVideosAddedOrDeleted(int numberOfNewVideos)
        {            
            string message;
            if (numberOfNewVideos != 0)
            {
                message = Math.Abs(numberOfNewVideos) + " video";
                if (Math.Abs(numberOfNewVideos) > 1)
                {
                    message += "s were ";
                }
                else
                {
                    message += " was ";
                }

                string title = "Videos ";
                if (numberOfNewVideos < 0)
                {
                    message += "deleted";
                    title += " Deleted";
                }
                else if (numberOfNewVideos > 0)
                {
                    message += "created";
                    title += " Created";
                }

                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            }            
        }

        /// <summary>
        /// Handles the Click event of the mnuUpdateViews control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void mnuUpdateViews_Click(object sender, RoutedEventArgs e)
        {
            // ManualUpdate();                        
            createBackgroundWorkerAndDisplayProgress(new DoWorkDelegate(UpdateDoWork), "Updating Views...");                        
        }

        /// <summary>
        /// Handles the Click event of the mnuTotalViews control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void mnuTotalViews_Click(object sender, RoutedEventArgs e)
        {
            int total = controller.TotalViews();
            MessageBox.Show("Total number of views: " + String.Format(THOUSANDS_FORMAT, total), "Total Views", MessageBoxButton.OK, MessageBoxImage.Information);            
        }

        /// <summary>
        /// Handles the Click event of the mnuAverageNumberOfViews control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void mnuAverageNumberOfViews_Click(object sender, RoutedEventArgs e)
        {
            int average = controller.AverageViews();
            MessageBox.Show("Average number of views: " + String.Format(THOUSANDS_FORMAT, average), "Average Views", MessageBoxButton.OK, MessageBoxImage.Information);            
        }

        /// <summary>
        /// Handles the Click event of the mnuDateLastUsed control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void mnuDateLastUsed_Click(object sender, RoutedEventArgs e)
        {
            string dateLastUsed = DateLastUsedIO.ReadDateFromFile();
            MessageBox.Show("Program was last used on " + dateLastUsed, "Date last used", MessageBoxButton.OK, MessageBoxImage.Information);            
        }

        /// <summary>
        /// Handles the Click event of the mnuAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void mnuAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditVideoWPF form = new AddEditVideoWPF();
            if (form.ShowDialog() == true)
            {
                controller.AddVideo(form.VideoToAddEdit);                
            }
        }

        /// <summary>
        /// Handles the Click event of the mnuEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void mnuEdit_Click(object sender, RoutedEventArgs e)
        {
            AddEditVideoWPF form = new AddEditVideoWPF((Video)dataGrid.SelectedItem);
            if (form.ShowDialog() == true)
            {
                string oldName = ((Video)dataGrid.SelectedItem).Name;
                controller.EditVideo(form.VideoToAddEdit, oldName);                
            }
        }

        /// <summary>
        /// Handles the Click event of the mnuDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void mnuDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
                "Are you sure that you want to delete " + ((Video)dataGrid.SelectedItem).Name + "?",
                "Are you sure?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                controller.DeleteVideo((Video)dataGrid.SelectedItem);
            }
        }

        /// <summary>
        /// Handles the Click event of the mnuChangeUser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void mnuChangeUser_Click(object sender, RoutedEventArgs e)
        {
            UsernameWindow form = new UsernameWindow();
            
            if (form.ShowDialog() == true)
            {
                controller.Username = form.Username;

                createBackgroundWorkerAndDisplayProgress(new DoWorkDelegate(ChangeUserDoWork), "Changing User...");                                

                bool enabled = controller.Username.Equals(Controller.DEFAULT_USERNAME);
                
                mnuAdd.IsEnabled = enabled;
                mnuEdit.IsEnabled = enabled;
                mnuDelete.IsEnabled = enabled;
                mnuUpdateViews.IsEnabled = enabled;                

                mnuAddAll.IsEnabled = !controller.UserHasVideosInDatabase();
                mnuDeleteAll.IsEnabled = !mnuAddAll.IsEnabled;
            }
        }

        /// <summary>
        /// Changes the user do work.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
        private void ChangeUserDoWork(object sender, DoWorkEventArgs e)
        {
            menu.Dispatcher.Invoke((Action)(() =>
            {
                menu.IsEnabled = false;
            }));

            BackgroundWorker worker = sender as BackgroundWorker;
            controller.SetVideosToYouTubeUser(worker);

            // controller.CalculateAllViewsPerDay();
            Dispatcher.Invoke((Action)(() =>
            {
                Title = "YouTube Views - " + controller.Username;
            }));            
        }

        /// <summary>
        /// Handles the Closing event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            DateLastUsedIO.WriteDateToFile();
        }

        /// <summary>
        /// Handles the Click event of the mnuWriteAll control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void mnuAddAll_Click(object sender, RoutedEventArgs e)
        {
            createBackgroundWorkerAndDisplayProgress(new DoWorkDelegate(AddAllVideosDoWork), "Adding All Videos...");
            mnuAddAll.IsEnabled = false;
            mnuDeleteAll.IsEnabled = !mnuAddAll.IsEnabled;
        }

        /// <summary>
        /// Adds all videos do work.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
        private void AddAllVideosDoWork(object sender, DoWorkEventArgs e)
        {
            menu.Dispatcher.Invoke((Action)(() =>
            {
                menu.IsEnabled = false;
            }));

            BackgroundWorker worker = sender as BackgroundWorker;
            controller.AddAllVideos();            
            MessageBox.Show("All videos written to database", "Videos Written", MessageBoxButton.OK, MessageBoxImage.Information);            
        }

        /// <summary>
        /// Performs a manual update
        /// </summary>
        private void manualUpdate()
        {
            /*
            List<Video> updatedVideos = controller.GetCopyOfVideos();
            InputBoxWPF inputBox = new InputBoxWPF(updatedVideos);
            if (inputBox.ShowDialog() == true)
            {
                controller.UpdateVideos(updatedVideos);
            }
             */
        }

        /// <summary>
        /// Creates the background worker and display progress.
        /// </summary>
        /// <param name="task">The task to be assigned to the DoWork</param>
        /// <param name="title">The title.</param>
        private void createBackgroundWorkerAndDisplayProgress(DoWorkDelegate task, string title)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.RunWorkerCompleted += WorkerCompleted;
            backgroundWorker.ProgressChanged += ProgressChanged;
            backgroundWorker.DoWork += new DoWorkEventHandler(task);
            backgroundWorker.RunWorkerAsync();

            progressWindow = new ProgressWindow(title);
            progressWindow.ShowDialog();
        }

        /// <summary>
        /// Deletes all videos do work.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
        private void DeleteAllVideosDoWork(object sender, DoWorkEventArgs e)
        {
            menu.Dispatcher.Invoke((Action)(() =>
            {
                menu.IsEnabled = false;
            }));

            BackgroundWorker worker = sender as BackgroundWorker;
            
            controller.DeleteAllVideos(worker);            
            MessageBox.Show("All all of " + controller.Username + "'s videos were deleted from the database", "Videos Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Handles the Click event of the mnuDeleteAll control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void mnuDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
                "Are you sure that you want to delete all of " + controller.Username + "'s videos from the database?",
                "Are you sure?",
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                createBackgroundWorkerAndDisplayProgress(new DoWorkDelegate(DeleteAllVideosDoWork), "Deleting all videos...");
                mnuDeleteAll.IsEnabled = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the mnuTotalViewsPerDay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void mnuTotalViewsPerDay_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Expected number of views per day = " + controller.TotalViewsPerDay(), "Total Views Per Day", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
