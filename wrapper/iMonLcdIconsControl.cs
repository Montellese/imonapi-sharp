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

        private static Dictionary<iMonIcons, byte> iconMasks;

        private iMonWrapperApi wrapper;

        private Dictionary<iMonIcons, bool> icons;

        #endregion

        #region Public variables

        public bool this[iMonIcons icon]
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
            iconMasks = new Dictionary<iMonIcons, byte>(Enum.GetValues(typeof(iMonIcons)).Length);

            // Disc icons
            iconMasks.Add(iMonIcons.DiscTopCenter, 0x80);
            iconMasks.Add(iMonIcons.DiscTopLeft, 0x40);
            iconMasks.Add(iMonIcons.DiscMiddleLeft, 0x20);
            iconMasks.Add(iMonIcons.DiscBottomLeft, 0x10);
            iconMasks.Add(iMonIcons.DiscBottomCenter, 0x08);
            iconMasks.Add(iMonIcons.DiscBottomRight, 0x04);
            iconMasks.Add(iMonIcons.DiscMiddleRight, 0x02);
            iconMasks.Add(iMonIcons.DiscTopRight, 0x01);
            iconMasks.Add(iMonIcons.DiscCircle, 0x80);

            // Media type icons
            iconMasks.Add(iMonIcons.Music, 0x80);
            iconMasks.Add(iMonIcons.Movie, 0x40);
            iconMasks.Add(iMonIcons.Photo, 0x20);
            iconMasks.Add(iMonIcons.CdDvd, 0x10);
            iconMasks.Add(iMonIcons.Tv, 0x08);
            iconMasks.Add(iMonIcons.Webcast, 0x04);
            iconMasks.Add(iMonIcons.NewsWeather, 0x02);

            // Speaker icons
            iconMasks.Add(iMonIcons.SpeakerFrontLeft, 0x80);
            iconMasks.Add(iMonIcons.SpeakerCenter, 0x40);
            iconMasks.Add(iMonIcons.SpeakerFrontRight, 0x20);
            iconMasks.Add(iMonIcons.SpeakerSideLeft, 0x10);
            iconMasks.Add(iMonIcons.SpeakerLFE, 0x08);
            iconMasks.Add(iMonIcons.SpeakerSideRight, 0x04);
            iconMasks.Add(iMonIcons.SpeakerRearLeft, 0x02);
            iconMasks.Add(iMonIcons.SpeakerSPDIF, 0x01);
            iconMasks.Add(iMonIcons.SpeakerRearRight, 0x80);

            // Video codec icons
            iconMasks.Add(iMonIcons.VideoMPG, 0x80);
            iconMasks.Add(iMonIcons.VideoDivX, 0x40);
            iconMasks.Add(iMonIcons.VideoXviD, 0x20);
            iconMasks.Add(iMonIcons.VideoWMV, 0x10);
            iconMasks.Add(iMonIcons.VideoMPGAudio, 0x08);
            iconMasks.Add(iMonIcons.VideoAC3, 0x04);
            iconMasks.Add(iMonIcons.VideoDTS, 0x02);
            iconMasks.Add(iMonIcons.VideoWMA, 0x01);

            // Audio codec icons
            iconMasks.Add(iMonIcons.AudioMP3, 0x80);
            iconMasks.Add(iMonIcons.AudioOGG, 0x40);
            iconMasks.Add(iMonIcons.AudioWMA, 0x20);
            iconMasks.Add(iMonIcons.AudioWAV, 0x10);

            // Aspect ratio icons
            iconMasks.Add(iMonIcons.AspectRatioSource, 0x80);
            iconMasks.Add(iMonIcons.AspectRatioFIT, 0x40);
            iconMasks.Add(iMonIcons.AspectRatioTv, 0x20);
            iconMasks.Add(iMonIcons.AspectRatioHDTV, 0x10);
            iconMasks.Add(iMonIcons.AspectRatioScreen1, 0x08);
            iconMasks.Add(iMonIcons.AspectRatioScreen2, 0x04);

            // Other icons
            iconMasks.Add(iMonIcons.Repeat, 0x80);
            iconMasks.Add(iMonIcons.Shuffle, 0x40);
            iconMasks.Add(iMonIcons.Alarm, 0x20);
            iconMasks.Add(iMonIcons.Recording, 0x10);
            iconMasks.Add(iMonIcons.Volume, 0x08);
            iconMasks.Add(iMonIcons.Time, 0x04);
        }

        internal iMonLcdIconsControl(iMonWrapperApi wrapper)
        {
            if (wrapper == null)
            {
                throw new ArgumentNullException("wrapper");
            }

            this.wrapper = wrapper;

            this.icons = new Dictionary<iMonIcons, bool>(Enum.GetValues(typeof(iMonIcons)).Length);
            foreach (iMonIcons icon in Enum.GetValues(typeof(iMonIcons)))
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
                    iMonIcons[] iconArray = new iMonIcons[this.icons.Count];
                    this.icons.Keys.CopyTo(iconArray, 0);
                    foreach (iMonIcons icon in iconArray)
                    {
                        this.icons[icon] = show;
                    } 
                    return true;

                case iMonNativeApi.iMonDisplayResult.NotInitialized:
                    throw new InvalidOperationException("The display is not initialized");
            }

            return false;
        }

        public bool Show(iMonIcons icon)
        {
            return this.Set(icon, true);
        }

        public bool Show(IEnumerable<iMonIcons> iconList)
        {
            return this.Set(iconList, true);
        }

        public bool Hide(iMonIcons icon)
        {
            return this.Set(icon, false);
        }

        public bool Hide(IEnumerable<iMonIcons> iconList)
        {
            return this.Set(iconList, false);
        }

        public bool Set(iMonIcons icon, bool show)
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

        public bool Set(IEnumerable<iMonIcons> iconList, bool show)
        {
            bool result = true;

            foreach (iMonIcons icon in iconList)
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
                iMonIcons[] iconArray = new iMonIcons[this.icons.Count];
                this.icons.Keys.CopyTo(iconArray, 0);
                foreach (iMonIcons icon in iconArray)
                {
                    this.icons[icon] = false;
                }
            }
        }

        #endregion

        #region Private functions

        private iMonNativeApi.iMonDisplayResult set(iMonIcons icon, bool show)
        {
            byte[] data = new byte[2];

            switch (icon)
            {
                #region Sound icons
                case iMonIcons.SpeakerCenter:
                case iMonIcons.SpeakerFrontLeft:
                case iMonIcons.SpeakerFrontRight:
                case iMonIcons.SpeakerLFE:
                case iMonIcons.SpeakerRearLeft:
                case iMonIcons.SpeakerRearRight:
                case iMonIcons.SpeakerSideLeft:
                case iMonIcons.SpeakerSideRight:
                case iMonIcons.SpeakerSPDIF:
                {
                    if (this.icons[iMonIcons.SpeakerCenter])
                        data[0] |= iconMasks[iMonIcons.SpeakerCenter];
                    if (this.icons[iMonIcons.SpeakerFrontLeft])
                        data[0] |= iconMasks[iMonIcons.SpeakerFrontLeft];
                    if (this.icons[iMonIcons.SpeakerFrontRight])
                        data[0] |= iconMasks[iMonIcons.SpeakerFrontRight];
                    if (this.icons[iMonIcons.SpeakerLFE])
                        data[0] |= iconMasks[iMonIcons.SpeakerLFE];
                    if (this.icons[iMonIcons.SpeakerRearLeft])
                        data[0] |= iconMasks[iMonIcons.SpeakerRearLeft];
                    if (this.icons[iMonIcons.SpeakerRearRight])
                        data[1] |= iconMasks[iMonIcons.SpeakerRearRight];
                    if (this.icons[iMonIcons.SpeakerSideLeft])
                        data[0] |= iconMasks[iMonIcons.SpeakerSideLeft];
                    if (this.icons[iMonIcons.SpeakerSideRight])
                        data[0] |= iconMasks[iMonIcons.SpeakerSideRight];
                    if (this.icons[iMonIcons.SpeakerSPDIF])
                        data[0] |= iconMasks[iMonIcons.SpeakerSPDIF];

                    int index = 0;
                    if (icon == iMonIcons.SpeakerRearRight)
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
                case iMonIcons.DiscTopCenter:
                case iMonIcons.DiscTopLeft:
                case iMonIcons.DiscMiddleLeft:
                case iMonIcons.DiscBottomLeft:
                case iMonIcons.DiscBottomCenter:
                case iMonIcons.DiscBottomRight:
                case iMonIcons.DiscMiddleRight:
                case iMonIcons.DiscTopRight:
                case iMonIcons.DiscCircle:
                {
                    if (this.icons[iMonIcons.DiscTopCenter])
                        data[0] |= iconMasks[iMonIcons.DiscTopCenter];
                    if (this.icons[iMonIcons.DiscTopLeft])
                        data[0] |= iconMasks[iMonIcons.DiscTopLeft];
                    if (this.icons[iMonIcons.DiscMiddleLeft])
                        data[0] |= iconMasks[iMonIcons.DiscMiddleLeft];
                    if (this.icons[iMonIcons.DiscBottomLeft])
                        data[0] |= iconMasks[iMonIcons.DiscBottomLeft];
                    if (this.icons[iMonIcons.DiscBottomCenter])
                        data[0] |= iconMasks[iMonIcons.DiscBottomCenter];
                    if (this.icons[iMonIcons.DiscBottomRight])
                        data[0] |= iconMasks[iMonIcons.DiscBottomRight];
                    if (this.icons[iMonIcons.DiscMiddleRight])
                        data[0] |= iconMasks[iMonIcons.DiscMiddleRight];
                    if (this.icons[iMonIcons.DiscTopRight])
                        data[0] |= iconMasks[iMonIcons.DiscTopRight];
                    if (this.icons[iMonIcons.DiscCircle])
                        data[0] |= iconMasks[iMonIcons.DiscCircle];

                    int index = 0;
                    if (icon == iMonIcons.DiscCircle)
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
                case iMonIcons.Music:
                case iMonIcons.Movie:
                case iMonIcons.Photo:
                case iMonIcons.CdDvd:
                case iMonIcons.Tv:
                case iMonIcons.Webcast:
                case iMonIcons.NewsWeather:
                {
                    if (this.icons[iMonIcons.Music])
                        data[0] |= iconMasks[iMonIcons.Music];
                    if (this.icons[iMonIcons.Movie])
                        data[0] |= iconMasks[iMonIcons.Movie];
                    if (this.icons[iMonIcons.Photo])
                        data[0] |= iconMasks[iMonIcons.Photo];
                    if (this.icons[iMonIcons.CdDvd])
                        data[0] |= iconMasks[iMonIcons.CdDvd];
                    if (this.icons[iMonIcons.Tv])
                        data[0] |= iconMasks[iMonIcons.Tv];
                    if (this.icons[iMonIcons.Webcast])
                        data[0] |= iconMasks[iMonIcons.Webcast];
                    if (this.icons[iMonIcons.NewsWeather])
                        data[0] |= iconMasks[iMonIcons.NewsWeather];

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
                case iMonIcons.VideoMPG:
                case iMonIcons.VideoDivX:
                case iMonIcons.VideoXviD:
                case iMonIcons.VideoWMV:
                case iMonIcons.VideoMPGAudio:
                case iMonIcons.VideoAC3:
                case iMonIcons.VideoDTS:
                case iMonIcons.VideoWMA:
                {
                    if (this.icons[iMonIcons.VideoMPG])
                        data[0] |= iconMasks[iMonIcons.VideoMPG];
                    if (this.icons[iMonIcons.VideoDivX])
                        data[0] |= iconMasks[iMonIcons.VideoDivX];
                    if (this.icons[iMonIcons.VideoXviD])
                        data[0] |= iconMasks[iMonIcons.VideoXviD];
                    if (this.icons[iMonIcons.VideoWMV])
                        data[0] |= iconMasks[iMonIcons.VideoWMV];
                    if (this.icons[iMonIcons.VideoMPGAudio])
                        data[0] |= iconMasks[iMonIcons.VideoMPGAudio];
                    if (this.icons[iMonIcons.VideoAC3])
                        data[0] |= iconMasks[iMonIcons.VideoAC3];
                    if (this.icons[iMonIcons.VideoDTS])
                        data[0] |= iconMasks[iMonIcons.VideoDTS];
                    if (this.icons[iMonIcons.VideoWMA])
                        data[0] |= iconMasks[iMonIcons.VideoWMA];

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
                case iMonIcons.AudioMP3:
                case iMonIcons.AudioOGG:
                case iMonIcons.AudioWMA:
                case iMonIcons.AudioWAV:
                {
                    if (this.icons[iMonIcons.AudioMP3])
                        data[0] |= iconMasks[iMonIcons.AudioMP3];
                    if (this.icons[iMonIcons.AudioOGG])
                        data[0] |= iconMasks[iMonIcons.AudioOGG];
                    if (this.icons[iMonIcons.AudioWMA])
                        data[0] |= iconMasks[iMonIcons.AudioWMA];
                    if (this.icons[iMonIcons.AudioWAV])
                        data[0] |= iconMasks[iMonIcons.AudioWAV];

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
                case iMonIcons.AspectRatioSource:
                case iMonIcons.AspectRatioFIT:
                case iMonIcons.AspectRatioTv:
                case iMonIcons.AspectRatioHDTV:
                case iMonIcons.AspectRatioScreen1:
                case iMonIcons.AspectRatioScreen2:
                {
                    if (this.icons[iMonIcons.AspectRatioSource])
                        data[0] |= iconMasks[iMonIcons.AspectRatioSource];
                    if (this.icons[iMonIcons.AspectRatioFIT])
                        data[0] |= iconMasks[iMonIcons.AspectRatioFIT];
                    if (this.icons[iMonIcons.AspectRatioTv])
                        data[0] |= iconMasks[iMonIcons.AspectRatioTv];
                    if (this.icons[iMonIcons.AspectRatioHDTV])
                        data[0] |= iconMasks[iMonIcons.AspectRatioHDTV];
                    if (this.icons[iMonIcons.AspectRatioScreen1])
                        data[0] |= iconMasks[iMonIcons.AspectRatioScreen1];
                    if (this.icons[iMonIcons.AspectRatioScreen2])
                        data[0] |= iconMasks[iMonIcons.AspectRatioScreen2];

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
                case iMonIcons.Repeat:
                case iMonIcons.Shuffle:
                case iMonIcons.Alarm:
                case iMonIcons.Recording:
                case iMonIcons.Volume:
                case iMonIcons.Time:
                {
                    if (this.icons[iMonIcons.Repeat])
                        data[0] |= iconMasks[iMonIcons.Repeat];
                    if (this.icons[iMonIcons.Shuffle])
                        data[0] |= iconMasks[iMonIcons.Shuffle];
                    if (this.icons[iMonIcons.Alarm])
                        data[0] |= iconMasks[iMonIcons.Alarm];
                    if (this.icons[iMonIcons.Recording])
                        data[0] |= iconMasks[iMonIcons.Recording];
                    if (this.icons[iMonIcons.Volume])
                        data[0] |= iconMasks[iMonIcons.Volume];
                    if (this.icons[iMonIcons.Time])
                        data[0] |= iconMasks[iMonIcons.Time];

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