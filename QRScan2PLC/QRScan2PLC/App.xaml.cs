using Prism;
using Prism.Ioc;
using QRScan2PLC.ViewModels;
using QRScan2PLC.Views;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace QRScan2PLC
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            //await NavigationService.NavigateAsync("DeviceSelection");
            await NavigationService.NavigateAsync("QRProcessing_Main");
            
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterRegionServices();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<DeviceSelection, DeviceSelectionViewModel>();
            containerRegistry.RegisterForNavigation<DeviceConf_NameAndModel, DeviceConf_NameAndModelViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_SiemensPLCInfo, DeviceConf_SiemensPLCInfoViewModel>();
            containerRegistry.RegisterForNavigation<DeviceConf_QRType, DeviceConf_QRTypeViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_SetDataFixed_Int, DeviceConf_SetDataFixed_IntViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_DataPLC, DeviceConf_DataPLCViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_SetDataFixed_Real, DeviceConf_SetDataFixed_RealViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_SetDataFixed_Bool, DeviceConf_SetDataFixed_BoolViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_SetDataFixed_String, DeviceConf_SetDataFixed_StringViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_Button, DeviceConf_ButtonViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_SetDataFromCode, DeviceConf_SetDataFromCodeViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_SetDataInput, DeviceConf_SetDataInputViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_DisplayValue, DeviceConf_DisplayValueViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_DisplayStatus, DeviceConf_DisplayStatusViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_DisplayStatus_View, DeviceConf_DisplayStatus_ViewViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_DisplayStatus_Edit, DeviceConf_DisplayStatus_EditViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_Address_Siemens_Int, DeviceConf_Address_Siemens_IntViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_Address_Siemens_Bool, DeviceConf_Address_Siemens_BoolViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_Address_Siemens_Real, DeviceConf_Address_Siemens_RealViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_Address_Siemens_String, DeviceConf_Address_Siemens_StringViewModel>();
            containerRegistry.RegisterForNavigation<DeviceConf_DataPLC_Selection, DeviceConf_DataPLC_SelectionViewModel>();
            containerRegistry.RegisterForRegionNavigation<QRProcessing_QRScanner, QRProcessing_QRScannerViewModel>();
            containerRegistry.RegisterForNavigation<QRProcessing_Main, QRProcessing_MainViewModel>();
            containerRegistry.RegisterForRegionNavigation<QRProcessing_QRTypeActing_Main, QRProcessing_QRTypeActing_MainViewModel>();
            containerRegistry.RegisterInstance<IContainerRegistry>(containerRegistry);
            containerRegistry.RegisterForRegionNavigation<QRProcessing_PLCInfo, QRProcessing_PLCInfoViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_SiemensEnablePLCFunction, DeviceConf_SiemensEnablePLCFunctionViewModel>();
            containerRegistry.RegisterForRegionNavigation<QRProcessing_Acting_Button, QRProcessing_Acting_ButtonViewModel>();
            containerRegistry.RegisterForRegionNavigation<QRProcessing_Acting_SetDataFromCode, QRProcessing_Acting_SetDataFromCodeViewModel>();
            containerRegistry.RegisterForRegionNavigation<QRProcessing_Acting_SetDataFixed, QRProcessing_Acting_SetDataFixedViewModel>();
            containerRegistry.RegisterForRegionNavigation<QRProcessing_Acting_SetDataInput, QRProcessing_Acting_SetDataInputViewModel>();
            containerRegistry.RegisterForRegionNavigation<QRProcessing_Acting_DisplayValue, QRProcessing_Acting_DisplayValueViewModel>();
            containerRegistry.RegisterForRegionNavigation<QRProcessing_Acting_DisplayStatus, QRProcessing_Acting_DisplayStatusViewModel>();
            containerRegistry.RegisterForRegionNavigation<QRProcessing_Acting_Null, QRProcessing_Acting_NullViewModel>();
            containerRegistry.RegisterForRegionNavigation<QRProcessing_Acting_Bargraph, QRProcessing_Acting_BargraphViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_DisplayBargraph, DeviceConf_DisplayBargraphViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_MQTTBrokerInfo, DeviceConf_MQTTBrokerInfoViewModel>();
            containerRegistry.RegisterForRegionNavigation<DeviceConf_Address_MQTT_Topic, DeviceConf_Address_MQTT_TopicViewModel>();
            containerRegistry.RegisterForNavigation<DeviceConf_CommModelSelection, DeviceConf_CommModelSelectionViewModel>();
        }
    }
}
