using Core.Interfaces;
using Core.Streamlabs;
using Core.Streamlabs.Events;

namespace Infrastructure.Streamlabs;

public class StreamlabsEventClient : IStreamlabsEventClient {
  public IObservable<IDonation> DonationReceived() {
    throw new NotImplementedException();
  }

  public IObservable<IStreamlabsDonation> StreamlabsDonationReceived() {
    throw new NotImplementedException();
  }

  public IObservable<ITwitchSubscription> TwitchSubscriptionReceived() {
    throw new NotImplementedException();
  }

  public IObservable<ITwitchBitsCheer> TwitchBitsCheerReceived() {
    throw new NotImplementedException();
  }
}