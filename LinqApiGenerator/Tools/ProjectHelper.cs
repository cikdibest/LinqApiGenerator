using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

public class ProjectHelper
{
    public static List<string> GetProjects()
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        List<string> projects = new List<string>();

        DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));
        foreach (Project project in dte.Solution.Projects)
        {
            if (project != null && project.Kind != EnvDTE.Constants.vsProjectKindSolutionItems)
                projects.Add(project.Name);
        }

        return projects;
    }

    public static List<string> GetProjectFolders(string projectName)
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        List<string> folders = new List<string>();

        DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));
        foreach (Project project in dte.Solution.Projects)
        {
            if (project.Name == projectName)
            {
                foreach (ProjectItem item in project.ProjectItems)
                {
                    if (item.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFolder)
                        folders.Add(item.Name);
                }
            }
        }

        return folders;
    }
}
