using System;

namespace iMon.DisplayApi
{
    public class iMonLogErrorEventArgs : iMonLogEventArgs
    {
        #region Private variables

        private Exception exception;

        #endregion

        #region Public variables

        public Exception Exception
        {
            get { return this.exception; }
        }

        #endregion

        #region Constructors

        public iMonLogErrorEventArgs(string message) 
            : base(message)
        { }

        public iMonLogErrorEventArgs(string message, Exception exception) 
            : base(message)
        {
            this.exception = exception;
        }

        #endregion
    }
}
