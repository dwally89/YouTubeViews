namespace YouTube_Views_WPF
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using YouTube_Views_CSharp;

    /// <summary>
    /// Interaction logic for AddEditVideoWPF.xaml
    /// </summary>
    public partial class AddEditVideoWPF : Window
    {
        /// TEST GIT
        /// <summary>
        /// Initializes a new instance of the <see cref="AddEditVideoWPF" /> class.
        /// </summary>
        public AddEditVideoWPF()
        {            
            InitializeComponent();
            Title = "Add Video";
            txtName.Focus();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEditVideoWPF" /> class.
        /// </summary>
        /// <param name="videoToEdit">The video to edit.</param>
        public AddEditVideoWPF(Video videoToEdit)
        {            
            VideoToAddEdit = videoToEdit;            
            InitializeComponent();

            txtName.Text = videoToEdit.Name;
            txtViews.Text = videoToEdit.Views.ToString();
            txtHiddenNumbers.Text = videoToEdit.HiddenNumbers.ToString();
            calendar.SelectedDate = videoToEdit.DateAdded;

            Title = "Edit Video";
            txtName.Focus();
        }

        /// <summary>
        /// Gets the video to add edit.
        /// </summary>
        /// <value>
        /// The video to add edit.
        /// </value>
        public Video VideoToAddEdit
        {            
            get;
            private set;
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        [SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Private method")]
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            int views = int.Parse(txtViews.Text);
            DateTime dateAdded = (DateTime)calendar.SelectedDate;
            int hiddenNumbers = int.Parse(txtHiddenNumbers.Text);

            VideoToAddEdit = new Video(name, views, dateAdded, hiddenNumbers);            
            
            DialogResult = true;
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
    }
}
