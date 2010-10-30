using System;

namespace iMon.DisplayApi
{
    public class iMonLogEventArgs : EventArgs
    {
        #region Private variables

        private string message;

        #endregion

        #region Public variables

        public string Message
        {
            get { return this.message; }
        }

        #endregion

        #region Constructors

        public iMonLogEventArgs(string message) 
            : base()
        {
            this.message = message;
        }

        #endregion
    }
}
