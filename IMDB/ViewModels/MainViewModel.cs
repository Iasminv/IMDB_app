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
            ShowMoviesCommand = new RelayCommand(() => NavigateToMovies("movie"));
            ShowTVShowsCommand = new RelayCommand(() => NavigateToMovies("tvSeries"));
            ShowActorsCommand = new RelayCommand(NavigateToActors);
            ShowGenresCommand = new RelayCommand(NavigateToGenres);
        }

        private void NavigateToHome()
        {
            _navigationService.NavigateToWithViewModel<HomeView, HomeViewModel>();
        }

        private void NavigateToMovies(string titleType)
        {
            _navigationService.NavigateToWithViewModel<MovieDetailsView, MovieDetailsViewModel>(titleType);
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