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
        private ObservableCollection<Title> _titles = new ObservableCollection<Title>();
        private string _listTitle;
        private string _searchText;
        private bool _isLoading;
        private object _parameter; // Store the passed parameter (genre ID or titleType)

        public ObservableCollection<Title> Titles
        {
            get => _titles;
            set => SetProperty(ref _titles, value);
        }

        public string ListTitle
        {
            get => _listTitle;
            set => SetProperty(ref _listTitle, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                   
                    PerformDynamicSearch();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand BackCommand { get; }
        public ICommand SelectTitleCommand { get; }
        public ICommand SearchCommand { get; }

        public MovieListViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            BackCommand = new RelayCommand(NavigateBack);
            SelectTitleCommand = new RelayCommand<Title>(SelectTitle);
            SearchCommand = new RelayCommand(Search);
        }

        public void Initialize(object parameter)
        {
            _parameter = parameter;

            if (parameter is int genreId)
            {
                LoadGenreMovies(genreId);
            }
            else if (parameter is string titleTypeParam)
            {
                LoadTitlesByType(titleTypeParam);
            }
        }

        private void LoadGenreMovies(int genreId)
        {
            IsLoading = true;
            using (var context = new ImdbContext())
            {
                var genre = context.Genres.FirstOrDefault(g => g.GenreId == genreId);
                if (genre != null)
                {
                    ListTitle = $"{genre.Name} Movies & Shows";
                }

                // Get movies for this genre
                var titlesQuery = context.Titles
                    .Where(t => t.Genres.Any(g => g.GenreId == genreId))
                    .Include(t => t.Rating)
                    .OrderByDescending(t => t.Rating.AverageRating)
                    .Take(50)
                    .ToList();

                Titles = new ObservableCollection<Title>(titlesQuery);
            }
            IsLoading = false;
        }

        private void LoadTitlesByType(string titleType)
        {
            IsLoading = true;

            switch (titleType)
            {
                case "movie":
                    ListTitle = "Movies";
                    break;
                case "tvSeries":
                    ListTitle = "TV Shows";
                    break;
                default:
                    ListTitle = "Titles";
                    break;
            }

            using (var context = new ImdbContext())
            {
                var titlesQuery = context.Titles
                    .Where(t => t.TitleType == titleType)
                    .Include(t => t.Rating)
                    .OrderByDescending(t => t.Rating.AverageRating)
                    .Take(50)
                    .ToList();

                Titles = new ObservableCollection<Title>(titlesQuery);
            }
            IsLoading = false;
        }

        private void PerformDynamicSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText) || SearchText.Length < 2)
            {
                // If search text is empty or too short, reload original data
                if (_parameter is int genreId)
                {
                    LoadGenreMovies(genreId);
                }
                else if (_parameter is string titleType)
                {
                    LoadTitlesByType(titleType);
                }
                return;
            }

            IsLoading = true;

            using (var context = new ImdbContext())
            {
                IQueryable<Title> query = context.Titles.Include(t => t.Rating);

                // If filtering by type
                if (_parameter is string titleType)
                {
                    query = query.Where(t => t.TitleType == titleType);
                }
                // If filtering by genre
                else if (_parameter is int genreId)
                {
                    query = query.Where(t => t.Genres.Any(g => g.GenreId == genreId));
                }

                // Add search condition
                query = query.Where(t => t.PrimaryTitle.Contains(SearchText));

                // Sort and get results
                var results = query.OrderByDescending(t => t.Rating.AverageRating)
                                   .Take(50)
                                   .ToList();

                Titles = new ObservableCollection<Title>(results);
            }

            IsLoading = false;
        }

        private void Search()
        {
            
            PerformDynamicSearch();
        }

        private void SelectTitle(Title title)
        {
            if (title == null) return;
            _navigationService.NavigateToWithViewModel<MovieDetailsView, MovieDetailsViewModel>(title.TitleId);
        }

        private void NavigateBack()
        {
            if (_parameter is int)
            {
                _navigationService.NavigateToWithViewModel<GenresView, GenresViewModel>();
            }
            else
            {
                _navigationService.NavigateToWithViewModel<HomeView, HomeViewModel>();
            }
        }
    }
}