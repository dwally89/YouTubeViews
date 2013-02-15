namespace YouTube_Views_WPF
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    /// <summary>
    /// Interaction logic for UsernameWindow.xaml
    /// </summary>
    public partial class UsernameWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsernameWindow" /> class.
        /// </summary>
        public UsernameWindow()
        {
            InitializeComponent();
            txtUsername.Focus();
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username
        {
            get;
            private set;
        }

        /// <summary>
        /// Handles the Click event of the btnOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        [SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Private method")]
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Username = txtUsername.Text;
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
