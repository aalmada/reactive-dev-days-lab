using System.Reactive.Disposables;
using DevDaysSpeakers.ViewModel;
using ReactiveUI;
using Xamvvm;

namespace DevDaysSpeakers.View
{
	public partial class DetailsPage
		: IBasePageRxUI<DetailsViewModel>
	{
		public DetailsPage()
		{
			InitializeComponent();

			this.WhenActivated(disposables =>
			{
				this.OneWayBind(ViewModel, vm => vm.Avatar, page => page.Avatar.Source, x => x)
                    .DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.Name, page => page.Name.Text)
                    .DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.Description, page => page.Description.Text)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Speak, page => page.Speak.Command)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.VisitWebSite, page => page.VisitWebSite.Command)
                    .DisposeWith(disposables);
            });
		}
	}
}
