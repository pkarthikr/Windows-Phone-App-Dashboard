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
    public partial class Add : Form
    {
        public Add()
        {
            InitializeComponent();
        }

        public string appid;  //Stores the App ID
        public string marketplace;  //Stores the Marketplace ID
        public string appdesc;   // Stores the App Description 
        public string appname;  // Stores the App Name 
        public string imageurl; // Stores the Image URL 


        public class app
        {
            public string name { get; set; }
            public string description { get; set; }
        }


        private void addbutton_Click(object sender, EventArgs e)
        {
            appid = appidtb.Text;
            marketplace = marketcombo.SelectedValue.ToString();

            getappdata(appid, marketplace);
        }

        public void getappdata(string appid, string marketplace)
        {
            try
            {
                string appidurl;
                string ZuneURL = "http://catalog.zune.net/v3.2/";
                appidurl = ZuneURL + marketplace + "/apps/" + appid;

                string imgprefix = "/primaryImage?width=100&height=100&resize=true";
                imageurl = appidurl + imgprefix;

                var xmlDoc = new XmlDocument();
                xmlDoc.Load(appidurl);

                XmlNodeList nodelist;

                XmlNamespaceManager nameSpaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
                nameSpaceManager.AddNamespace("a", "http://www.w3.org/2005/Atom");

                nodelist = xmlDoc.SelectNodes("/a:feed", nameSpaceManager);

                foreach (XmlNode node in nodelist)
                {
                    app newapp = new app();

                    XmlNode appnames = node.SelectSingleNode("a:title", nameSpaceManager);
                    newapp.name = appnames.InnerText;
                    appname = appnames.InnerText;

                    XmlNode description = node.SelectSingleNode("a:content", nameSpaceManager);
                    newapp.description = description.InnerText;
                    appdesc = description.InnerText;

                }


                XDocument localDoc = XDocument.Load("Apps.xml");

                localDoc.Root.Add(new XElement("app", new XAttribute("appid", appid),
                                                                           new XAttribute("marketplace", marketplace),
                                                                           new XAttribute("appname", appname),
                                                                           new XAttribute("description", appdesc),
                                                                           new XAttribute("image", imageurl)));

                localDoc.Save("Apps.xml");
                this.Close();
                AppList app = new AppList();
                app.Show();

            }

            catch 
            {
                MessageBox.Show("Oops ! You did something wrong ! Go back and Try Again");
            }


        }

        private void Add_Load(object sender, EventArgs e)
        {


            List<MarketPlace> marketplaces = new List<MarketPlace>();
            XmlDocument doc = new XmlDocument();
            doc.Load("Marketplace.xml");
            XmlNodeList marketplacesNodeList = doc.SelectNodes("marketplaces/marketplace");
            foreach (XmlNode node in marketplacesNodeList)
            {
                //Add items to  Marketplace list
                MarketPlace place = new MarketPlace();
                place.language = node.SelectSingleNode("language").InnerText;
                place.marketplace = node.SelectSingleNode("title").InnerText;
                marketplaces.Add(place);
            }

            //Set datasource property for combox 
            marketcombo.DataSource = marketplaces;

            //These 2 lines say which list column to display and which one to set as value
            marketcombo.DisplayMember = "marketplace";
            marketcombo.ValueMember = "language";

        }
    }
}
