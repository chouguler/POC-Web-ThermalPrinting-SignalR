using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace POCThinClientServices
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void POCThinClientServicesInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            //The following code starts the services after it is installed.
            using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController(POCThinClientServicesInstaller.ServiceName))
            {
                serviceController.Start();
            }
        }
    }
}
