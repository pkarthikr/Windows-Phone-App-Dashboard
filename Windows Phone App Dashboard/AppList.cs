using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Windows_Phone_App_Dashboard
{
    public partial class AppList : Form
    {
        public AppList()
        {
            InitializeComponent();
        }

        public string imageUrl;
        public string appName;
        public string appId;
        public string marketPlace;

        private void addbutton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Add add = new Add();
            add.Show();
            
        }

        private void AppList_Load(object sender, EventArgs e)
        {
            PopulateApps();
        }

        public void PopulateApps() {

            
            XmlDocument doc = new XmlDocument();
            doc.Load("Apps.xml");
            XmlNodeList appList = doc.SelectNodes("apps/app");

            foreach (XmlNode dataSources in appList)
            {
                appId = dataSources.Attributes["appid"].Value.ToString();
                appName = dataSources.Attributes["appname"].Value.ToString();
                marketPlace = dataSources.Attributes["marketplace"].Value.ToString();
                string[] appArray = { appName, marketPlace };
                applistview.Items.Add(appId).SubItems.AddRange(appArray);

            }
        }

        private void deletebutton_Click(object sender, EventArgs e)
        {
            string selectedAppId;
            selectedAppId = applistview.SelectedItems[0].Text;
            

            XDocument localDoc = XDocument.Load("Apps.xml");

            localDoc.Descendants("app").Where(p => p.Attribute("appid").Value == selectedAppId).FirstOrDefault().Remove();
            
            localDoc.Save("Apps.xml");
            applistview.Items.Clear();
            PopulateApps();
            
        }


    }
}
