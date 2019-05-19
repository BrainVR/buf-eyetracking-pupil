using System;
using System.Collections.Generic;
using BrainVR.Eyetracking.PupilLabs;
using BrainVR.UnityFramework.Eyetracking.PupilLabs;
using BrainVR.UnityFramework.Logger;
using BrainVR.UnityFramework.Objects.Goals;
using BrainVR.UnityFramework.Scripts.Experiments.DemoExperiment;
using BrainVR.UnityFramework.Scripts.Objects.Beeper;
using BrainVR.UnityFramework.Scripts.Objects.Goals;
using BrainVR.UnityFramework.UI.InGame;
using UnityEngine;

namespace BrainVR.UnityFramework.Experiment.Demo
{
    public class DemoExperimentEyetracking : BaseExperiment
    {
        //maybe more to some other abstract class
        protected BeeperManager BeepManager;
        private ExperimentCanvasManager _canvas;
        protected float TrialStartTime;
        protected float TrialEndTime;
        protected List<float> TrialTimes = new List<float>();
        public new DemoExperimentSettings Settings;
        private GoalController _currentGoal;
        private PupilManager _pupilManager;
        private PupilSynchronizationLog _synchronizationLog;
        #region Forced API
        public override void AddSettings(ExperimentSettings settings)
        {
            Settings = (DemoExperimentSettings)settings;
        }
        #endregion
        #region BaseExperiment Logic
        protected override void OnExperimentInitialise()
        {
            _canvas = ExperimentCanvasManager.Instance;
            ///// CONNECTING TO THE PUPIL ///////
            _pupilManager = PupilManager.Instance;
            _pupilManager.Connect();
        }
        protected override void AfterExperimentInitialise()
        {
            _pupilManager.SetTimestamp(0);
        }
        protected override void OnExperimentSetup()
        {
            _canvas.Show();
            _synchronizationLog = new PupilSynchronizationLog();
            _synchronizationLog.Setup(id: ExperimentInfo.Instance.Participant.Id);
            GoalManager.Instance.HideAll();
        }
        protected override void AfterTrialSetup()
        {
            StartTrial();
        }
        protected override void OnTrialStart()
        {
            TrialStartTime = Time.realtimeSinceStartup;
            _currentGoal = GoalManager.Instance.GetGoal(TrialNumber);
            _currentGoal.Show();
            _currentGoal.OnEnter += GoalEntered;
            _currentGoal.SetColor(Color.green);
            _synchronizationLog.LogPupilTimestamp("TrialStart");
        }
        private void GoalEntered(GoalController sender, EventArgs e)
        {
            sender.OnEnter -= GoalEntered;
            sender.Hide();
            FinishTrial();
        }
        protected override void OnTrialFinished()
        {
            TrialEndTime = Time.realtimeSinceStartup;
            TrialTimes.Add(TrialEndTime - TrialStartTime);
        }
        protected override void AfterTrialFinished()
        {
            NextTrial();
        }
        protected override void OnExperimentClosed()
        {
            _canvas.Show(false);
        }
        public override string ExperimentHeaderLog()
        {
            return "";
        }
        protected override bool CheckForEnd()
        {
            return TrialNumber >= Settings.GoalOrder.Length - 1;
        }
        #endregion
        #region Helpers
        #endregion
    }
}
