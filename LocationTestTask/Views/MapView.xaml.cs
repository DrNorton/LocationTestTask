using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;

namespace LocationTestTask.UI.Views
{
    public partial class MapView : PhoneApplicationPage
    {
        public MapView()
        {
            InitializeComponent();
            BingMap.CredentialsProvider = new ApplicationIdCredentialsProvider("AuxNBnpoOmnhUmUoL2a8xcc5z6B1eK_58NBtUEUaIuHkuKpHQDmVRxaB7lw_uuye");
        }
    }
}