using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SoccerProbability.Computation;

namespace SoccerProbability
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var goals = new[] {GoalType.Host, GoalType.Guest , GoalType.Guest};
            var interval = new GoalsInterval(4, 6);
            const double meanIntensityHost = 1.7262 / Constants.MinutesPerMatch;
            const double meanIntensityGuest = 1.3758 / Constants.MinutesPerMatch;


            var inputData = new InputData(90, goals, interval, meanIntensityHost, meanIntensityGuest);
            var probs = ProbModel.ComputeProbs(inputData);
            var sp = probs.HostsWonProb + probs.GuestsWonProb + probs.DrawProb + probs.NotFinishedProb;
        }
    }
}
