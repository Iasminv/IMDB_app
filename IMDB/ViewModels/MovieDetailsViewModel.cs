using IMDB.Commands;
using IMDB.Services;
using IMDB.Views;
using IMDB_App.Data;
using IMDB_App.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace IMDB.ViewModels
{
    public class MovieDetailsViewModel : ViewModelBase, IParameterizedViewModel
    {
        private readonly INavigationService _navigationService;
        private Title _title;
        private string _titleName;
        private string _overview;
        private string _genreTags;
        private decimal _rating;
        private string _runtime;
        private string _releaseDate;
        private string _castAndCrew;
        private ObservableCollection<Title> _similarTitles = new ObservableCollection<Title>();

        public string TitleName
        {
            get => _titleName;
            set => SetProperty(ref _titleName, value);
        }

        public string Overview
        {
            get => _overview;
            set => SetProperty(ref _overview, value);
        }

        public string GenreTags
        {
            get => _genreTags;
            set => SetProperty(ref _genreTags, value);
        }

        public decimal Rating
        {
            get => _rating;
            set => SetProperty(ref _rating, value);
        }

        public string Runtime
        {
            get => _runtime;
            set => SetProperty(ref _runtime, value);
        }

        public string ReleaseDate
        {
            get => _releaseDate;
            set => SetProperty(ref _releaseDate, value);
        }

        public string CastAndCrew
        {
            get => _castAndCrew;
            set => SetProperty(ref _castAndCrew, value);
        }

        public ObservableCollection<Title> SimilarTitles
        {
            get => _similarTitles;
            set => SetProperty(ref _similarTitles, value);
        }

        public ICommand BackToHomeCommand { get; }

        public MovieDetailsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            BackToHomeCommand = new RelayCommand(NavigateToHome);
        }

        public void Initialize(object parameter)
        {
            // Check if parameter is a type or a specific titleId
            if (parameter is string identifier)
            {
                if (identifier == "movie" || identifier == "tvSeries")
                {
                    LoadRandomTitle(identifier);
                }
                else
                {
                    LoadTitleById(identifier);
                }
            }
        }

        private void LoadRandomTitle(string titleType)
        {
            using (var context = new ImdbContext())
            {
                _title = context.Titles
                    .Where(t => t.TitleType == titleType)
                    .Include(t => t.Rating)
                    .Include(t => t.Genres)
                    .OrderByDescending(t => t.Rating.AverageRating)
                    .FirstOrDefault();

                if (_title != null)
                {
                    LoadTitleDetails();
                }
            }
        }

        private void LoadTitleById(string titleId)
        {
            using (var context = new ImdbContext())
            {
                _title = context.Titles
                    .Where(t => t.TitleId == titleId)
                    .Include(t => t.Rating)
                    .Include(t => t.Genres)
                    .FirstOrDefault();

                if (_title != null)
                {
                    LoadTitleDetails();
                }
            }
        }

        private void LoadTitleDetails()
        {
            TitleName = _title.PrimaryTitle;

            // Load genre tags
            var genres = _title.Genres.Select(g => g.Name).ToList();
            GenreTags = string.Join(", ", genres);

            // Load rating
            Rating = _title.Rating?.AverageRating ?? 0;

            // Generate overview
            Overview = $"This is a {_title.TitleType} titled {_title.PrimaryTitle}.";
            if (_title.OriginalTitle != _title.PrimaryTitle)
            {
                Overview += $" Original title: {_title.OriginalTitle}.";
            }

            // Set runtime
            Runtime = _title.RuntimeMinutes.HasValue ? $"{_title.RuntimeMinutes} minutes" : "Unknown";

            // Set release date
            ReleaseDate = _title.StartYear.HasValue ? $"{_title.StartYear}" : "Unknown";
            if (_title.EndYear.HasValue)
            {
                ReleaseDate += $" - {_title.EndYear}";
            }

            // Load similar titles
            LoadSimilarTitles();

            // Load cast and crew
            LoadCastAndCrew();
        }

        private void LoadSimilarTitles()
        {
            using (var context = new ImdbContext())
            {
                // Find similar titles based on same type and similar rating
                var similarTitlesList = context.Titles
                    .Where(t => t.TitleType == _title.TitleType && t.TitleId != _title.TitleId)
                    .Include(t => t.Rating)
                    .OrderByDescending(t => t.Rating.AverageRating)
                    .Take(5)
                    .ToList();

                SimilarTitles = new ObservableCollection<Title>(similarTitlesList);
            }
        }

        private void LoadCastAndCrew()
        {
            using (var context = new ImdbContext())
            {
                var principals = context.Principals
                    .Where(p => p.TitleId == _title.TitleId)
                    .Include(p => p.Name)
                    .OrderBy(p => p.Ordering)
                    .Take(10)
                    .ToList();

                var castAndCrewList = new List<string>();
                foreach (var person in principals)
                {
                    castAndCrewList.Add($"{person.Name?.PrimaryName} ({person.JobCategory})");
                }

                CastAndCrew = string.Join("\n", castAndCrewList);
            }
        }

        private void NavigateToHome()
        {
            _navigationService.NavigateToWithViewModel<HomeView, HomeViewModel>();
        }
    }
}