﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SoccerProbability.Annotations;
using SoccerProbability.Computation;
using SoccerProbability.DataPreprocessing;

namespace SoccerProbability
{
    class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<GoalType> Goals { get; set; } = new ObservableCollection<GoalType>();
        public int MinutesTillEnd { get; set; } = Constants.MinutesPerMatch;
        public GoalsInterval Interval { get; set; } = new GoalsInterval();

        private double _hostsWinProb;

        public double HostsWinProb
        {
            get => _hostsWinProb;
            set
            {
                _hostsWinProb = value;
                OnPropertyChanged();
            }
        }

        private double _guestsWinProb;

        public double GuestsWinProb
        {
            get => _guestsWinProb;
            set
            {
                _guestsWinProb = value;
                OnPropertyChanged();
            }
        }

        private double _drawProb;

        public double DrawProb
        {
            get => _drawProb;
            set
            {
                _drawProb = value;
                OnPropertyChanged();
            }
        }

        private double _notFinishedProb;

        public double NotFinishedProb
        {
            get => _notFinishedProb;
            set
            {
                _notFinishedProb = value;
                OnPropertyChanged();
            }
        }

        public void AddGuestGoal()
        {
            Goals.Add(GoalType.Guest);
        }

        public void AddHostGoal()
        {
            Goals.Add(GoalType.Host);
        }

        public void Calculate()
        {
            var data = StatsData.MatchStats;
            var (meanGuest,meanHost) = ComputeStats.ComputeMeans(data);
            var inputData = new InputData(MinutesTillEnd, Goals, Interval, meanGuest, meanHost);
            var result = ProbModel.ComputeProbs(inputData);
            HostsWinProb = result.HostsWonProb;
            GuestsWinProb = result.GuestsWonProb;
            DrawProb = result.DrawProb;
            NotFinishedProb = result.NotFinishedProb;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
