namespace iMon.DisplayApi
{
    public enum iMonErrorType
    {
        Unknown,
        OutOfMemory,
        InvalidArguments,
        NotInitialized,
        InvalidPointer,
        ApiNotInitialized,
        NotInPluginMode,

        iMonClosed,
        HardwareDisconnected,

        PluginModeAlreadyInUse,
        HardwareNotConnected,
        HardwareNotSupported,
        PluginModeDisabled,
        iMonNotResponding
    }
}