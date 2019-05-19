using System.Collections.Generic;
using BrainVR.Eyetracking.PupilLabs;
using BrainVR.UnityFramework.Logger;

namespace BrainVR.UnityFramework.Eyetracking.PupilLabs
{
    public class PupilSynchronizationLog : MonoLog
    {
        protected override string LogName => "pupil";
        private PupilManager _manager;
        #region Monolog functions

        protected override void OnLogSetup()
        {
            _manager = PupilManager.Instance;
            if (!_manager.IsConnected) _manager.Connect();
        }
        protected override void AfterLogSetup()
        {
            WritePupilHeader();
            WriteLogHeader();
        }
        #endregion
        #region public API
        /// <summary>
        /// Logs the pupil labs timestamp at the time of an event 
        /// </summary>
        /// <param name="eventName">Name of the event during which the timestamp was taken</param>
        public void LogPupilTimestamp(string eventName)
        {
            if (!_manager.IsConnected) return;
            var timestamp = _manager.GetTimestamp();
            WriteLine(new List<string>{eventName, timestamp.ToString()});
        }
        #endregion
        #region private functions

        private void WritePupilHeader()
        {
            Log.WriteBlock("PUPIL", "{}");
        }
        private void WriteLogHeader()
        {
            Log.WriteLine("Event;PupilTime;");
        }
        #endregion
    }
}
