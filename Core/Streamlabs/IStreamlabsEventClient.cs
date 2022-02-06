using System.Reactive.Subjects;
using Core.Interfaces;
using Core.Streamlabs.Events;

namespace Core.Streamlabs;

public interface IStreamlabsEventClient {
  public ISubject<object> EventReceived { get; }
  public IObservable<IDonation> DonationReceived();
  public IObservable<IStreamlabsDonation> StreamlabsDonationReceived();
  public IObservable<ITwitchSubscription> TwitchSubscriptionReceived();
  public IObservable<ITwitchBitsCheer> TwitchBitsCheerReceived();
}