namespace iMon.DisplayApi
{
    public static partial class iMonNativeApi
    {
        /// <summary>
        /// These enumeration values represent the result status for requesting Display Plug-in Mode to iMON.
        /// iMON Display API notifies one of this result values to the caller application after requesting Display Plug-in Mode to iMON.
        /// For more information, refer the comment of each line below
        /// </summary>
        public enum iMonDisplayInitResult : int
        {
            /// <summary>
            /// Display Plug-in Mode is Initialized Successfully
            /// </summary>
            Succeeded = 0,
            /// <summary>
            /// Display Plug-in is Already Used by Other Application
            /// </summary>
            PluginModeAlreadyInUse = 0x0100,
            /// <summary>
            /// iMON HW is Not Connected
            /// </summary>
            HardwareNotConnected,
            /// <summary>
            /// The Connected iMON HW doesn't Support Display Plug-in
            /// </summary>
            HardwareNotSupported,
            /// <summary>
            /// Display Plug-in Mode Option is Disabled
            /// </summary>
            PluginModeDisabled,
            /// <summary>
            /// The Latest iMON is Not Installed or iMON Not Running
            /// </summary>
            iMonNotResponding,
            /// <summary>
            /// Unknown Failure
            /// </summary>
            Unknown = 0x0200,
        }
    }
}