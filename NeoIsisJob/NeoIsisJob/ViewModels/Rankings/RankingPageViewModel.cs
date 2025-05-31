using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NeoIsisJob.ViewModels.Rankings
{
    public class RankingPageViewModel : INotifyPropertyChanged
    {
        private List<RankingItemViewModel> _rankings = new List<RankingItemViewModel>();
        private int _currentUserRank;
        private int _currentUserPoints;
        private bool _hasRankings;

        public List<RankingItemViewModel> Rankings
        {
            get => _rankings;
            set => SetProperty(ref _rankings, value);
        }

        public int CurrentUserRank
        {
            get => _currentUserRank;
            set => SetProperty(ref _currentUserRank, value);
        }

        public int CurrentUserPoints
        {
            get => _currentUserPoints;
            set => SetProperty(ref _currentUserPoints, value);
        }

        public bool HasRankings
        {
            get => _hasRankings;
            set => SetProperty(ref _hasRankings, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
} 