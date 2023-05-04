using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Interfaces
{
    public interface IEnableFunctionsPLCFactory
    {
        IEnableFunctionsPLC GetEnableFunctionPLC();
    }
}
