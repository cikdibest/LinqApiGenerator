namespace LinqApiGenerator.Tools
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public partial class LinqApiEntityGeneratorControl : UserControl
    {
        private readonly GeneratorConfiguration config = new GeneratorConfiguration();

        public LinqApiEntityGeneratorControl()
        {
            this.InitializeComponent();
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

        private void ProjectSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                string selectedProject = comboBox.SelectedItem as string;

                if (comboBox == cmbEntityProject)
                {
                    config.EntityProject = selectedProject;
                    config.EntityProjectFolders = ProjectHelper.GetProjectFolders(selectedProject);
                    cmbEntityFolder.ItemsSource = config.EntityProjectFolders;
                }
                else if (comboBox == cmbDtoProject)
                {
                    config.DtoProject = selectedProject;
                    config.DtoProjectFolders = ProjectHelper.GetProjectFolders(selectedProject);
                    cmbDtoFolder.ItemsSource = config.DtoProjectFolders;
                }
                else if (comboBox == cmbConfigProject)
                {
                    config.ConfigProject = selectedProject;
                    config.ConfigProjectFolders = ProjectHelper.GetProjectFolders(selectedProject);
                    cmbConfigFolder.ItemsSource = config.ConfigProjectFolders;
                }
                else if (comboBox == cmbDbContextProject)
                {
                    config.DbContextProject = selectedProject;
                    config.DbContextClasses = DbContextHelper.GetDbContexts(selectedProject);
                    cmbDbContextClass.ItemsSource = config.DbContextClasses;
                }

                UpdateGenerateButtonState();
            }
        }

        private void FolderSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                string selectedFolder = comboBox.SelectedItem as string;

                if (comboBox == cmbEntityFolder)
                    config.EntityFolder = selectedFolder;
                else if (comboBox == cmbDtoFolder)
                    config.DtoFolder = selectedFolder;
                else if (comboBox == cmbConfigFolder)
                    config.ConfigFolder = selectedFolder;

                UpdateGenerateButtonState();
            }
        }

        private void chkGenerateEntityConfig_Checked(object sender, RoutedEventArgs e)
        {
            config.GenerateEntityTypeConfiguration = chkGenerateEntityConfig.IsChecked ?? false;
            ConfigOptions.Visibility = config.GenerateEntityTypeConfiguration ? Visibility.Visible : Visibility.Collapsed;
            UpdateGenerateButtonState();
        }

        private void chkAddDbSets_Checked(object sender, RoutedEventArgs e)
        {
            config.AddDbSetsToDbContext = chkAddDbSets.IsChecked ?? false;
            DbContextOptions.Visibility = config.AddDbSetsToDbContext ? Visibility.Visible : Visibility.Collapsed;
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
    }
}
