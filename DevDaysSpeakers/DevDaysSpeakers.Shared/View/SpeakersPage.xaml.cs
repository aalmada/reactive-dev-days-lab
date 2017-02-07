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
				this.OneWayBind(ViewModel, x => x.Speakers, x => x.ListViewSpeakers.ItemsSource).DisposeWith(disposables);
				this.OneWayBind(ViewModel, x => x.GetSpeakers, x => x.SyncSpeakers.Command).DisposeWith(disposables);
				this.OneWayBind(ViewModel, x => x.IsBusy, x => x.IsBusy.IsVisible).DisposeWith(disposables);
				this.OneWayBind(ViewModel, x => x.IsBusy, x => x.IsBusy.IsEnabled).DisposeWith(disposables);
				this.Bind(ViewModel, x => x.Speaker, x => x.ListViewSpeakers.SelectedItem).DisposeWith(disposables);
            });
        }
    }
}
