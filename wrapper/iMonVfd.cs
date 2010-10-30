using System;

namespace iMon.DisplayApi
{
    public class iMonVfd
    {
        #region Private variables

        private iMonWrapperApi wrapper;

        #endregion

        #region Public variables

        #endregion

        #region Constructors

        internal iMonVfd(iMonWrapperApi wrapper)
        {
            if (wrapper == null)
            {
                throw new ArgumentNullException("wrapper");
            }

            this.wrapper = wrapper;
        }

        #endregion

        #region Public functions

        public bool SetText(string firstLine, string secondLine)
        {
            if (firstLine == null)
            {
                throw new ArgumentNullException("firstLine");
            }
            if (secondLine == null)
            {
                throw new ArgumentNullException("secondLine");
            }
            if (!this.wrapper.IsInitialized)
            {
                this.wrapper.OnLogError("VFD.SetText(): Not initialized");
                throw new InvalidOperationException("The display is not initialized");
            }

            this.wrapper.OnLog("IMON_Display_SetVfdText(" + firstLine + ", " + secondLine + ")");
            iMonNativeApi.iMonDisplayResult result = iMonNativeApi.IMON_Display_SetVfdText(firstLine, secondLine);

            switch (result)
            {
                case iMonNativeApi.iMonDisplayResult.Succeeded:
                    return true;
                
                case iMonNativeApi.iMonDisplayResult.InvalidPointer:
                    this.wrapper.OnLogError("IMON_Display_SetVfdText() => Invalid pointer");
                    throw new NullReferenceException();

                case iMonNativeApi.iMonDisplayResult.NotInitialized:
                    this.wrapper.OnLogError("IMON_Display_SetVfdText() => Not initialized");
                    throw new InvalidOperationException("The display is not initialized");
            }

            return false;
        }

        public bool SetEqualizer(int[] bandData)
        {
            if (bandData == null)
            {
                throw new ArgumentNullException("bandData");
            }
            if (bandData.Length != 16)
            {
                throw new ArgumentException("The equalizer band data must consist of 16 values between 0 and 100");
            }
            if (!this.wrapper.IsInitialized)
            {
                this.wrapper.OnLogError("VFD.SetEqualizer(): Not initialized");
                throw new InvalidOperationException("The display is not initialized");
            }

            iMonNativeApi.iMonDisplayEqualizerData eqData = new iMonNativeApi.iMonDisplayEqualizerData();
            eqData.BandData = bandData;

            this.wrapper.OnLog("IMON_Display_SetVfdEqData()");
            iMonNativeApi.iMonDisplayResult result = iMonNativeApi.IMON_Display_SetVfdEqData(ref eqData);

            switch (result)
            {
                case iMonNativeApi.iMonDisplayResult.Succeeded:
                    return true;

                case iMonNativeApi.iMonDisplayResult.InvalidPointer:
                    this.wrapper.OnLogError("IMON_Display_SetVfdEqData() => Invalid pointer");
                    throw new NullReferenceException();

                case iMonNativeApi.iMonDisplayResult.NotInitialized:
                    this.wrapper.OnLogError("IMON_Display_SetVfdEqData() => Not initialized");
                    throw new InvalidOperationException("The display is not initialized");
            }

            return false;
        }

        #endregion

        #region Internal functions

        internal void Reset()
        {
            this.wrapper.OnLog("VFD.Reset()");
            if (this.wrapper.IsInitialized)
            {
                this.SetText(string.Empty, string.Empty);
            }
        }

        #endregion

        #region Private functions

        #endregion
    }
}