using DevDaysSpeakers.Services;
using DevDaysSpeakers.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Disposables;

[assembly: Dependency(typeof(AzureService))]
namespace DevDaysSpeakers.Services
{
    public class AzureService
    {
        const string appUrl = "https://montemagnospeakers.azurewebsites.net";

        MobileServiceClient Client { get; set; } = null;
        IMobileServiceSyncTable<Speaker> table;

        IObservable<Unit> Initialize()
        {
            return Observable.Return(Unit.Default)
                .Select(_ =>
                {
                    if (Client?.SyncContext?.IsInitialized ?? false)
                        return Observable.Return(Unit.Default);

                    //Create our client
                    Client = new MobileServiceClient(appUrl);

                    //InitialzeDatabase for path
                    var path = Path.Combine(MobileServiceClient.DefaultDatabasePath, "syncstore.db");

                    //setup our local sqlite store and intialize our table
                    var store = new MobileServiceSQLiteStore(path);

                    //Define table
                    store.DefineTable<Speaker>();

                    return Observable.Start(() => Client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler()))
                        .SelectMany(__ => Observable.Start(() =>
                        {
                            //Get our sync table that will call out to azure
                            table = Client.GetSyncTable<Speaker>();
                        }));
                })
                .Switch();
        }

        public IObservable<IEnumerable<Speaker>> GetSpeakers()
        {
            return Initialize()
                .SelectMany(_ => SyncSpeakers(Client, table))
                .SelectMany(_ => Observable.StartAsync(() => table.OrderBy(s => s.Name).ToEnumerableAsync()));
        }

        public static IObservable<Unit> SyncSpeakers(MobileServiceClient client, IMobileServiceSyncTable<Speaker> table)
        {
            return Observable.StartAsync(() => client.SyncContext.PushAsync())
                .SelectMany(_ => Observable.StartAsync(() => client.SyncContext.PushAsync()))
                .SelectMany(_ => Observable.StartAsync(() => table.PullAsync("allSpeakers", table.CreateQuery())))
                .Catch<Unit, Exception>(ex =>
                {
                    Debug.WriteLine("Unable to sync speakers, that is alright as we have offline capabilities: " + ex);
                    return Observable.Return(Unit.Default);
                });
        }
    }
}