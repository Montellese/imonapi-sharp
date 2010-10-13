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
using System.Collections.Generic;

namespace iMon.DisplayApi
{
    public class iMonLcdIconsControl
    {
        #region Private variables

        private static Dictionary<iMonLcdIcons, byte> iconMasks;

        private iMonWrapperApi wrapper;

        private Dictionary<iMonLcdIcons, bool> icons;

        #endregion

        #region Public variables

        public bool this[iMonLcdIcons icon]
        {
            get
            {
                if (!this.wrapper.IsInitialized)
                {
                    return false;
                }

                return this.icons[icon];
            }
            set
            {
                if (!this.wrapper.IsInitialized)
                {
                    throw new InvalidOperationException("The display is not initialized");
                }

                this.Set(icon, value);
            }
        }

        #endregion

        #region Constructors

        static iMonLcdIconsControl()
        {
            iconMasks = new Dictionary<iMonLcdIcons, byte>(Enum.GetValues(typeof(iMonLcdIcons)).Length);

            // Disc icons
            iconMasks.Add(iMonLcdIcons.DiscTopCenter, 0x80);
            iconMasks.Add(iMonLcdIcons.DiscTopLeft, 0x40);
            iconMasks.Add(iMonLcdIcons.DiscMiddleLeft, 0x20);
            iconMasks.Add(iMonLcdIcons.DiscBottomLeft, 0x10);
            iconMasks.Add(iMonLcdIcons.DiscBottomCenter, 0x08);
            iconMasks.Add(iMonLcdIcons.DiscBottomRight, 0x04);
            iconMasks.Add(iMonLcdIcons.DiscMiddleRight, 0x02);
            iconMasks.Add(iMonLcdIcons.DiscTopRight, 0x01);
            iconMasks.Add(iMonLcdIcons.DiscCircle, 0x80);

            // Media type icons
            iconMasks.Add(iMonLcdIcons.Music, 0x80);
            iconMasks.Add(iMonLcdIcons.Movie, 0x40);
            iconMasks.Add(iMonLcdIcons.Photo, 0x20);
            iconMasks.Add(iMonLcdIcons.CdDvd, 0x10);
            iconMasks.Add(iMonLcdIcons.Tv, 0x08);
            iconMasks.Add(iMonLcdIcons.Webcast, 0x04);
            iconMasks.Add(iMonLcdIcons.NewsWeather, 0x02);

            // Speaker icons
            iconMasks.Add(iMonLcdIcons.SpeakerFrontLeft, 0x80);
            iconMasks.Add(iMonLcdIcons.SpeakerCenter, 0x40);
            iconMasks.Add(iMonLcdIcons.SpeakerFrontRight, 0x20);
            iconMasks.Add(iMonLcdIcons.SpeakerSideLeft, 0x10);
            iconMasks.Add(iMonLcdIcons.SpeakerLFE, 0x08);
            iconMasks.Add(iMonLcdIcons.SpeakerSideRight, 0x04);
            iconMasks.Add(iMonLcdIcons.SpeakerRearLeft, 0x02);
            iconMasks.Add(iMonLcdIcons.SpeakerSPDIF, 0x01);
            iconMasks.Add(iMonLcdIcons.SpeakerRearRight, 0x80);

            // Video codec icons
            iconMasks.Add(iMonLcdIcons.VideoMPG, 0x80);
            iconMasks.Add(iMonLcdIcons.VideoDivX, 0x40);
            iconMasks.Add(iMonLcdIcons.VideoXviD, 0x20);
            iconMasks.Add(iMonLcdIcons.VideoWMV, 0x10);
            iconMasks.Add(iMonLcdIcons.VideoMPGAudio, 0x08);
            iconMasks.Add(iMonLcdIcons.VideoAC3, 0x04);
            iconMasks.Add(iMonLcdIcons.VideoDTS, 0x02);
            iconMasks.Add(iMonLcdIcons.VideoWMA, 0x01);

            // Audio codec icons
            iconMasks.Add(iMonLcdIcons.AudioMP3, 0x80);
            iconMasks.Add(iMonLcdIcons.AudioOGG, 0x40);
            iconMasks.Add(iMonLcdIcons.AudioWMA, 0x20);
            iconMasks.Add(iMonLcdIcons.AudioWAV, 0x10);

            // Aspect ratio icons
            iconMasks.Add(iMonLcdIcons.AspectRatioSource, 0x80);
            iconMasks.Add(iMonLcdIcons.AspectRatioFIT, 0x40);
            iconMasks.Add(iMonLcdIcons.AspectRatioTv, 0x20);
            iconMasks.Add(iMonLcdIcons.AspectRatioHDTV, 0x10);
            iconMasks.Add(iMonLcdIcons.AspectRatioScreen1, 0x08);
            iconMasks.Add(iMonLcdIcons.AspectRatioScreen2, 0x04);

            // Other icons
            iconMasks.Add(iMonLcdIcons.Repeat, 0x80);
            iconMasks.Add(iMonLcdIcons.Shuffle, 0x40);
            iconMasks.Add(iMonLcdIcons.Alarm, 0x20);
            iconMasks.Add(iMonLcdIcons.Recording, 0x10);
            iconMasks.Add(iMonLcdIcons.Volume, 0x08);
            iconMasks.Add(iMonLcdIcons.Time, 0x04);
        }

        internal iMonLcdIconsControl(iMonWrapperApi wrapper)
        {
            if (wrapper == null)
            {
                throw new ArgumentNullException("wrapper");
            }

            this.wrapper = wrapper;

            this.icons = new Dictionary<iMonLcdIcons, bool>(Enum.GetValues(typeof(iMonLcdIcons)).Length);
            foreach (iMonLcdIcons icon in Enum.GetValues(typeof(iMonLcdIcons)))
            {
                this.icons.Add(icon, false);
            }
        }

        #endregion

        #region Public functions

        public bool ShowAll()
        {
            return this.SetAll(true);
        }

        public bool HideAll()
        {
            return this.SetAll(false);
        }

        public bool SetAll(bool show)
        {
            if (!this.wrapper.IsInitialized)
            {
                throw new InvalidOperationException("The display is not initialized");
            }

            iMonNativeApi.iMonDisplayResult result = iMonNativeApi.IMON_Display_SetLcdAllIcons(show);
            
            switch (result)
            {
                case iMonNativeApi.iMonDisplayResult.Succeeded:
                    iMonLcdIcons[] iconArray = new iMonLcdIcons[this.icons.Count];
                    this.icons.Keys.CopyTo(iconArray, 0);
                    foreach (iMonLcdIcons icon in iconArray)
                    {
                        this.icons[icon] = show;
                    } 
                    return true;

                case iMonNativeApi.iMonDisplayResult.NotInitialized:
                    throw new InvalidOperationException("The display is not initialized");
            }

            return false;
        }

        public bool Show(iMonLcdIcons icon)
        {
            return this.Set(icon, true);
        }

        public bool Show(IEnumerable<iMonLcdIcons> iconList)
        {
            return this.Set(iconList, true);
        }

        public bool Hide(iMonLcdIcons icon)
        {
            return this.Set(icon, false);
        }

        public bool Hide(IEnumerable<iMonLcdIcons> iconList)
        {
            return this.Set(iconList, false);
        }

        public bool Set(iMonLcdIcons icon, bool show)
        {
            if (!this.wrapper.IsInitialized)
            {
                throw new InvalidOperationException("The display is not initialized");
            }
            if (this.icons[icon] == show)
            {
                return true;
            }

            iMonNativeApi.iMonDisplayResult result = this.set(icon, show);

            switch (result)
            {
                case iMonNativeApi.iMonDisplayResult.Succeeded:
                    this.icons[icon] = show;
                    return true;

                case iMonNativeApi.iMonDisplayResult.NotInitialized:
                    throw new InvalidOperationException("The display is not initialized");
            }

            return false;
        }

        public bool Set(IEnumerable<iMonLcdIcons> iconList, bool show)
        {
            bool result = true;

            foreach (iMonLcdIcons icon in iconList)
            {
                if (!this.Set(icon, show))
                {
                    result = false;
                }
            }

            return result;
        }

        #endregion

        #region Internal functions

        internal void Reset()
        {
            if (this.wrapper.IsInitialized)
            {
                this.SetAll(false);
            }
            else
            {
                iMonLcdIcons[] iconArray = new iMonLcdIcons[this.icons.Count];
                this.icons.Keys.CopyTo(iconArray, 0);
                foreach (iMonLcdIcons icon in iconArray)
                {
                    this.icons[icon] = false;
                }
            }
        }

        #endregion

        #region Private functions

        private iMonNativeApi.iMonDisplayResult set(iMonLcdIcons icon, bool show)
        {
            byte[] data = new byte[2];

            switch (icon)
            {
                #region Sound icons
                case iMonLcdIcons.SpeakerCenter:
                case iMonLcdIcons.SpeakerFrontLeft:
                case iMonLcdIcons.SpeakerFrontRight:
                case iMonLcdIcons.SpeakerLFE:
                case iMonLcdIcons.SpeakerRearLeft:
                case iMonLcdIcons.SpeakerRearRight:
                case iMonLcdIcons.SpeakerSideLeft:
                case iMonLcdIcons.SpeakerSideRight:
                case iMonLcdIcons.SpeakerSPDIF:
                {
                    if (this.icons[iMonLcdIcons.SpeakerCenter])
                        data[0] |= iconMasks[iMonLcdIcons.SpeakerCenter];
                    if (this.icons[iMonLcdIcons.SpeakerFrontLeft])
                        data[0] |= iconMasks[iMonLcdIcons.SpeakerFrontLeft];
                    if (this.icons[iMonLcdIcons.SpeakerFrontRight])
                        data[0] |= iconMasks[iMonLcdIcons.SpeakerFrontRight];
                    if (this.icons[iMonLcdIcons.SpeakerLFE])
                        data[0] |= iconMasks[iMonLcdIcons.SpeakerLFE];
                    if (this.icons[iMonLcdIcons.SpeakerRearLeft])
                        data[0] |= iconMasks[iMonLcdIcons.SpeakerRearLeft];
                    if (this.icons[iMonLcdIcons.SpeakerRearRight])
                        data[1] |= iconMasks[iMonLcdIcons.SpeakerRearRight];
                    if (this.icons[iMonLcdIcons.SpeakerSideLeft])
                        data[0] |= iconMasks[iMonLcdIcons.SpeakerSideLeft];
                    if (this.icons[iMonLcdIcons.SpeakerSideRight])
                        data[0] |= iconMasks[iMonLcdIcons.SpeakerSideRight];
                    if (this.icons[iMonLcdIcons.SpeakerSPDIF])
                        data[0] |= iconMasks[iMonLcdIcons.SpeakerSPDIF];

                    int index = 0;
                    if (icon == iMonLcdIcons.SpeakerRearRight)
                    {
                        index = 1;
                    }

                    if (show)
                    {
                        data[index] |= iconMasks[icon];
                    }
                    else
                    {
                        data[index] &= (byte)~iconMasks[icon];
                    }

                    return iMonNativeApi.IMON_Display_SetLcdSpeakerIcon(data[0], data[1]);
                }
                #endregion

                #region Disc icons
                case iMonLcdIcons.DiscTopCenter:
                case iMonLcdIcons.DiscTopLeft:
                case iMonLcdIcons.DiscMiddleLeft:
                case iMonLcdIcons.DiscBottomLeft:
                case iMonLcdIcons.DiscBottomCenter:
                case iMonLcdIcons.DiscBottomRight:
                case iMonLcdIcons.DiscMiddleRight:
                case iMonLcdIcons.DiscTopRight:
                case iMonLcdIcons.DiscCircle:
                {
                    if (this.icons[iMonLcdIcons.DiscTopCenter])
                        data[0] |= iconMasks[iMonLcdIcons.DiscTopCenter];
                    if (this.icons[iMonLcdIcons.DiscTopLeft])
                        data[0] |= iconMasks[iMonLcdIcons.DiscTopLeft];
                    if (this.icons[iMonLcdIcons.DiscMiddleLeft])
                        data[0] |= iconMasks[iMonLcdIcons.DiscMiddleLeft];
                    if (this.icons[iMonLcdIcons.DiscBottomLeft])
                        data[0] |= iconMasks[iMonLcdIcons.DiscBottomLeft];
                    if (this.icons[iMonLcdIcons.DiscBottomCenter])
                        data[0] |= iconMasks[iMonLcdIcons.DiscBottomCenter];
                    if (this.icons[iMonLcdIcons.DiscBottomRight])
                        data[0] |= iconMasks[iMonLcdIcons.DiscBottomRight];
                    if (this.icons[iMonLcdIcons.DiscMiddleRight])
                        data[0] |= iconMasks[iMonLcdIcons.DiscMiddleRight];
                    if (this.icons[iMonLcdIcons.DiscTopRight])
                        data[0] |= iconMasks[iMonLcdIcons.DiscTopRight];
                    if (this.icons[iMonLcdIcons.DiscCircle])
                        data[0] |= iconMasks[iMonLcdIcons.DiscCircle];

                    int index = 0;
                    if (icon == iMonLcdIcons.DiscCircle)
                    {
                        index = 1;
                    }

                    if (show)
                    {
                        data[index] |= iconMasks[icon];
                    }
                    else
                    {
                        data[index] &= (byte)~iconMasks[icon];
                    }

                    return iMonNativeApi.IMON_Display_SetLcdOrangeIcon(data[0], data[1]);
                }
                #endregion

                #region Media type icons
                case iMonLcdIcons.Music:
                case iMonLcdIcons.Movie:
                case iMonLcdIcons.Photo:
                case iMonLcdIcons.CdDvd:
                case iMonLcdIcons.Tv:
                case iMonLcdIcons.Webcast:
                case iMonLcdIcons.NewsWeather:
                {
                    if (this.icons[iMonLcdIcons.Music])
                        data[0] |= iconMasks[iMonLcdIcons.Music];
                    if (this.icons[iMonLcdIcons.Movie])
                        data[0] |= iconMasks[iMonLcdIcons.Movie];
                    if (this.icons[iMonLcdIcons.Photo])
                        data[0] |= iconMasks[iMonLcdIcons.Photo];
                    if (this.icons[iMonLcdIcons.CdDvd])
                        data[0] |= iconMasks[iMonLcdIcons.CdDvd];
                    if (this.icons[iMonLcdIcons.Tv])
                        data[0] |= iconMasks[iMonLcdIcons.Tv];
                    if (this.icons[iMonLcdIcons.Webcast])
                        data[0] |= iconMasks[iMonLcdIcons.Webcast];
                    if (this.icons[iMonLcdIcons.NewsWeather])
                        data[0] |= iconMasks[iMonLcdIcons.NewsWeather];

                    if (show)
                    {
                        data[0] |= iconMasks[icon];
                    }
                    else
                    {
                        data[0] &= (byte)~iconMasks[icon];
                    }

                    return iMonNativeApi.IMON_Display_SetLcdMediaTypeIcon(data[0]);
                }
                #endregion

                #region Video codec icons
                case iMonLcdIcons.VideoMPG:
                case iMonLcdIcons.VideoDivX:
                case iMonLcdIcons.VideoXviD:
                case iMonLcdIcons.VideoWMV:
                case iMonLcdIcons.VideoMPGAudio:
                case iMonLcdIcons.VideoAC3:
                case iMonLcdIcons.VideoDTS:
                case iMonLcdIcons.VideoWMA:
                {
                    if (this.icons[iMonLcdIcons.VideoMPG])
                        data[0] |= iconMasks[iMonLcdIcons.VideoMPG];
                    if (this.icons[iMonLcdIcons.VideoDivX])
                        data[0] |= iconMasks[iMonLcdIcons.VideoDivX];
                    if (this.icons[iMonLcdIcons.VideoXviD])
                        data[0] |= iconMasks[iMonLcdIcons.VideoXviD];
                    if (this.icons[iMonLcdIcons.VideoWMV])
                        data[0] |= iconMasks[iMonLcdIcons.VideoWMV];
                    if (this.icons[iMonLcdIcons.VideoMPGAudio])
                        data[0] |= iconMasks[iMonLcdIcons.VideoMPGAudio];
                    if (this.icons[iMonLcdIcons.VideoAC3])
                        data[0] |= iconMasks[iMonLcdIcons.VideoAC3];
                    if (this.icons[iMonLcdIcons.VideoDTS])
                        data[0] |= iconMasks[iMonLcdIcons.VideoDTS];
                    if (this.icons[iMonLcdIcons.VideoWMA])
                        data[0] |= iconMasks[iMonLcdIcons.VideoWMA];

                    if (show)
                    {
                        data[0] |= iconMasks[icon];
                    }
                    else
                    {
                        data[0] &= (byte)~iconMasks[icon];
                    }

                    return iMonNativeApi.IMON_Display_SetLcdVideoCodecIcon(data[0]);
                }
                #endregion

                #region Audio codec icons
                case iMonLcdIcons.AudioMP3:
                case iMonLcdIcons.AudioOGG:
                case iMonLcdIcons.AudioWMA:
                case iMonLcdIcons.AudioWAV:
                {
                    if (this.icons[iMonLcdIcons.AudioMP3])
                        data[0] |= iconMasks[iMonLcdIcons.AudioMP3];
                    if (this.icons[iMonLcdIcons.AudioOGG])
                        data[0] |= iconMasks[iMonLcdIcons.AudioOGG];
                    if (this.icons[iMonLcdIcons.AudioWMA])
                        data[0] |= iconMasks[iMonLcdIcons.AudioWMA];
                    if (this.icons[iMonLcdIcons.AudioWAV])
                        data[0] |= iconMasks[iMonLcdIcons.AudioWAV];

                    if (show)
                    {
                        data[0] |= iconMasks[icon];
                    }
                    else
                    {
                        data[0] &= (byte)~iconMasks[icon];
                    }

                    return iMonNativeApi.IMON_Display_SetLcdAudioCodecIcon(data[0]);
                }
                #endregion

                #region Aspect ratio icons
                case iMonLcdIcons.AspectRatioSource:
                case iMonLcdIcons.AspectRatioFIT:
                case iMonLcdIcons.AspectRatioTv:
                case iMonLcdIcons.AspectRatioHDTV:
                case iMonLcdIcons.AspectRatioScreen1:
                case iMonLcdIcons.AspectRatioScreen2:
                {
                    if (this.icons[iMonLcdIcons.AspectRatioSource])
                        data[0] |= iconMasks[iMonLcdIcons.AspectRatioSource];
                    if (this.icons[iMonLcdIcons.AspectRatioFIT])
                        data[0] |= iconMasks[iMonLcdIcons.AspectRatioFIT];
                    if (this.icons[iMonLcdIcons.AspectRatioTv])
                        data[0] |= iconMasks[iMonLcdIcons.AspectRatioTv];
                    if (this.icons[iMonLcdIcons.AspectRatioHDTV])
                        data[0] |= iconMasks[iMonLcdIcons.AspectRatioHDTV];
                    if (this.icons[iMonLcdIcons.AspectRatioScreen1])
                        data[0] |= iconMasks[iMonLcdIcons.AspectRatioScreen1];
                    if (this.icons[iMonLcdIcons.AspectRatioScreen2])
                        data[0] |= iconMasks[iMonLcdIcons.AspectRatioScreen2];

                    if (show)
                    {
                        data[0] |= iconMasks[icon];
                    }
                    else
                    {
                        data[0] &= (byte)~iconMasks[icon];
                    }

                    return iMonNativeApi.IMON_Display_SetLcdAspectRatioIcon(data[0]);
                }
                #endregion

                #region Other icons
                case iMonLcdIcons.Repeat:
                case iMonLcdIcons.Shuffle:
                case iMonLcdIcons.Alarm:
                case iMonLcdIcons.Recording:
                case iMonLcdIcons.Volume:
                case iMonLcdIcons.Time:
                {
                    if (this.icons[iMonLcdIcons.Repeat])
                        data[0] |= iconMasks[iMonLcdIcons.Repeat];
                    if (this.icons[iMonLcdIcons.Shuffle])
                        data[0] |= iconMasks[iMonLcdIcons.Shuffle];
                    if (this.icons[iMonLcdIcons.Alarm])
                        data[0] |= iconMasks[iMonLcdIcons.Alarm];
                    if (this.icons[iMonLcdIcons.Recording])
                        data[0] |= iconMasks[iMonLcdIcons.Recording];
                    if (this.icons[iMonLcdIcons.Volume])
                        data[0] |= iconMasks[iMonLcdIcons.Volume];
                    if (this.icons[iMonLcdIcons.Time])
                        data[0] |= iconMasks[iMonLcdIcons.Time];

                    if (show)
                    {
                        data[0] |= iconMasks[icon];
                    }
                    else
                    {
                        data[0] &= (byte)~iconMasks[icon];
                    }

                    return iMonNativeApi.IMON_Display_SetLcdEtcIcon(data[0]);
                }
                #endregion
            }

            return iMonNativeApi.iMonDisplayResult.Failed;
        }

        #endregion
    }
}