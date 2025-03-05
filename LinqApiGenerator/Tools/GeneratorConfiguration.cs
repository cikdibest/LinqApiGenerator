using System.Collections.Generic;

public class GeneratorConfiguration
{
    public string ConnectionString { get; set; }
    public bool IsConnectionStringValid { get; set; }
    public bool CanConnectToDatabase { get; set; }

    public string EntityProject { get; set; }
    public List<string> EntityProjectFolders { get; set; } = new List<string>();
    public string EntityFolder { get; set; }

    public string DtoProject { get; set; }
    public List<string> DtoProjectFolders { get; set; } = new List<string>();
    public string DtoFolder { get; set; }

    public bool GenerateEntityTypeConfiguration { get; set; }
    public string ConfigProject { get; set; }
    public List<string> ConfigProjectFolders { get; set; } = new List<string>();
    public string ConfigFolder { get; set; }

    public bool AddDbSetsToDbContext { get; set; }
    public string DbContextProject { get; set; }
    public List<string> DbContextClasses { get; set; } = new List<string>();
    public string SelectedDbContext { get; set; }
    public List<string> AvailableTables { get; internal set; }
}
