namespace YouTube_Views_WPF
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using YouTube_Views_CSharp;

    /// <summary>
    /// Interaction logic for InputBoxWPF.xaml
    /// </summary>
    public partial class InputBoxWPF : Window
    {
        /// <summary>
        /// The index of the song currently having its views entered
        /// </summary>
        private int index = 0;

        /// <summary>
        /// The videos
        /// </summary>
        private ExtendedObservableCollection<Video> videos;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBoxWPF" /> class.
        /// </summary>
        /// <param name="videos">The videos.</param>
        public InputBoxWPF(ExtendedObservableCollection<Video> videos)
        {
            InitializeComponent();
            this.videos = videos;
            lblDisplay.Content = "Enter in number of views for " + videos[index].Name;
            txtViews.Text = videos[index].Views.ToString();
            txtViews.SelectAll();
            txtViews.Focus();
            btnPrevious.Visibility = Visibility.Hidden;            
        }

        /// <summary>
        /// The direction to move in
        /// </summary>
        private enum MoveDirection
        {
            /// <summary>
            /// The forwards
            /// </summary>
            Forwards,

            /// <summary>
            /// The backwards
            /// </summary>
            Backwards
        }       

        /// <summary>
        /// Handles the Click event of the btnPrevious control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            move(MoveDirection.Backwards);
        }

        /// <summary>
        /// Handles the Click event of the btnNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            move(MoveDirection.Forwards);
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Moves the specified forwards.
        /// </summary>
        /// <param name="moveDirection">The move direction.</param>
        private void move(MoveDirection moveDirection)
        {
            videos[index].Views = int.Parse(txtViews.Text);
            if (moveDirection == MoveDirection.Forwards)
            {
                index++;
                btnPrevious.Visibility = Visibility.Visible;
                if (index >= (videos.Count - 1))
                {
                    btnNext.Content = "Finish";
                }
                else
                {
                    btnNext.Content = "Next";
                }
            }
            else
            {
                index--;
                if (index == 0)
                {
                    btnPrevious.Visibility = Visibility.Hidden;
                }
            }

            if (index < videos.Count)
            {
                lblDisplay.Content = "Enter in number of views for " + videos[index].Name;
                txtViews.Text = videos[index].Views.ToString();
                txtViews.Focus();
                txtViews.SelectAll();
            }
            else
            {                
                DialogResult = true;
            }
        }
    }
}
