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
    [Flags]
    public enum iMonDisplayType
    {
        None    = 0,
        VFD     = 1,
        LCD     = 2,
        Unknown = 4 
    }
}