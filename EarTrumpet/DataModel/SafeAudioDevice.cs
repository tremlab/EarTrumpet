﻿using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EarTrumpet.DataModel
{
    // Avoid device invalidation COMExceptions from bubbling up out of devices that have been removed.
    public class SafeAudioDevice : IAudioDevice
    {
        IAudioDevice _device;

        public SafeAudioDevice(IAudioDevice device)
        {
            _device = device;
            _device.PropertyChanged += Device_PropertyChanged;
        }

        private void Device_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public ObservableCollection<IAudioDeviceSession> Sessions => SafeCallHelper.GetValue(() => _device.Sessions);

        public string DisplayName => SafeCallHelper.GetValue(() => _device.DisplayName);

        public string Id => SafeCallHelper.GetValue(() => _device.Id);

        public bool IsMuted { get => SafeCallHelper.GetValue(() => _device.IsMuted); set => SafeCallHelper.SetValue(() => _device.IsMuted = value); }
        public float Volume { get => SafeCallHelper.GetValue(() => _device.Volume); set => SafeCallHelper.SetValue(() => _device.Volume = value); }

        public float PeakValue => SafeCallHelper.GetValue(() => _device.PeakValue);

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
