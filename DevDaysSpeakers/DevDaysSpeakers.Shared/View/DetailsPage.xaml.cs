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
				this.OneWayBind(ViewModel, x => x.Avatar, x => x.Avatar.Source, x => x).DisposeWith(disposables);
				this.OneWayBind(ViewModel, x => x.Name, x => x.Name.Text).DisposeWith(disposables);
				this.OneWayBind(ViewModel, x => x.Description, x => x.Description.Text).DisposeWith(disposables);
			});
		}
	}
}
