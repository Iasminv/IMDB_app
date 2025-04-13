using IMDB.Commands;
using IMDB.Services;
using IMDB.Views;
using IMDB_App.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace IMDB.ViewModels
{
    public class GenresViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private ObservableCollection<Genre> _genres = new ObservableCollection<Genre>();

        public ObservableCollection<Genre> Genres
        {
            get => _genres;
            set => SetProperty(ref _genres, value);
        }

        public ICommand BackToHomeCommand { get; }
        public ICommand SelectGenreCommand { get; }

        public GenresViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            BackToHomeCommand = new RelayCommand(NavigateToHome);
            SelectGenreCommand = new RelayCommand<Genre>(SelectGenre);

            // Data is pre-loaded by App.LoadData()
        }

        private void SelectGenre(Genre genre)
        {
            if (genre == null) return;
            _navigationService.NavigateToWithViewModel<MovieListView, MovieListViewModel>(genre.GenreId);
        }

        private void NavigateToHome()
        {
            _navigationService.NavigateToWithViewModel<HomeView, HomeViewModel>();
        }
    }
}