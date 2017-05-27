using DevDaysSpeakers.Model;
using Plugin.TextToSpeech;
using ReactiveUI;
using Splat;
using System;
using System.Reactive.Linq;
using Xamarin.Forms;
using Xamvvm;

namespace DevDaysSpeakers.Features.Details
{
    public class DetailsViewModel
        : BasePageModelRxUI
    {
        Speaker speaker;
        public Speaker Speaker
        {
            get { return speaker; }
            set { this.RaiseAndSetIfChanged(ref speaker, value); }
        }

        public ReactiveCommand Speak { get; }

        public ReactiveCommand VisitWebSite { get; }

        public DetailsViewModel()
        {
            var canSpeak = this.WhenAnyValue(vm => vm.Speaker)
                .DistinctUntilChanged()
                .Select(speaker => speaker != null && !String.IsNullOrEmpty(speaker.Description));

            Speak = ReactiveCommand.CreateFromObservable(() => 
                Observable.Start(() => CrossTextToSpeech.Current.Speak(Speaker.Description), RxApp.MainThreadScheduler),
                canSpeak);

            var canVisitWebSite = this.WhenAnyValue(vm => vm.Speaker)
                .DistinctUntilChanged()
                .Select(speaker => speaker != null && !String.IsNullOrEmpty(speaker.Website) && speaker.Website.StartsWith("http"));

            VisitWebSite = ReactiveCommand.CreateFromObservable(() => 
                Observable.Start(() => Device.OpenUri(new Uri(Speaker.Website))),
                canVisitWebSite);
        }
    }
}