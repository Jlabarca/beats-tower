using Jlabarca.BeatsTower.Core.Tempo.Tools;

namespace Jlabarca.BeatsTower.Core.Tempo.Components
{
    internal struct TempoEvent
    {
        public MidiEvent MidiEvent;
        public float ReleaseTime; //Event emit time, should be MidiEvent.time + GameState.timeOffset
    }
}
