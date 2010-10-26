using System;

namespace iMon.DisplayApi
{
    [Flags]
    public enum iMonDisplayType
    {
        None    = 0,
        VFD     = 1,
        LCD     = 2,
        Unknown = 4 
    }
}