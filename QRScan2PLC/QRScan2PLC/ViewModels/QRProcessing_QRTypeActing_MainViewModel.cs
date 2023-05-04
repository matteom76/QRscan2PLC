using DeviceCommunication.Enums;
using DeviceCommunication.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using Prism.Regions.Navigation;
using QRScan2PLC.Enums;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Models;
using QRScan2PLC.Models.Properties;
using QRScan2PLC.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace QRScan2PLC.ViewModels
{
    public class QRProcessing_QRTypeActing_MainViewModel : BindableBase, IRegionAware
    {
        
        private ObservableCollection<QRTypes> QRTypesList;
        private ICommunicationDevice _commDevice;
        private IEnableFunctionsPLC _EnableFunctionsPLC;
        private IRegionManager _regionManager { get; }

        private string _QRResult;
        public string QRResult { get => _QRResult; set => SetProperty(ref _QRResult, value); }

        private string _QRFormat;
        public string QRFormat { get => _QRFormat; set => SetProperty(ref _QRFormat, value); }

        private string _TitleView;
        public string TitleView { get => _TitleView; set => SetProperty(ref _TitleView, value); }

        public QRProcessing_QRTypeActing_MainViewModel(ICommunicationDevice communicationDevice, IEnableFunctionsPLC enableFunctionsPLC,IRegionManager regionManager)
        {
            _commDevice = communicationDevice;
            _EnableFunctionsPLC = enableFunctionsPLC;
            _regionManager = regionManager;
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
            var DataRead = navigationContext.Parameters.GetValue<CodeReadData>("QRResult");
            QRResult = DataRead.ResultCode;
            QRFormat = DataRead.Format;
            QRTypesList = navigationContext.Parameters.GetValue<ObservableCollection<QRTypes>>("QRTypesList");
            _regionManager.RequestNavigate("ActingRegion1", "QRProcessing_Acting_Null", navigationCallback);
            _regionManager.RequestNavigate("ActingRegion2", "QRProcessing_Acting_Null", navigationCallback);
            _regionManager.RequestNavigate("ActingRegion3", "QRProcessing_Acting_Null", navigationCallback);
            _regionManager.RequestNavigate("ActingRegion4", "QRProcessing_Acting_Null", navigationCallback);
            _regionManager.RequestNavigate("ActingRegion5", "QRProcessing_Acting_Null", navigationCallback);

            if (QRResult!="")
                if (QRTypesList!=null)
                { 
                    foreach (QRTypes qrtype in QRTypesList)
                        if (CheckValidQRType(qrtype,QRResult))
                        {
                            TitleView = qrtype.Name;
                            PopulateRegion(qrtype.DataList);
                            return;
                        }
                    
                    TitleView = "NO QR TYPE FOUND";
                }
                else
                {
                    TitleView = "QR CODE NOT DETECTED";

                }
            else
                TitleView = "NO DEVICE CONFIGURATION SELECTED";

        }

        private void PopulateRegion(ObservableCollection<DataPLC> dataList)
        {
            int CountRegion = 1;
            foreach (DataPLC data in dataList)
            {
                if (CountRegion > 5)
                    break;
                else
                {
                    var DataParameters = new NavigationParameters();
                    DataParameters.Add("DataPLC", data);
                    switch (data.Acting.Type)
                    {
                        case ActingType.Button:
                            _regionManager.RequestNavigate("ActingRegion" + CountRegion, "QRProcessing_Acting_Button", navigationCallback, DataParameters);
                            CountRegion++;
                            break;
                        case ActingType.DisplayStatus:
                            _regionManager.RequestNavigate("ActingRegion" + CountRegion, "QRProcessing_Acting_DisplayStatus", navigationCallback, DataParameters);
                            CountRegion++;
                            break;
                        case ActingType.DisplayValue:
                            _regionManager.RequestNavigate("ActingRegion" + CountRegion, "QRProcessing_Acting_DisplayValue", navigationCallback, DataParameters);
                            CountRegion++;
                            break;
                        case ActingType.SetDataFixed:
                            if (data.DeviceAdr.type==DataType.Integer)
                            {
                                SetDataFixed<int> dataFixed = (SetDataFixed<int>)data.Acting.Property;
                                if (!dataFixed.Visible)
                                {
                                    if (_EnableFunctionsPLC.ActualValue)
                                    {
                                        DataFixedProcessing<int> DataProcessing = new DataFixedProcessing<int>(_commDevice, dataFixed);
                                        try
                                        {
                                            DataProcessing.WriteDataToPLC(data.DeviceAdr);
                                            break;
                                        }
                                        catch (Exception e)
                                        {
                                            DependencyService.Get<IToastMessage>().Show("Datafixed error:" + e.Message);
                                            break;
                                        }
                                    }
                                    else
                                        break;
                                   
                                }
                            }
                            else
                                if (data.DeviceAdr.type == DataType.Boolean)
                                {
                                    SetDataFixed<bool> dataFixed = (SetDataFixed<bool>)data.Acting.Property;
                                    if (!dataFixed.Visible)
                                    {
                                        if (_EnableFunctionsPLC.ActualValue)
                                        {
                                            DataFixedProcessing<bool> DataProcessing = new DataFixedProcessing<bool>(_commDevice, dataFixed);
                                            try
                                            {
                                                DataProcessing.WriteDataToPLC(data.DeviceAdr);
                                                break;
                                            }
                                            catch (Exception e)
                                            {
                                                DependencyService.Get<IToastMessage>().Show("Datafixed error:" + e.Message);
                                                break;
                                            }
                                        }
                                        else
                                            break;

                                    }
                                }
                            else
                                if (data.DeviceAdr.type == DataType.Real)
                                {
                                    SetDataFixed<double> dataFixed = (SetDataFixed<double>)data.Acting.Property;
                                    if (!dataFixed.Visible)
                                    {

                                        if (_EnableFunctionsPLC.ActualValue)
                                        {
                                            DataFixedProcessing<double> DataProcessing = new DataFixedProcessing<double>(_commDevice, dataFixed);
                                            try
                                            {
                                                DataProcessing.WriteDataToPLC(data.DeviceAdr);
                                                break;
                                            }
                                            catch (Exception e)
                                            {
                                                DependencyService.Get<IToastMessage>().Show("Datafixed error:" + e.Message);
                                                break;
                                            }

                                        }
                                        else
                                            break;
                                    }
                                }
                            else
                                if (data.DeviceAdr.type == DataType.String)
                                {
                                    SetDataFixed<string> dataFixed = (SetDataFixed<string>)data.Acting.Property;
                                    if (!dataFixed.Visible)
                                    {
                                        if (_EnableFunctionsPLC.ActualValue)
                                        {
                                            DataFixedProcessing<string> DataProcessing = new DataFixedProcessing<string>(_commDevice, dataFixed);
                                            try
                                            {
                                                DataProcessing.WriteDataToPLC(data.DeviceAdr);
                                                break;
                                            }
                                            catch (Exception e)
                                            {
                                                DependencyService.Get<IToastMessage>().Show("Datafixed error:" + e.Message);
                                                break;
                                            }

                                        }
                                        else
                                            break;
                                    }

                                }

                            _regionManager.RequestNavigate("ActingRegion" + CountRegion, "QRProcessing_Acting_SetDataFixed", navigationCallback, DataParameters);
                            CountRegion++;
                            break;
                        case ActingType.SetDataFromCode:
                            SetDataFromCode DataFromBarCode = (SetDataFromCode)data.Acting.Property;
                            if (!DataFromBarCode.Visible)
                            {
                                if (_EnableFunctionsPLC.ActualValue)
                                {                                 
                                    DataFromCodeProcessing DataProcessing = new DataFromCodeProcessing(_commDevice, DataFromBarCode);
                                    try
                                    {
                                        string InfoFromBarCode = DataProcessing.CheckInfoFromCode(QRResult);
                                        DataProcessing.WriteDataToPLC(InfoFromBarCode, data.DeviceAdr);
                                        break;

                                    }
                                    catch (Exception e)
                                    {
                                        DependencyService.Get<IToastMessage>().Show("Data from barcode error:" + e.Message);
                                        break;
                                    }
                                }
                                else
                                    break;
                            }

                            DataParameters.Add("QRCodeData", QRResult);
                            _regionManager.RequestNavigate("ActingRegion" + CountRegion, "QRProcessing_Acting_SetDataFromCode", navigationCallback, DataParameters);
                            CountRegion++;
                            break;
                        case ActingType.SetDataInput:
                            _regionManager.RequestNavigate("ActingRegion" + CountRegion, "QRProcessing_Acting_SetDataInput", navigationCallback, DataParameters);
                            CountRegion++;
                            break;
                        case ActingType.DisplayBargraph:
                            _regionManager.RequestNavigate("ActingRegion" + CountRegion, "QRProcessing_Acting_Bargraph", navigationCallback, DataParameters);
                            CountRegion++;
                            break;
                    }                    
                }
            }
        }

        private void navigationCallback(Prism.Regions.Navigation.IRegionNavigationResult obj)
        {
            Console.WriteLine(obj.ToString());
        }

        private bool CheckValidQRType(QRTypes qrtype, string qrResult)
        {
            if (qrtype.Condition.qrstringlenght>0)
            {
                if (qrResult.Length != qrtype.Condition.qrstringlenght)
                    return false;
            }

            if (qrtype.Condition.substring.Length>0)
            {
                if (qrtype.Condition.substringpos > 0)
                {
                    try
                    {
                        string sub = qrResult.Substring(qrtype.Condition.substringpos - 1, qrtype.Condition.substring.Length);
                        return (sub.Equals(qrtype.Condition.substring));
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
                else                   
                {
                    if (qrtype.Condition.substringpos == 0)
                        return qrResult.Contains(qrtype.Condition.substring);
                    else
                        return false;
                }
            }
            
            return true;
        }
    }
}
