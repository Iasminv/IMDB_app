using IMDB.Commands;
using IMDB.Services;
using IMDB.Views;
using System.Windows.Controls;
using System.Windows.Input;

namespace IMDB.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private UserControl _currentView;

        public UserControl CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public ICommand CloseCommand { get; }
        public ICommand ShowHomeCommand { get; }
        public ICommand ShowMoviesCommand { get; }
        public ICommand ShowTVShowsCommand { get; }
        public ICommand ShowActorsCommand { get; }
        public ICommand ShowGenresCommand { get; }

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Subscribe to navigation service view change event
            _navigationService.CurrentViewChanged += (sender, view) => CurrentView = view;

            // Initialize commands
            CloseCommand = new RelayCommand(() => System.Windows.Application.Current.Shutdown());
            ShowHomeCommand = new RelayCommand(NavigateToHome);
            // Modified here: navigate to list view instead of detail view
            ShowMoviesCommand = new RelayCommand(() => NavigateToMoviesList("movie"));
            ShowTVShowsCommand = new RelayCommand(() => NavigateToMoviesList("tvSeries"));
            ShowActorsCommand = new RelayCommand(NavigateToActors);
            ShowGenresCommand = new RelayCommand(NavigateToGenres);

            // Navigate to home on initialization
            NavigateToHome();
        }

        private void NavigateToHome()
        {
            _navigationService.NavigateToWithViewModel<HomeView, HomeViewModel>();
        }

        // Modified method: navigate to movie list instead of details
        private void NavigateToMoviesList(string titleType)
        {
            _navigationService.NavigateToWithViewModel<MovieListView, MovieListViewModel>(titleType);
        }

        private void NavigateToActors()
        {
            _navigationService.NavigateToWithViewModel<ActorsView, ActorsViewModel>();
        }

        private void NavigateToGenres()
        {
            _navigationService.NavigateToWithViewModel<GenresView, GenresViewModel>();
        }
    }
}