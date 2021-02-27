﻿using Midi;

namespace Jlabarca.BeatsTower.Tempo.Components
{
    internal struct TempoEvent
    {
        public SoundEvent SoundEvent;
        public float ReleaseTime; //Event emit time, should be MidiEvent.time + GameState.timeOffset
    }
}
