using System;

namespace iMon.DisplayApi
{
    public class iMonStateChangedEventArgs : EventArgs
    {
        #region Private variables

        private bool initialized;

        #endregion

        #region Public variables

        public bool IsInitialized
        {
            get { return this.initialized; }
        }

        #endregion

        #region Constructors

        public iMonStateChangedEventArgs(bool initialized) 
            : base()
        {
            this.initialized = initialized;
        }

        #endregion
    }
}