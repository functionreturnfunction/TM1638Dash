using System;
using AssettoCorsaSharedMemory;

namespace SimDash.UI
{
    public class AssettoCorsaHelper : IAssettoCorsaHelper
    {
        #region Private Members

        private readonly AssettoCorsa _game;
        private readonly IUsbDeviceHelper _device;
        private int _currentMaxRpm;
        private bool _started;

        #endregion

        #region Constructors

        public AssettoCorsaHelper(AssettoCorsa game, IUsbDeviceHelper device)
        {
            _game = game;
            _device = device;
            _game.PhysicsUpdated += OnPhysicsUpdated;
            _game.StaticInfoUpdated += OnStaticInfoUpdated;
        }

        #endregion

        #region Event Handlers

        private void OnStaticInfoUpdated(object sender, StaticInfoEventArgs e)
        {
            _currentMaxRpm = e.StaticInfo.MaxRpm;
        }

        private void OnPhysicsUpdated(object sender, PhysicsEventArgs e)
        {
            if (_device.Started)
            {
                _device.DisplayStats(_currentMaxRpm, e.Physics.Rpms, e.Physics.Gear, e.Physics.SpeedKmh);
            }
        }

        #endregion

        #region Exposed Methods

        public void Stop()
        {
            if (!_started)
            {
                throw new InvalidOperationException("Not yet started!");
            }

            _game.Stop();
            _started = false;
        }

        public void Start()
        {
            if (_started)
            {
                throw new InvalidOperationException("Already started!");
            }

            _game.Start();
            _started = true;
        }

        #endregion
    }

    public interface IAssettoCorsaHelper
    {
        #region Abstract Methods

        void Stop();
        void Start();

        #endregion
    }
}