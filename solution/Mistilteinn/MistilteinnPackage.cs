using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using System.IO;
using Mistilteinn.ToolWindows;
using Mistilteinn.Infos;

namespace Mistilteinn
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(TicketListToolWindow))]
    [Guid(GuidList.guidMistilteinnPkgString)]
    [ProvideAutoLoad("ADFC4E64-0397-11D1-9F4E-00A0C911004F")]
    [ProvideAutoLoad("F1536EF8-92EC-443C-9ED7-FDADF150DA82")]
    public sealed class MistilteinnPackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public MistilteinnPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        DTE dte;
        DocumentEvents docEvent;
        SolutionEvents solEvent;

        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation
        #region Package Members

        TResult GetService<TService, TResult>()
        {
            return (TResult)GetService(typeof(TService));
        }

        void AddMenuCommand(MenuCommandService target, int pkgCmdId, EventHandler handler)
        {
            target.AddCommand(new MenuCommand(handler, new CommandID(GuidList.guidMistilteinnCmdSet, pkgCmdId)));
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            dte = GetService<DTE, DTE>();
            solEvent = dte.Events.SolutionEvents;
            solEvent.Opened += () =>
            {
                SolutionInfo.RootDir = Path.GetDirectoryName(dte.Solution.FullName);
                // ゴミを作りまくるので、ソリューションを開いた時に裏でGCする
                new System.Threading.Thread(() => GitUtil.DoGitGC(dte.Solution.FullName)).Start();
            };
            solEvent.AfterClosing += () =>
            {
                SolutionInfo.RootDir = null;
            };
            docEvent = dte.Events.DocumentEvents;
            docEvent.DocumentSaved += doc =>
            {
                var sol = dte.Solution.FullName;
                if (doc.FullName == GitUtil.GetCommitMessagePath(sol))
                    return;

                dte.Solution.SaveAs(sol);
                doc.ProjectItem.ContainingProject.Save();
                GitUtil.DoGitNow(sol);
            };
            // Fixupメソッドと対になるイベントハンドラ
            // Fixupメソッドで開いたコミットメッセージを閉じるときに、実際にfixupする
            docEvent.DocumentClosing += doc =>
            {
                if (doc.FullName == GitUtil.GetCommitMessagePath(dte.Solution.FullName))
                {
                    GitUtil.DoGitNowFixup(dte.Solution.FullName);
                }
            };

            // Add our command handlers for menu (commands must exist in the .vsct file)
            var mcs = GetService<IMenuCommandService, OleMenuCommandService>();
            if ( null != mcs )
            {
                // Create the command for the menu item.
                AddMenuCommand(mcs, PkgCmdIDList.cmdidFixup, (_, __) => Fixup());
                AddMenuCommand(mcs, PkgCmdIDList.cmdidMasterize, (_, __) => Masterize());
                AddMenuCommand(mcs, PkgCmdIDList.cmdidTicketList, (_, __) => ShowTicketList());
                AddMenuCommand(mcs, PkgCmdIDList.cmdidPrivateBuild, MenuItemCallback);
                AddMenuCommand(mcs, PkgCmdIDList.cmdidPull, MenuItemCallback);
            }
        }

        // fixup用のコミットメッセージファイルを開くだけ(無ければ作る)
        // 実際にfixupするのは、コミットメッセージファイルを閉じたとき(DocumentEventsのDocumentClosingイベント)
        void Fixup()
        {
            var commitMsgFile = GitUtil.GetCommitMessagePath(dte.Solution.FullName); 
            if (File.Exists(commitMsgFile) == false)
                File.CreateText(commitMsgFile).Close();
            dte.ItemOperations.OpenFile(commitMsgFile);
        }

        void Masterize()
        {
            GitUtil.DoGitMaster(dte.Solution.FullName);
        }

        void ShowTicketList()
        {
            var window = this.FindToolWindow(typeof(TicketListToolWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException("Can not create tool window.");
            }
            var windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
        #endregion

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            // Show a Message Box to prove we were here
            IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
            Guid clsid = Guid.Empty;
            int result;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(
                       0,
                       ref clsid,
                       "Mistilteinn",
                       string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.ToString()),
                       string.Empty,
                       0,
                       OLEMSGBUTTON.OLEMSGBUTTON_OK,
                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                       OLEMSGICON.OLEMSGICON_INFO,
                       0,        // false
                       out result));
        }

    }
}
