using System;
using AssettoCorsaSharedMemory;

namespace TM1638Dash
{
    public class AssettoCorsaHelper : IAssettoCorsaHelper
    {
        #region Private Members

        private readonly AssettoCorsa _game;
        private readonly IUsbDeviceHelper _device;
        private bool _imperial;

        private CurrentStats _stats;

        #endregion

        #region Properties

        public bool Started { get; private set; }

        #endregion

        #region Constructors

        public AssettoCorsaHelper(AssettoCorsa game, IUsbDeviceHelper device)
        {
            _stats = new CurrentStats();
            _game = game;
            _device = device;
            _game.PhysicsUpdated += OnPhysicsUpdated;
            _game.StaticInfoUpdated += OnStaticInfoUpdated;
            _game.GraphicsUpdated += OnGraphicsUpdated;
        }

        #endregion

        private void DisplayStats()
        {
            if (_device.Started)
            {
                _device.DisplayStats(_stats);
            }
        }

        #region Event Handlers

        private void OnStaticInfoUpdated(object sender, StaticInfoEventArgs e)
        {
            _stats.MaxRPM = e.StaticInfo.MaxRpm;
        }

        private void OnPhysicsUpdated(object sender, PhysicsEventArgs e)
        {
                _stats.RPM = e.Physics.Rpms;
                _stats.Gear = e.Physics.Gear;
                _stats.Speed = (int)Math.Round(_imperial ? e.Physics.SpeedKmh*0.621371192 : e.Physics.SpeedKmh);
                DisplayStats();
        }

        private void OnGraphicsUpdated(object sender, GraphicsEventArgs e)
        {
            _stats.CompletedLaps = e.Graphics.CompletedLaps;
            _stats.Position = e.Graphics.Position;
            _stats.CurrentTime = e.Graphics.CurrentTime;
            _stats.LastTime = e.Graphics.LastTime;
            _stats.BestTime = e.Graphics.BestTime;
            _stats.Split = e.Graphics.Split;
            _stats.NumberOfLaps = e.Graphics.NumberOfLaps;
            DisplayStats();
        }

        #endregion

        #region Exposed Methods

        public void Stop()
        {
            if (!Started)
            {
                throw new InvalidOperationException("Not yet started!");
            }

            _game.Stop();
            Started = false;
        }

        public void Start(bool imperial, LEDStyle style)
        {
            if (Started)
            {
                throw new InvalidOperationException("Already started!");
            }

            _imperial = imperial;
            _stats.Style = style;

            _game.Start();
            Started = true;
        }

        #endregion

        public class CurrentStats
        {
            public LEDStyle Style { get; set; }
            public int MaxRPM { get; set; }
            public int RPM { get; set; }
            public int Gear { get; set; }
            public int Speed { get; set; }
            public int CompletedLaps { get; set; }
            public int Position { get; set; }
            public string CurrentTime { get; set; }
            public string LastTime { get; set; }
            public string BestTime { get; set; }
            public string Split { get; set; }
            public int NumberOfLaps { get; set; }
        }
    }

    public interface IAssettoCorsaHelper
    {
        #region Abstract Properties

        bool Started { get; }

        #endregion

        #region Abstract Methods

        void Stop();
        void Start(bool imperial, LEDStyle style);

        #endregion
    }
}