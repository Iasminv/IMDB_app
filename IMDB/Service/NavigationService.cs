using System;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace IMDB.Services
{
    public interface INavigationService
    {
        void NavigateTo(UserControl view);
        void NavigateToWithViewModel<TView, TViewModel>(object parameter = null)
            where TView : UserControl
            where TViewModel : class;
        event EventHandler<UserControl> CurrentViewChanged;
    }

    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public event EventHandler<UserControl> CurrentViewChanged;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo(UserControl view)
        {
            CurrentViewChanged?.Invoke(this, view);
        }

        public void NavigateToWithViewModel<TView, TViewModel>(object parameter = null)
            where TView : UserControl
            where TViewModel : class
        {
            var view = _serviceProvider.GetRequiredService<TView>();
            var viewModel = _serviceProvider.GetRequiredService<TViewModel>();

            
            if (viewModel is IParameterizedViewModel parameterizedViewModel && parameter != null)
            {
                parameterizedViewModel.Initialize(parameter);
            }

            view.DataContext = viewModel;
            CurrentViewChanged?.Invoke(this, view);
        }
    }

    public interface IParameterizedViewModel
    {
        void Initialize(object parameter);
    }
}