using System.Runtime.InteropServices;

namespace iMon.DisplayApi
{
    public static partial class iMonNativeApi
    {
        /// <summary>
        /// This function should be called to use other functions in iMON Display API.
        /// When the caller application calls this function, API tries to request Display Plug-in Mode to iMON.
        /// </summary>
        /// <param name="hwndNoti">API will send/post message to this handle.</param>
        /// <param name="uMsgNotification">API will send/post message to hwndNoti with this message identifier.</param>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// DisplayResult.Succeeded will be returned if succeeded. DisplayResult.InvalidArguments or DisplayResult.OutOfMemory can be returned when error occurs.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_Init")]
        public static extern iMonDisplayResult IMON_Display_Init(int hwndNoti, uint uMsgNotification);

        /// <summary>
        /// This function should be called when the caller application need not use this API any more.
        /// If this function call is missed, iMON can't display other information.
        /// </summary>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// DisplayResult.Succeeded will be returned if succeeded.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_Uninit")]
        public static extern iMonDisplayResult IMON_Display_Uninit();

        /// <summary>
        /// This function can be used when the caller application wants to know if API is initialized.
        /// </summary>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// If API is initialized, this call will return DisplayResult.Initialized. Otherwise DisplayResult.ApiNotInitialized will be returned.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_IsInited")]
        public static extern iMonDisplayResult IMON_Display_IsInited();

        /// <summary>
        /// This function can be used when the caller application wants to know if API can control iMON display.
        /// </summary>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// If API can control iMON display, this call will return DisplayResult.InPluginMode. Otherwise DisplayResult.NotInPluginMode will be returned.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_IsPluginModeEnabled")]
        public static extern iMonDisplayResult IMON_Display_IsPluginModeEnabled();

        /// <summary>
        /// This function can be used when the caller application wants to display text data on VFD module.
        /// </summary>
        /// <param name="line1">This string data will be displayed on the 1st line of VFD module.
        /// It doesn't support multi-byte character and if string data is longer than 16 characters, it displays 16 characters from the first.</param>
        /// <param name="line2">This string data will be displayed on the 2nd line of VFD module.
        /// It doesn't support multi-byte character and if string data is longer than 16 characters, it displays 16 characters from the first.</param>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// DisplayResult.Succeeded will be returned if succeeded. DisplayResult.InvalidPointer, DisplayResult.NotInitialized or DisplayResult.Failed can be returned if failed.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_SetVfdText", CharSet = CharSet.Auto)]
        public static extern iMonDisplayResult IMON_Display_SetVfdText(string line1, string line2);

        /// <summary>
        /// This function can be used when the caller application wants to display equalizer data on VFD module.
        /// </summary>
        /// <param name="pEqData">Pointer of DisplayEqualizerData structure. The caller application should fill this structure with the equalizer data for 16 bands.</param>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// DisplayResult.Succeeded will be returned if succeeded. DisplayResult.InvalidPointer, DisplayResult.NotInitialized or DisplayResult.Failed can be returned if failed.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_SetVfdEqData")]
        public static extern iMonDisplayResult IMON_Display_SetVfdEqData(ref iMonDisplayEqualizerData pEqData);

        /// <summary>
        /// This function can be used when the caller application wants to display text data on LCD module.
        /// </summary>
        /// <param name="line">This string data will be displayed on the LCD module.
        /// It supports multi-byte character and if string data is longer than display area, it will start to scroll.
        /// When text scrolling is finished, API will notify it with DisplayNotifyCode enumeration value, DisplayNotifyCode.LCDTextScrollDone.</param>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// DisplayResult.Succeeded will be returned if succeeded. DisplayResult.InvalidPointer, DisplayResult.NotInitialized or DisplayResult.Failed can be returned if failed.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_SetLcdText", CharSet = CharSet.Auto)]
        public static extern iMonDisplayResult IMON_Display_SetLcdText(string line);

        /// <summary>
        /// This function can be used when the caller application wants to display equalizer data on LCD module.
        /// </summary>
        /// <param name="pEqDataL">Pointer of DisplayEqualizerData structure. This parameter represents equalizer data of left channel.
        /// The caller application should fill this structure with the equalizer data of left channel for 16 bands.</param>
        /// <param name="pEqDataR">Pointer of DisplayEqualizerData structure. This parameter represents equalizer data of right channel.
        /// The caller application should fill this structure with the equalizer data of right channel for 16 bands.</param>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// DisplayResult.Succeeded will be returned if succeeded. DisplayResult.InvalidPointer, DisplayResult.NotInitialized or DisplayResult.Failed can be returned if failed.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_SetLcdEqData")]
        public static extern iMonDisplayResult IMON_Display_SetLcdEqData(ref iMonDisplayEqualizerData pEqDataL, ref iMonDisplayEqualizerData pEqDataR);

        /// <summary>
        /// This function can be used when the caller application wants to turn on/off all icons on LCD module.
        /// </summary>
        /// <param name="bOn">If this value is TRUE, iMON will turn on all icons. Otherwise, iMON will turn off all icons.</param>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// DisplayResult.Succeeded will be returned if succeeded. DisplayResult.NotInitialized or DisplayResult.Failed can be returned if failed.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_SetLcdAllIcons")]
        public static extern iMonDisplayResult IMON_Display_SetLcdAllIcons(bool bOn);

        /// <summary>
        /// This function can be used when the caller application wants to turn on/off orange shaped disk icons on the upper left part of LCD module.
        /// Disk icons consist of 8 pieces of orange and orange peel.
        /// </summary>
        /// <param name="btIconData1">Each bit represents one of icons shaped the piece of orange.
        /// MSB is used for the piece placed on top and the remaining bits are for the piece placed in CCW from top.</param>
        /// <param name="btIconData2">MSB represents the orange peel shaped icon. Other bits are not used.</param>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// DisplayResult.Succeeded will be returned if succeeded. DisplayResult.NotInitialized or DisplayResult.Failed can be returned if failed.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_SetLcdOrangeIcon")]
        public static extern iMonDisplayResult IMON_Display_SetLcdOrangeIcon(byte btIconData1, byte btIconData2);

        /// <summary>
        /// This function can be used when the caller application wants to turn on/off media type icons on the upper part of LCD module.
        /// </summary>
        /// <param name="btIconData">Each bit represents one of media type icons. From MSB each bit represents MUSIC, MOVIE, PHOTO, CD/DVD, TV, WEBCASTING and NEWS/WEATHER icon.</param>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// DisplayResult.Succeeded will be returned if succeeded. DisplayResult.NotInitialized or DisplayResult.Failed can be returned if failed.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_SetLcdMediaTypeIcon")]
        public static extern iMonDisplayResult IMON_Display_SetLcdMediaTypeIcon(byte btIconData);

        /// <summary>
        /// This function can be used when the caller application wants to turn on/off speaker icons on the upper right part of LCD module.
        /// </summary>
        /// <param name="btIconData1">Each bit represents one of speaker icons.\nFrom MSB each bit represents L, C, R, SL, LFE, SR, RL and SPDIF icon.</param>
        /// <param name="btIconData2">MSB represents RR icon. Other bits are not used.</param>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// DisplayResult.Succeeded will be returned if succeeded. DisplayResult.NotInitialized or DisplayResult.Failed can be returned if failed.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_SetLcdSpeakerIcon")]
        public static extern iMonDisplayResult IMON_Display_SetLcdSpeakerIcon(byte btIconData1, byte btIconData2);

        /// <summary>
        /// This function can be used when the caller application wants to turn on/off codec icons for video file on the lower part of LCD module.
        /// </summary>
        /// <param name="btIconData">Each bit represents one of video codec icons. From MSB each bit represents MPG, DIVX, XVID, WMV, MPG, AC3, DTS and WMA icon.</param>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// DisplayResult.Succeeded will be returned if succeeded. DisplayResult.NotInitialized or DisplayResult.Failed can be returned if failed.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_SetLcdVideoCodecIcon")]
        public static extern iMonDisplayResult IMON_Display_SetLcdVideoCodecIcon(byte btIconData);

        /// <summary>
        /// This function can be used when the caller application wants to turn on/off codec icons for audio file on the lower part of LCD module.
        /// </summary>
        /// <param name="btIconData">Each bit represents one of audio codec icons. From MSB each bit represents MP3, OGG, WMA and WAV icon.</param>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// DisplayResult.Succeeded will be returned if succeeded. DisplayResult.NotInitialized or DisplayResult.Failed can be returned if failed.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_SetLcdAudioCodecIcon")]
        public static extern iMonDisplayResult IMON_Display_SetLcdAudioCodecIcon(byte btIconData);

        /// <summary>
        /// This function can be used when the caller application wants to turn on/off aspect ratio icons on the lower right part of LCD module.
        /// </summary>
        /// <param name="btIconData">Each bit represents one of aspect ratio icons. From MSB each bit represents SRC, FIT, TV, HDTV, SCR1 and SCR2 icon.</param>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// DisplayResult.Succeeded will be returned if succeeded. DisplayResult.NotInitialized or DisplayResult.Failed can be returned if failed.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_SetLcdAspectRatioIcon")]
        public static extern iMonDisplayResult IMON_Display_SetLcdAspectRatioIcon(byte btIconData);

        /// <summary>
        /// This function can be used when the caller application wants to turn on/off icons on the lower left part of LCD module.
        /// </summary>
        /// <param name="btIconData">Each bit represents icon. From MSB each bit represents REPEAT, SHUFFLE, ALARM, REC, VOL and TIME icon.</param>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// DisplayResult.Succeeded will be returned if succeeded. DisplayResult.NotInitialized or DisplayResult.Failed can be returned if failed.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_SetLcdEtcIcon")]
        public static extern iMonDisplayResult IMON_Display_SetLcdEtcIcon(byte btIconData);

        /// <summary>
        /// This function can be used when the caller application wants to display progress bar on the upper and lower left part of text area of LCD module.
        /// </summary>
        /// <param name="nCurPos">It represents the current position of progress bar.</param>
        /// <param name="nTotal">It represents the total length of progress bar.</param>
        /// <returns>This function will return one of DisplayResult enumeration value.
        /// DisplayResult.Succeeded will be returned if succeeded. DisplayResult.NotInitialized or DisplayResult.Failed can be returned if failed.</returns>
        [DllImport("iMONDisplay.dll", EntryPoint = "IMON_Display_SetLcdProgress")]
        public static extern iMonDisplayResult IMON_Display_SetLcdProgress(int nCurPos, int nTotal);
    }
}