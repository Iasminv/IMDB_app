using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IMDB.Services;
using IMDB.ViewModels;
using IMDB.Views;
using IMDB_App.Data;
using IMDB_App.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace IMDB
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Load data
            LoadData();

            // Configure navigation
            var mainViewModel = ServiceProvider.GetRequiredService<MainViewModel>();
            var navigationService = ServiceProvider.GetRequiredService<INavigationService>();

            // Show main window
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = mainViewModel;
            navigationService.NavigateTo(ServiceProvider.GetRequiredService<HomeView>());
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Register database context
            services.AddDbContext<ImdbContext>(options =>
                options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=IMDB;Integrated Security=True;Trust Server Certificate=False;"));

            // Register services
            services.AddSingleton<INavigationService, NavigationService>();

            // Register views
            services.AddSingleton<MainWindow>();
            services.AddSingleton<HomeView>();
            services.AddSingleton<MovieDetailsView>();
            services.AddSingleton<ActorsView>();
            services.AddSingleton<GenresView>();
            services.AddSingleton<MovieListView>();

            // Register view models
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<MovieDetailsViewModel>();
            services.AddSingleton<ActorsViewModel>();
            services.AddSingleton<GenresViewModel>();
            services.AddSingleton<MovieListViewModel>();
        }

        private void LoadData()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ImdbContext>();
                var homeViewModel = ServiceProvider.GetRequiredService<HomeViewModel>();
                var genresViewModel = ServiceProvider.GetRequiredService<GenresViewModel>();

                // Load top movies data
                var topMovies = dbContext.Titles
                    .Where(t => t.TitleType == "movie")
                    .Include(t => t.Rating)
                    .OrderByDescending(t => t.Rating.AverageRating)
                    .Take(20)
                    .ToList();
                homeViewModel.SearchResults = new ObservableCollection<Title>(topMovies);

                // Load genres data
                var genres = dbContext.Genres.ToList();
                genresViewModel.Genres = new ObservableCollection<Genre>(genres);
            }
        }
    }
}