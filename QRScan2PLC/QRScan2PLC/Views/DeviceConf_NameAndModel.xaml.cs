using Xamarin.Forms;

namespace QRScan2PLC.Views
{
    public partial class DeviceConf_NameAndModel : ContentPage
    {
        public ViewCell lastCell;

        public DeviceConf_NameAndModel()
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
