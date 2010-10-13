/*
 *  Universal Media Manager - Copyright (C) 2009 - 2010 Team UMX
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with UMM; see the file COPYING.htm in the main solution.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */

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

        #endregion

        #region Public variables

        public bool IsInitialized
        {
            get { return (iMonNativeApi.IMON_Display_IsInited() == iMonNativeApi.iMonDisplayResult.Initialized); }
        }

        public bool IsPluginModeEnabled
        {
            get { return (iMonNativeApi.IMON_Display_IsPluginModeEnabled() == iMonNativeApi.iMonDisplayResult.InPluginMode); }
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
                    // throw an InvalidDisplayHardwareException
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
                    // throw an InvalidDisplayHardwareException
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

            iMonNativeApi.iMonDisplayResult result = iMonNativeApi.IMON_Display_Init(this.Handle.ToInt32(), WM_IMON_NOTIFICATION);
            if (result != iMonNativeApi.iMonDisplayResult.Succeeded)
            {
                this.onError(getErrorType(result));
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

        #region Private functions

        private void onMessage(iMonNativeApi.iMonDisplayNotifyCode code, IntPtr data)
        {
            switch (code)
            {
                case iMonNativeApi.iMonDisplayNotifyCode.PluginSuccess:
                case iMonNativeApi.iMonDisplayNotifyCode.HardwareConnected:
                case iMonNativeApi.iMonDisplayNotifyCode.iMonRestarted:
                    this.displayType = (iMonDisplayType)data;
                    this.onStateChanged(true);
                    break;

                case iMonNativeApi.iMonDisplayNotifyCode.PluginFailed:
                    this.onError(getErrorType((iMonNativeApi.iMonDisplayInitResult)data));
                    break;

                case iMonNativeApi.iMonDisplayNotifyCode.HardwareDisconnected:
                case iMonNativeApi.iMonDisplayNotifyCode.iMonClosed:
                    this.onError(getErrorType(code));
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

            this.initialized = isInitialized;

            if (this.StateChanged != null)
            {
                this.StateChanged(this, new iMonStateChangedEventArgs(isInitialized));
            }
        }

        private void onError(iMonErrorType error)
        {
            switch (error)
            {
                
            }

            if (this.Error != null)
            {
                this.Error(this, new iMonErrorEventArgs(error));
            }

            this.onStateChanged(false);
        }

        private static iMonErrorType getErrorType(iMonNativeApi.iMonDisplayResult result)
        {
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

        private static iMonErrorType getErrorType(iMonNativeApi.iMonDisplayNotifyCode notifyCode)
        {
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

        private static iMonErrorType getErrorType(iMonNativeApi.iMonDisplayInitResult code)
        {
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