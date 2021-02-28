using System.Collections.Generic;
using System.Linq;
using Jlabarca.BeatsTower.Core.Tempo.Components;
using Jlabarca.BeatsTower.Core.Tempo.Tools;
using Leopotam.Ecs;
using UnityEngine;

namespace Jlabarca.BeatsTower.Core.Tempo.Systems
{
    public class TempoEmitSystem : IEcsRunSystem, IEcsInitSystem
    {
        private const string MidiFilePath = "Assets/Tempo/Midi/wat.mid";

        private readonly EcsWorld _world = default;
        private readonly AudioSource _audioSource = default;
        private readonly GameState _gameState = default;

        private List<MidiEvent> _midiData;
        private float _startTime;
        private int _midiEventIndex;

        public void Init()
        {
            _midiData = GetMidiFile(MidiFilePath);
            _audioSource.Play();
            _audioSource.time = Time.time;
            _startTime = Time.time;
        }

        public void Run()
        {
            var time = _audioSource.time;
            _gameState.time = time;
            var preFireTime = time + _gameState.timeOffset;

            while (_midiEventIndex < _midiData.Count && _midiData[_midiEventIndex].TimeInSeconds < preFireTime)
            {
                Debug.Log($"{_midiData[_midiEventIndex]}");
                var actorEntity = _world.NewEntity();

                actorEntity.Replace(new TempoEvent
                {
                    MidiEvent = _midiData[_midiEventIndex],
                    ReleaseTime = _gameState.time,
                });

                _midiEventIndex++;
            }
        }

        private static List<MidiEvent> GetMidiFile(string fileName)
        {
            var path = $"{MidiFilePath}/{fileName}";
            var midiFile = new MidiFile(MidiFilePath);
            Debug.Log(midiFile.Format);
            Debug.Log(midiFile.PulsesPerQuarter);
            Debug.Log(midiFile.BeatsPerMinute);
            foreach (var midiEvent in midiFile.Tracks[0].MidiEvents)
                Debug.Log(midiEvent);

            return midiFile.Tracks[1].MidiEvents.Where(e => e.MidiEventType == MidiEventType.NoteOn).ToList();
        }
    }
}
