using IMDB.Commands;
using IMDB.Services;
using IMDB.Views;
using IMDB_App.Data;
using IMDB_App.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace IMDB.ViewModels
{
    public class MovieListViewModel : ViewModelBase, IParameterizedViewModel
    {
        private readonly INavigationService _navigationService;
        private int _genreId;
        private ObservableCollection<Title> _titles = new ObservableCollection<Title>();
        private string _genreName;

        public ObservableCollection<Title> Titles
        {
            get => _titles;
            set => SetProperty(ref _titles, value);
        }

        public string GenreName
        {
            get => _genreName;
            set => SetProperty(ref _genreName, value);
        }

        public ICommand BackToGenresCommand { get; }
        public ICommand SelectTitleCommand { get; }

        public MovieListViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            BackToGenresCommand = new RelayCommand(NavigateToGenres);
            SelectTitleCommand = new RelayCommand<Title>(SelectTitle);
        }

        public void Initialize(object parameter)
        {
            if (parameter is int genreId)
            {
                _genreId = genreId;
                LoadGenreMovies();
            }
        }

        private void LoadGenreMovies()
        {
            using (var context = new ImdbContext())
            {
                var genre = context.Genres.FirstOrDefault(g => g.GenreId == _genreId);
                if (genre != null)
                {
                    GenreName = genre.Name;
                }

                // Get movies for this genre
                var titlesQuery = context.Titles
                    .Where(t => t.Genres.Any(g => g.GenreId == _genreId))
                    .Include(t => t.Rating)
                    .OrderByDescending(t => t.Rating.AverageRating)
                    .Take(50)
                    .ToList();

                Titles = new ObservableCollection<Title>(titlesQuery);
            }
        }

        private void SelectTitle(Title title)
        {
            if (title == null) return;
            _navigationService.NavigateToWithViewModel<MovieDetailsView, MovieDetailsViewModel>(title.TitleId);
        }

        private void NavigateToGenres()
        {
            _navigationService.NavigateToWithViewModel<GenresView, GenresViewModel>();
        }
    }
}