using System;
using System.Windows.Forms;

namespace iMon.DisplayApi
{
    public class iMonWrapperApi : Control
    {
        #region Private variables

        private const int WM_IMON_NOTIFICATION = 0x8000 + 0x1234;

        private bool disposed;

        private bool initialized;
        private iMonDisplayType displayType;

        private iMonVfd vfd;
        private iMonLcd lcd;

        #endregion

        #region Events

        public event EventHandler<iMonStateChangedEventArgs> StateChanged;
        public event EventHandler<iMonErrorEventArgs> Error;

        public event EventHandler<iMonLogErrorEventArgs> LogError;
        public event EventHandler<iMonLogEventArgs> Log;

        #endregion

        #region Public variables

        public bool IsInitialized
        {
            get 
            {
                return this.initialized; 
            }
        }

        public bool IsPluginModeEnabled
        {
            get 
            {
                this.OnLog("IMON_Display_IsPluginModeEnabled()");
                return (iMonNativeApi.IMON_Display_IsPluginModeEnabled() == iMonNativeApi.iMonDisplayResult.InPluginMode); 
            }
        }

        public iMonDisplayType DisplayType
        {
            get { return this.displayType; }
        }

        public iMonVfd VFD
        {
            get
            {
                if ((this.displayType & iMonDisplayType.VFD) != iMonDisplayType.VFD)
                {
                    this.OnLogError("VFD is not available");
                    return null;
                }

                return this.vfd;
            }
        }

        public iMonLcd LCD
        {
            get
            {
                if ((this.displayType & iMonDisplayType.LCD) != iMonDisplayType.LCD)
                {
                    this.OnLogError("LCD is not available");
                    return null;
                }

                return this.lcd;
            }
        }

        #endregion

        #region Constructors

        public iMonWrapperApi()
        {
            this.displayType = iMonDisplayType.Unknown;
            this.vfd = new iMonVfd(this);
            this.lcd = new iMonLcd(this);
        }

        #endregion

        #region Public functions

        public void Initialize()
        {
            if (this.IsInitialized)
            {
                return;
            }

            this.OnLog("IMON_Display_Init(" + this.Handle + ", " + WM_IMON_NOTIFICATION + ")");
            iMonNativeApi.iMonDisplayResult result = iMonNativeApi.IMON_Display_Init(this.Handle, WM_IMON_NOTIFICATION);
            if (result != iMonNativeApi.iMonDisplayResult.Succeeded)
            {
                this.onError(this.getErrorType(result));
            }
        }

        public void Uninitialize()
        {
            if (!this.IsInitialized)
            {
                return;
            }

            if ((this.displayType & iMonDisplayType.VFD) == iMonDisplayType.VFD)
            {
                this.vfd.Reset();
            }
            if ((this.displayType & iMonDisplayType.LCD) == iMonDisplayType.LCD)
            {
                this.lcd.Reset();
            }

            this.OnLog("IMON_Display_Uninit()");
            iMonNativeApi.IMON_Display_Uninit();
            this.onStateChanged(false);
        }

        #endregion

        #region Implementations of IDisposable

        public new void Dispose()
        {
            if (!this.disposed)
            {
                this.Uninitialize();

                base.Dispose(true);
                this.disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        #endregion

        #region Overrides of Control

        protected override void WndProc(ref Message msg)
        {
            switch (msg.Msg)
            {
                case WM_IMON_NOTIFICATION:
                    this.onMessage((iMonNativeApi.iMonDisplayNotifyCode)msg.WParam, msg.LParam);
                    break;
            }

            base.WndProc(ref msg);
        }

        #endregion

        #region Internal functions

        internal void OnLog(string message)
        {
            if (this.Log == null || string.IsNullOrEmpty(message))
            {
                return;
            }

            this.Log(this, new iMonLogEventArgs(message));
        }

        internal void OnLogError(string message)
        {
            this.OnLogError(message, null);
        }

        internal void OnLogError(string message, Exception exception)
        {
            if (this.LogError == null || string.IsNullOrEmpty(message))
            {
                return;
            }

            this.LogError(this, new iMonLogErrorEventArgs(message, exception));
        }

        #endregion

        #region Private functions

        private void onMessage(iMonNativeApi.iMonDisplayNotifyCode code, IntPtr data)
        {
            this.OnLog("Message received: " + code + "(" + data + ")");
            
            switch (code)
            {
                case iMonNativeApi.iMonDisplayNotifyCode.PluginSuccess:
                case iMonNativeApi.iMonDisplayNotifyCode.HardwareConnected:
                case iMonNativeApi.iMonDisplayNotifyCode.iMonRestarted:
                    this.displayType = (iMonDisplayType)data;
                    this.onStateChanged(true);
                    break;

                case iMonNativeApi.iMonDisplayNotifyCode.PluginFailed:
                    this.onError(this.getErrorType((iMonNativeApi.iMonDisplayInitResult)data));
                    break;

                case iMonNativeApi.iMonDisplayNotifyCode.HardwareDisconnected:
                case iMonNativeApi.iMonDisplayNotifyCode.iMonClosed:
                    this.onError(this.getErrorType(code));
                    break;

                case iMonNativeApi.iMonDisplayNotifyCode.LCDTextScrollDone:
                    this.lcd.OnScrollFinished();
                    break;
            }
        }

        private void onStateChanged(bool isInitialized)
        {
            if (this.initialized == isInitialized)
            {
                return;
            }

            this.OnLog("State changed");

            this.initialized = isInitialized;

            if (this.StateChanged != null)
            {
                this.StateChanged(this, new iMonStateChangedEventArgs(isInitialized));
            }
        }

        private void onError(iMonErrorType error)
        {
            this.OnLogError("Error received: " + error);

            if (this.Error != null)
            {
                this.Error(this, new iMonErrorEventArgs(error));
            }

            this.onStateChanged(false);
        }

        private iMonErrorType getErrorType(iMonNativeApi.iMonDisplayResult result)
        {
            this.OnLogError("Interpreting result error type: " + result);

            switch (result)
            {
                case iMonNativeApi.iMonDisplayResult.ApiNotInitialized:
                    return iMonErrorType.ApiNotInitialized;

                case iMonNativeApi.iMonDisplayResult.Failed:
                    return iMonErrorType.Unknown;

                case iMonNativeApi.iMonDisplayResult.InvalidArguments:
                    return iMonErrorType.InvalidArguments;

                case iMonNativeApi.iMonDisplayResult.InvalidPointer:
                    return iMonErrorType.InvalidPointer;

                case iMonNativeApi.iMonDisplayResult.NotInitialized:
                    return iMonErrorType.NotInitialized;

                case iMonNativeApi.iMonDisplayResult.NotInPluginMode:
                    return iMonErrorType.NotInPluginMode;

                case iMonNativeApi.iMonDisplayResult.OutOfMemory:
                    return iMonErrorType.OutOfMemory;
            }

            return iMonErrorType.Unknown;
        }

        private iMonErrorType getErrorType(iMonNativeApi.iMonDisplayNotifyCode notifyCode)
        {
            this.OnLogError("Interpreting notify error type: " + notifyCode);

            switch (notifyCode)
            {
                case iMonNativeApi.iMonDisplayNotifyCode.PluginFailed:
                    return iMonErrorType.Unknown;

                case iMonNativeApi.iMonDisplayNotifyCode.iMonClosed:
                    return iMonErrorType.iMonClosed;

                case iMonNativeApi.iMonDisplayNotifyCode.HardwareDisconnected:
                    return iMonErrorType.HardwareDisconnected;
            }

            return iMonErrorType.Unknown;
        }

        private iMonErrorType getErrorType(iMonNativeApi.iMonDisplayInitResult code)
        {
            this.OnLogError("Interpreting init result error type: " + code);

            switch (code)
            {
                case iMonNativeApi.iMonDisplayInitResult.HardwareNotConnected:
                    return iMonErrorType.HardwareNotConnected;

                case iMonNativeApi.iMonDisplayInitResult.HardwareNotSupported:
                    return iMonErrorType.HardwareNotSupported;

                case iMonNativeApi.iMonDisplayInitResult.iMonNotResponding:
                    return iMonErrorType.iMonNotResponding;

                case iMonNativeApi.iMonDisplayInitResult.PluginModeAlreadyInUse:
                    return iMonErrorType.PluginModeAlreadyInUse;

                case iMonNativeApi.iMonDisplayInitResult.PluginModeDisabled:
                    return iMonErrorType.PluginModeDisabled;
            }

            return iMonErrorType.Unknown;
        }

        #endregion
    }
}