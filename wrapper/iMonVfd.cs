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
                throw new InvalidOperationException("The display is not initialized");
            }

            iMonNativeApi.iMonDisplayResult result = iMonNativeApi.IMON_Display_SetVfdText(firstLine, secondLine);

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
                throw new InvalidOperationException("The display is not initialized");
            }

            iMonNativeApi.iMonDisplayEqualizerData eqData = new iMonNativeApi.iMonDisplayEqualizerData();
            eqData.BandData = bandData;

            iMonNativeApi.iMonDisplayResult result = iMonNativeApi.IMON_Display_SetVfdEqData(ref eqData);

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

        #endregion

        #region Internal functions

        internal void Reset()
        {
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