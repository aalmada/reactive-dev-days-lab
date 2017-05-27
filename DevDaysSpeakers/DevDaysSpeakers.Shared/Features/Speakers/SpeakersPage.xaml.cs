using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Xamvvm;

namespace DevDaysSpeakers.Features.Speakers
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

                // show dialog on connection error
                ViewModel.ConnectionError.RegisterHandler(interaction =>
                    Observable.Start(async () => await this.DisplayAlert("Connection error", interaction.Input.Message, "OK"), RxApp.MainThreadScheduler)
                    .Do(_ => interaction.SetOutput(Unit.Default))
                )
                .DisposeWith(disposables);
            });
        }
    }
}
