using DeviceCommunication;
using DeviceCommunication.Enums;
using DeviceCommunication.MQTT;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions.Navigation;
using QRScan2PLC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;

namespace QRScan2PLC.ViewModels
{
    public class DeviceConf_Address_MQTT_TopicViewModel : BindableBase, IRegionAware
    {
        private string _TopicEntry;
        public string TopicEntry { get => _TopicEntry; set => SetProperty(ref _TopicEntry,value); }

        private MQTTEnableFunctionsPLC _EnableFunctionsPLC;
        public MQTTEnableFunctionsPLC EnableFunctionsPLC { get => _EnableFunctionsPLC; set => SetProperty(ref _EnableFunctionsPLC, value); }
        private MQTTAddressBoolean _AddressBoolean;
        public MQTTAddressBoolean AddressBoolean { get => _AddressBoolean; set => SetProperty(ref _AddressBoolean, value); }       
        private MQTTAddressInteger _AddressInteger;
        public MQTTAddressInteger AddressInteger { get => _AddressInteger; set => SetProperty(ref _AddressInteger, value); }
        private MQTTAddressReal _AddressReal;
        public MQTTAddressReal AddressReal { get => _AddressReal; set => SetProperty(ref _AddressReal, value); }
        private MQTTAddressString _AddressString;
        public MQTTAddressString AddressString { get => _AddressString; set => SetProperty(ref _AddressString, value); }
        
        private DataType TagType;

        public DelegateCommand ConfirmCmd { get; private set; }

        public DeviceConf_Address_MQTT_TopicViewModel()
        {
            ConfirmCmd = new DelegateCommand(ConfirmTopicEntry);
        }

        private void ConfirmTopicEntry()
        {
            try
            {
                if (EnableFunctionsPLC != null)
                    EnableFunctionsPLC.Topic = TopicEntry;
                else
                {
                    if (TagType == DataType.Boolean)
                        AddressBoolean.Topic = TopicEntry;
                    else
                    if (TagType == DataType.Integer)
                        AddressInteger.Topic = TopicEntry;
                    else
                    if (TagType == DataType.Real)
                        AddressReal.Topic = TopicEntry;
                    else
                    if (TagType == DataType.String)
                        AddressString.Topic = TopicEntry;
                }
            }
            catch (Exception e)
            {
                DependencyService.Get<IToastMessage>().Show(e.Message);
                Application.Current.MainPage.DisplayToastAsync(e.Message,2000);
                TopicEntry = "";
            }
        }

        public bool IsNavigationTarget(INavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(INavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(INavigationContext navigationContext)
        {
            var addressFactory = navigationContext.Parameters.GetValue<DeviceTag>("addressTag");
            TagType = navigationContext.Parameters.GetValue<DataType>("tagType");
            var enableFunctionsPLC = navigationContext.Parameters.GetValue<MQTTEnableFunctionsPLC>("EnablePLCInfo");
            if (enableFunctionsPLC != null)
            { 
                EnableFunctionsPLC = enableFunctionsPLC;
                TopicEntry = EnableFunctionsPLC.Topic;
            }
            else
            {
                if (TagType == DataType.Boolean)
                { 
                    AddressBoolean = (MQTTAddressBoolean)addressFactory.addressBoolean;
                    TopicEntry = AddressBoolean.Topic;
                }
                else
                if (TagType == DataType.Integer)
                { 
                    AddressInteger = (MQTTAddressInteger)addressFactory.addressInteger;
                    TopicEntry = AddressInteger.Topic;
                }
                else
                if (TagType == DataType.Real)
                {  
                    AddressReal = (MQTTAddressReal)addressFactory.addressReal;
                    TopicEntry = AddressReal.Topic;
                }
                else
                if (TagType == DataType.String)
                { 
                    AddressString = (MQTTAddressString)addressFactory.addressString;
                    TopicEntry = AddressString.Topic;
                }
            }
        }
    }
}
