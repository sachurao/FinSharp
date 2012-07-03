using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irrefutable.FinSharp.Desktop.Common
{
    public static class DesktopDefaults
    {
        public static string WindowRegionNavigation { get { return "NavigationRegion"; }}
        public static string WindowRegionToolkit { get { return "ToolkitRegion"; }}
        public static string WindowRegionWorkspace { get { return  "WorkspaceRegion"; }}
        public static string WindoRegionFooter { get { return  "StatusBarRegion"; }}
        public static string WindowRegionWorkspacePrimary { get { return  "WorkspacePrimaryRegion"; }}
        public static string WindowRegionWorkspaceSecondary { get { return "WorkspaceSecondaryRegion"; }}
        

        public const string RELATIVE_PATH_MODULES = @".\Modules";

    }
}
