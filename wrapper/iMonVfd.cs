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