using IMDB.Services;
using System;
using System.Windows.Controls;

namespace IMDB_Tests
{
    public class TestNavigationService : INavigationService
    {
        private EventHandler<UserControl> _currentViewChanged;
        public event EventHandler<UserControl> CurrentViewChanged
        {
            add { _currentViewChanged += value; }
            remove { _currentViewChanged -= value; }
        }

        public string LastNavigatedView { get; set; }  // Made setter public for testing
        public object LastNavigatedViewModel { get; private set; }
        public object LastParameter { get; private set; }

        public void NavigateTo(UserControl view)
        {
            _currentViewChanged?.Invoke(this, view);
        }

        public void NavigateToWithViewModel<TView, TViewModel>(object parameter = null)
            where TView : UserControl
            where TViewModel : class
        {
            LastNavigatedView = typeof(TView).Name;
            LastNavigatedViewModel = typeof(TViewModel).Name;
            LastParameter = parameter;

            _currentViewChanged?.Invoke(this, null);
        }

        public void RaiseViewChanged(UserControl view)
        {
            _currentViewChanged?.Invoke(this, view);
        }
    }
}