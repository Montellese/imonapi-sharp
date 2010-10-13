namespace iMon.DisplayApi
{
    public static partial class iMonNativeApi
    {
        /// <summary>
        /// These enumeration values represent the returned result for iMON Display API function calls.
        /// All iMON Display API function calls return one of this result values.
        /// For meaning of each result, refer the comment of each line below
        /// </summary>
        public enum iMonDisplayResult : int
        {
            /// <summary>
            /// Function Call Succeeded Without Error
            /// </summary>
            Succeeded = 0,
            /// <summary>
            /// Unspecified Failure
            /// </summary>
            Failed,
            /// <summary>
            /// Failed to Allocate Necessary Memory
            /// </summary>
            OutOfMemory,
            /// <summary>
            /// One or More Arguments Are Not Valid
            /// </summary>
            InvalidArguments,
            /// <summary>
            /// API is Not Initialized
            /// </summary>
            NotInitialized,
            /// <summary>
            /// Pointer is Not Valid
            /// </summary>
            InvalidPointer,

            /// <summary>
            /// API is Initialized
            /// </summary>
            Initialized = 0x1000,
            /// <summary>
            /// API is Not Initialized
            /// </summary>
            ApiNotInitialized,
            /// <summary>
            /// API Can Control iMON Display (Display Plug-in Mode)
            /// </summary>
            InPluginMode,
            /// <summary>
            /// API Can't Control iMON Display
            /// </summary>
            NotInPluginMode,
        }
    }
}