using System.Reactive.Disposables;
using DevDaysSpeakers.ViewModel;
using ReactiveUI;
using Xamvvm;

namespace DevDaysSpeakers.View
{
	public partial class SpeakersPage 
        : IBasePageRxUI<SpeakersViewModel>
    {
        public SpeakersPage()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
				this.OneWayBind(ViewModel, vm => vm.Speakers, page => page.ListViewSpeakers.ItemsSource)
                    .DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.GetSpeakers, page => page.SyncSpeakers.Command)
                    .DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.IsBusy, page => page.IsBusy.IsVisible)
                    .DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.IsBusy, page => page.IsBusy.IsEnabled)
                    .DisposeWith(disposables);
				this.Bind(ViewModel, vm => vm.Speaker, page => page.ListViewSpeakers.SelectedItem)
                    .DisposeWith(disposables);
            });
        }
    }
}
