using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DTO;
using Microsoft.Win32;

namespace Bll.Abstract
{
    public interface IShellTools
    {
        Boolean OpenFile();
        List<BridgeWeibull> Plots { get; }
    }
}