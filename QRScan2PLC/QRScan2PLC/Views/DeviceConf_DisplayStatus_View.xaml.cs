using Xamarin.Forms;

namespace QRScan2PLC.Views
{
    public partial class DeviceConf_DisplayStatus_View : ContentView
    {
        public ViewCell lastCell;

        public DeviceConf_DisplayStatus_View()
        {
            InitializeComponent();
        }

        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.FromHex("#4d4d4d");
            
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.FromHex("#e6e6e6");
                lastCell = viewCell;

            }

        }
    }
}
