using System;

namespace iMon.DisplayApi
{
    public class iMonErrorEventArgs : EventArgs
    {
        #region Private variables

        private iMonErrorType type;

        #endregion

        #region Public variables

        public iMonErrorType Type
        {
            get { return this.type; }
        }

        #endregion

        #region Constructors

        public iMonErrorEventArgs(iMonErrorType errorType) 
            : base()
        {
            this.type = errorType;
        }

        #endregion
    }
}