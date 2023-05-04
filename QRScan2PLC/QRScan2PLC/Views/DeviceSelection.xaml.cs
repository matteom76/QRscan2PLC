using Xamarin.Forms;

namespace QRScan2PLC.Views
{
    public partial class DeviceSelection : ContentPage
    {
        public ViewCell lastCell;

        public DeviceSelection()
        {
            InitializeComponent();
        }

        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.FromHex("#e6e6e6");
                lastCell = viewCell;

            }
        }

        private void DeviceList_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}
