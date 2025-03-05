namespace LinqApiGenerator.Tools
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for LinqApiEntityGeneratorControl.
    /// </summary>
    public partial class LinqApiEntityGeneratorControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinqApiEntityGeneratorControl"/> class.
        /// </summary>
        public LinqApiEntityGeneratorControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "LinqApiEntityGenerator");
        }

        private async void txtConnectionString_TextChanged(object sender, TextChangedEventArgs e)
        {
            string connString = txtConnectionString.Text;
            config.ConnectionString = connString;

            bool isValidFormat = DatabaseValidator.ValidateConnectionStringFormat(connString);
            config.IsConnectionStringValid = isValidFormat;
            txtDbStatus.Text = isValidFormat ? "✔" : "❌";
            txtDbStatus.Foreground = isValidFormat ? Brushes.Orange : Brushes.Red;

            if (isValidFormat)
            {
                bool canConnect = await DatabaseValidator.CanConnectToDatabaseAsync(connString);
                config.CanConnectToDatabase = canConnect;
                txtDbStatus.Text = canConnect ? "✅" : "❌";
                txtDbStatus.Foreground = canConnect ? Brushes.Green : Brushes.Red;

                if (canConnect)
                {
                    config.AvailableTables = DatabaseHelper.GetTables(connString);
                    lstTables.ItemsSource = config.AvailableTables;
                }
            }
            UpdateGenerateButtonState();
        }



        private void cmbEntityProject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedProject = cmbEntityProject.SelectedItem as string;
            config.EntityProject = selectedProject;

            config.EntityProjectFolders = ProjectHelper.GetProjectFolders(selectedProject);
            cmbEntityFolder.ItemsSource = config.EntityProjectFolders;
            UpdateGenerateButtonState();
        }

        private void cmbDtoProject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedProject = cmbDtoProject.SelectedItem as string;
            config.DtoProject = selectedProject;

            config.DtoProjectFolders = ProjectHelper.GetProjectFolders(selectedProject);
            cmbDtoFolder.ItemsSource = config.DtoProjectFolders;
            UpdateGenerateButtonState();
        }

        private void cmbConfigProject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedProject = cmbConfigProject.SelectedItem as string;
            config.ConfigProject = selectedProject;

            config.ConfigProjectFolders = ProjectHelper.GetProjectFolders(selectedProject);
            cmbConfigFolder.ItemsSource = config.ConfigProjectFolders;
            UpdateGenerateButtonState();
        }

        private void cmbEntityFolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            config.EntityFolder = cmbEntityFolder.SelectedItem as string;
            UpdateGenerateButtonState();
        }

        private void cmbDtoFolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            config.DtoFolder = cmbDtoFolder.SelectedItem as string;
            UpdateGenerateButtonState();
        }

        private void cmbConfigFolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            config.ConfigFolder = cmbConfigFolder.SelectedItem as string;
            UpdateGenerateButtonState();
        }



        private void cmbDbContextProject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedProject = cmbDbContextProject.SelectedItem as string;
            config.DbContextProject = selectedProject;

            config.DbContextClasses = DbContextHelper.GetDbContexts(selectedProject);
            cmbDbContextClass.ItemsSource = config.DbContextClasses;
            UpdateGenerateButtonState();
        }



        private void cmbDbContextClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            config.SelectedDbContext = cmbDbContextClass.SelectedItem as string;
        }

       


        private void chkGenerateEntityConfig_Checked(object sender, RoutedEventArgs e)
        {
            config.GenerateEntityTypeConfiguration = chkGenerateEntityConfig.IsChecked ?? false;
            ConfigOptions.Visibility = config.GenerateEntityTypeConfiguration ? Visibility.Visible : Visibility.Collapsed;
            UpdateGenerateButtonState();
        }

       
        private void cmbDbContextClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            config.SelectedDbContext = cmbDbContextClass.SelectedItem as string;
            UpdateGenerateButtonState();
        }



        private void UpdateGenerateButtonState()
        {
            btnGenerate.IsEnabled = config.IsConnectionStringValid
                                    && config.CanConnectToDatabase
                                    && !string.IsNullOrEmpty(config.EntityProject)
                                    && !string.IsNullOrEmpty(config.EntityFolder)
                                    && !string.IsNullOrEmpty(config.DtoProject)
                                    && !string.IsNullOrEmpty(config.DtoFolder)
                                    && (!config.GenerateEntityTypeConfiguration || (!string.IsNullOrEmpty(config.ConfigProject) && !string.IsNullOrEmpty(config.ConfigFolder)))
                                    && (!config.AddDbSetsToDbContext || !string.IsNullOrEmpty(config.SelectedDbContext));
        }




        private void UpdateGenerateButtonState()
        {
            btnGenerate.IsEnabled = config.IsConnectionStringValid && config.CanConnectToDatabase && config.SelectedTables.Count > 0;
        }


    }
}