using System;

namespace iMon.DisplayApi
{
    public class iMonLcd
    {
        #region Private variables

        private iMonWrapperApi wrapper;

        private iMonLcdIconsControl icons;

        #endregion

        #region Events

        public event EventHandler ScrollFinished;

        #endregion

        #region Public variables

        public iMonLcdIconsControl Icons
        {
            get { return this.icons; }
        }

        #endregion

        #region Constructors

        internal iMonLcd(iMonWrapperApi wrapper)
        {
            if (wrapper == null)
            {
                throw new ArgumentNullException("wrapper");
            }

            this.wrapper = wrapper;
            this.icons = new iMonLcdIconsControl(wrapper);
        }

        #endregion

        #region Public functions

        public bool SetText(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            if (!this.wrapper.IsInitialized)
            {
                throw new InvalidOperationException("The display is not initialized");
            }

            iMonNativeApi.iMonDisplayResult result = iMonNativeApi.IMON_Display_SetLcdText(text);

            switch (result)
            {
                case iMonNativeApi.iMonDisplayResult.Succeeded:
                    return true;

                case iMonNativeApi.iMonDisplayResult.InvalidPointer:
                    throw new NullReferenceException();

                case iMonNativeApi.iMonDisplayResult.NotInitialized:
                    throw new InvalidOperationException("The display is not initialized");
            }

            return false;
        }

        public bool SetEqualizer(int[] leftChannelBandData, int[] rightChannelBandData)
        {
            if (leftChannelBandData == null)
            {
                throw new ArgumentNullException("leftChannelBandData");
            }
            if (rightChannelBandData == null)
            {
                throw new ArgumentNullException("rightChannelBandData");
            }
            if (leftChannelBandData.Length != 16)
            {
                throw new ArgumentException("The equalizer's left channel band data must consist of 16 values between 0 and 100");
            }
            if (rightChannelBandData.Length != 16)
            {
                throw new ArgumentException("The equalizer's right channel band data must consist of 16 values between 0 and 100");
            }
            if (!this.wrapper.IsInitialized)
            {
                throw new InvalidOperationException("The display is not initialized");
            }

            iMonNativeApi.iMonDisplayEqualizerData eqLeftData = new iMonNativeApi.iMonDisplayEqualizerData();
            eqLeftData.BandData = leftChannelBandData;
            iMonNativeApi.iMonDisplayEqualizerData eqRightData = new iMonNativeApi.iMonDisplayEqualizerData();
            eqRightData.BandData = rightChannelBandData;

            iMonNativeApi.iMonDisplayResult result = iMonNativeApi.IMON_Display_SetLcdEqData(ref eqLeftData, ref eqRightData);

            switch (result)
            {
                case iMonNativeApi.iMonDisplayResult.Succeeded:
                    return true;

                case iMonNativeApi.iMonDisplayResult.InvalidPointer:
                    throw new NullReferenceException();

                case iMonNativeApi.iMonDisplayResult.NotInitialized:
                    throw new InvalidOperationException("The display is not initialized");
            }

            return false;
        }

        public bool SetProgress(int value, int total)
        {
            if (value < 0)
            {
                throw new ArgumentException("The value of the progress can't be less than 0");
            }
            if (total < 0)
            {
                throw new ArgumentException("The maximum of the progress can't be less than 0");
            }
            if (value > total)
            {
                value = total;
            }
            if (!this.wrapper.IsInitialized)
            {
                throw new InvalidOperationException("The display is not initialized");
            } 
            
            iMonNativeApi.iMonDisplayResult result = iMonNativeApi.IMON_Display_SetLcdProgress(value, total);

            switch (result)
            {
                case iMonNativeApi.iMonDisplayResult.Succeeded:
                    return true;

                case iMonNativeApi.iMonDisplayResult.NotInitialized:
                    throw new InvalidOperationException("The display is not initialized");
            }

            return false;
        }

        #endregion

        #region Internal functions

        internal void Reset()
        {
            if (this.wrapper.IsInitialized)
            {
                this.SetText(string.Empty);
                this.icons.Reset();
            }
        }

        internal void OnScrollFinished()
        {
            if (this.ScrollFinished != null)
            {
                this.ScrollFinished(this, new EventArgs());
            }
        }

        #endregion

        #region Private functions

        #endregion
    }
}