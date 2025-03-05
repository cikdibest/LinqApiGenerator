using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

public class DbContextHelper
{
    public static List<string> GetDbContexts(string projectName)
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        List<string> dbContexts = new List<string>();

        DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));
        Project project = dte.Solution.Projects.Cast<Project>().FirstOrDefault(p => p.Name == projectName);
        if (project == null) return dbContexts;

        foreach (ProjectItem item in project.ProjectItems)
        {
            if (item.FileCodeModel != null)
            {
                foreach (CodeElement element in item.FileCodeModel.CodeElements)
                {
                    if (element is CodeClass codeClass)
                    {
                        foreach (CodeElement baseClass in codeClass.Bases)
                        {
                            if (baseClass.FullName == "Microsoft.EntityFrameworkCore.DbContext")
                            {
                                dbContexts.Add(codeClass.FullName);
                            }
                        }
                    }
                }
            }
        }

        return dbContexts;
    }
}
