using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kilolani_WebIO_Power_Controller
{
    public class IOCfg
    {

        ///GPIOCfg
        //Common constants for I/O codes, commands and channels

        //'Power relay channels
        //'  Channels 8-11 are 120 volt AC for individual power supplies
        //'  Channels 12-15 are 12 volt DC from high power 12V supply 
        //'
        //'  Note that for some reason, AC #2 (Channel 9) seems to be set
        //'   (turned on) upon power up.  All other channels are unset.
        //'

        //'Control relay assignments
        public const int MountChannel =0;           //AC #1
        public const int AllSkyCamChannel = 1;       //AC#2 - turned on upon power up
        public const int CamerasChannel = 2;        //AC #3
        public const int FlatChannel = 3;           //AC #4
        public const int FanChannel = 4;            //DC #4
        public const int DewChannel = 5;            //DC #3
        public const int FocuserChannel = 6;        //DC #2
        public const int DomeChannel =7;           //DC #1

        //Button Names
        public const string DomeChannelName = "Dome";
        public const string MountChannelName = "Mount";
        public const string CamerasChannelName = "Imager";
        public const string FocuserChannelName = "Focuser/Rotator";
        public const string DewChannelName = "Dew Control";
        public const string FanChannelName = "Fan";
        public const string FlatChannelName = "FlatMan";
        public const string AllSkyCamName = "AllSky";

        //Button Name configuration
        public const string PowerButton1Name = DomeChannelName;
        public const string PowerButton2Name = MountChannelName;
        public const string PowerButton3Name = CamerasChannelName;
        public const string PowerButton4Name = FocuserChannelName;
        public const string PowerButton5Name = DewChannelName;
        public const string PowerButton6Name = FanChannelName;
        public const string PowerButton7Name = FlatChannelName;
        public const string PowerButton8Name = AllSkyCamName;

        //Button channel configuration
        public const int PowerButton1Channel = DomeChannel;
        public const int PowerButton2Channel = MountChannel;
        public const int PowerButton3Channel = CamerasChannel;
        public const int PowerButton4Channel = FocuserChannel;
        public const int PowerButton5Channel = DewChannel;
        public const int PowerButton6Channel = FanChannel;
        public const int PowerButton7Channel = FlatChannel;
        public const int PowerButton8Channel = AllSkyCamChannel;

        //Button imaging configuration
        public const bool PowerButton1ChannelOn = true;
        public const bool PowerButton2ChannelOn = true;
        public const bool PowerButton3ChannelOn = true;
        public const bool PowerButton4ChannelOn = true;
        public const bool PowerButton5ChannelOn = true;
        public const bool PowerButton6ChannelOn = true;
        public const bool PowerButton7ChannelOn = true;
        public const bool PowerButton8ChannelOn = true;

        //Button standby configuration
        public const bool PowerButton1ChannelStby = true;
        public const bool PowerButton2ChannelStby = false;
        public const bool PowerButton3ChannelStby = false;
        public const bool PowerButton4ChannelStby = false;
        public const bool PowerButton5ChannelStby = false;
        public const bool PowerButton6ChannelStby = false;
        public const bool PowerButton7ChannelStby = false;
        public const bool PowerButton8ChannelStby = true;


    }
}
