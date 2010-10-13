using System;
using System.Runtime.InteropServices;

namespace iMon.DisplayApi
{
    public static partial class iMonNativeApi
    {
        /// <summary>
        /// This structure contains Equalizer data for 16 bands.
        /// </summary>
        [StructLayout(LayoutKind.Sequential), Serializable]
        public struct iMonDisplayEqualizerData
        {
            /// <summary>
            /// Represents Equalizer data for 16 bands. Its range is from 0 to 100.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public int[] BandData;
        }
    }
}