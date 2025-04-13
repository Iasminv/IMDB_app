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
    public class ActorsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private string _actorName;
        private string _searchText;
        private int? _birthYear;
        private string _primaryProfession;
        private ObservableCollection<ActorFilmography> _filmography = new ObservableCollection<ActorFilmography>();
        private Name _selectedActor;

        public string ActorName
        {
            get => _actorName;
            set => SetProperty(ref _actorName, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public int? BirthYear
        {
            get => _birthYear;
            set => SetProperty(ref _birthYear, value);
        }

        public string PrimaryProfession
        {
            get => _primaryProfession;
            set => SetProperty(ref _primaryProfession, value);
        }

        public ObservableCollection<ActorFilmography> Filmography
        {
            get => _filmography;
            set => SetProperty(ref _filmography, value);
        }

        public ICommand BackToHomeCommand { get; }
        public ICommand SearchActorCommand { get; }

        public ActorsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            BackToHomeCommand = new RelayCommand(NavigateToHome);
            SearchActorCommand = new RelayCommand(SearchActor);

            // Load a default actor
            LoadRandomActor();
        }

        private void LoadRandomActor()
        {
            using (var context = new ImdbContext())
            {
                _selectedActor = context.Names
                    .Where(n => n.PrimaryProfession.Contains("actor") || n.PrimaryProfession.Contains("actress"))
                    .FirstOrDefault();

                if (_selectedActor != null)
                {
                    LoadActorDetails();
                }
            }
        }

        private void SearchActor()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return;

            using (var context = new ImdbContext())
            {
                _selectedActor = context.Names
                    .Where(n => n.PrimaryName.Contains(SearchText))
                    .FirstOrDefault();

                if (_selectedActor != null)
                {
                    LoadActorDetails();
                }
            }
        }

        private void LoadActorDetails()
        {
            ActorName = _selectedActor.PrimaryName;
            BirthYear = _selectedActor.BirthYear;
            PrimaryProfession = _selectedActor.PrimaryProfession;

            LoadFilmography();
        }

        private void LoadFilmography()
        {
            using (var context = new ImdbContext())
            {
                var principals = context.Principals
                    .Where(p => p.NameId == _selectedActor.NameId)
                    .Include(p => p.Name)
                    .Take(20)
                    .ToList();

                var filmographyItems = new List<ActorFilmography>();

                foreach (var principal in principals)
                {
                    var title = context.Titles
                        .Include(t => t.Rating)
                        .FirstOrDefault(t => t.TitleId == principal.TitleId);

                    if (title != null)
                    {
                        var item = new ActorFilmography
                        {
                            Year = title.StartYear,
                            Title = title.PrimaryTitle,
                            RolePosition = principal.Characters ?? principal.JobCategory ?? "Unknown",
                            Rating = title.Rating?.AverageRating ?? 0
                        };

                        filmographyItems.Add(item);
                    }
                }

                Filmography = new ObservableCollection<ActorFilmography>(
                    filmographyItems.OrderByDescending(f => f.Year));
            }
        }

        private void NavigateToHome()
        {
            _navigationService.NavigateToWithViewModel<HomeView, HomeViewModel>();
        }
    }

    public class ActorFilmography
    {
        public short? Year { get; set; }
        public string Title { get; set; }
        public string RolePosition { get; set; }
        public decimal Rating { get; set; }
    }
}