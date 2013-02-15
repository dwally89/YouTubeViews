namespace YouTube_Views_WPF
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressWindow" /> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public ProgressWindow(string title)
        {
            InitializeComponent();
            Title = title;
            lblDisplay.Content = title;
        }

        /// <summary>
        /// Sets the progress.
        /// </summary>
        /// <param name="percentage">The percentage.</param>
        public void SetProgress(int percentage)
        {
            progressBar.Value = percentage;
            if (progressBar.Value == 100)
            {
                this.Close();
            }
        }        
 
        /// <summary>
        /// Handles the Closing event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs" /> instance containing the event data.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (progressBar.Value != 100)
            {
                e.Cancel = true;
            }            
        }        
    }
}
