namespace iMon.DisplayApi
{
    public static partial class iMonNativeApi
    {
        /// <summary>
        /// These enumeration values represent the notification codes.
        /// iMON Display API will send or post message to the caller application.
        /// The caller application should assign the message and the winodw handle to receivce message with IMON_Display_Init fucntion.
        /// These enumeration values are used with WPARAM parameter of the message.
        /// </summary>
        public enum iMonDisplayNotifyCode : int
        {
            /// <summary>
            /// When API succeeds to get the control for the display, API will post caller-specified message with DSPNM_PLUGIN_SUCCEED as WPARAM parameter.
            /// LPARAM represents DSPType. This value can be 0x01 (VFD), 0x02 (LCD) or 0x03 (VFD+LCD).
            /// </summary>
            PluginSuccess = 0,

            /// <summary>
            /// When API fails to get the control for the display, API will post caller-specified message with DSPNM_PLUGIN_FAILED as WPARAM parameter.
            /// LPARAM represents error code with DSPNResult.
            /// </summary>
            PluginFailed,

            /// <summary>
            /// When iMON starts, API will post caller-specified message with DSPNM_IMON_RESTARTED as WPARAM parameter.\n
            /// LPARAM represents DSPType. This value can be 0 (No Display), 0x01 (VFD), 0x02 (LCD) or 0x03 (VFD+LCD).
            /// </summary>
            iMonRestarted,

            /// <summary>
            /// When iMON closed, API will post caller-specified message with DSPNM_IMON_CLOSED as WPARAM parameter.\n
            /// LPARAM is not used.
            /// </summary>
            iMonClosed,

            /// <summary>
            /// When iMON HW newly connected, API will post caller-specified message with DSPNM_HW_CONNECTED as WPARAM parameter.\n
            /// LPARAM represents DSPType. This value can be 0 (No Display), 0x01 (VFD), 0x02 (LCD) or 0x03 (VFD+LCD).
            /// </summary>
            HardwareConnected,

            /// <summary>
            /// When iMON HW disconnected, API will post caller-specified message with DSPNM_HW_DISCONNECTED as WPARAM parameter.\n
            /// LPARAM is DSPNResult value, DSPN_ERR_HW_DISCONNECTED.
            /// </summary>
            HardwareDisconnected,

            /// <summary>
            /// When iMON LCD finishes scrolling Text, API will post caller-specified message with DSPNM_LCD_TEXT_SCROLL_DONE as WPARAM parameter.\n
            /// The caller application may need to know when text scroll is finished, for sending next text.\n
            /// LPARAM is not used.
            /// </summary>
            LCDTextScrollDone = 0x1000,
        }
    }
}