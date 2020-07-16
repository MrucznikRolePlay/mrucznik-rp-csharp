using System;
using System.Timers;
using SampSharp.GameMode.World;

namespace Mrucznik
{
    public class RealTime
    {
        private readonly Timer _realWorldTimeTimer;
        
        public RealTime(BasePlayer player) 
        {
            _realWorldTimeTimer = new Timer(1000);
            _realWorldTimeTimer.Elapsed += (sender, args) =>
            {
                player.SetTime(DateTime.Now.Hour, DateTime.Now.Minute);
            };
            _realWorldTimeTimer.AutoReset = true;
            
            player.Connected += (sender, args) => 
                _realWorldTimeTimer.Start();
            
            player.Disconnected += (sender, args) =>   
                _realWorldTimeTimer.Stop();
        }
    }
}