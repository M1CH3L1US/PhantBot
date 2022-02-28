import { Inject, Optional } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { BehaviorSubject, Observable, Subscriber } from 'rxjs';

import { SIGNALR_HUB_URL } from './signalr-hub-url.token';

export class SignalRClient {
  private _connection: HubConnection;
  private _handlers = new Map<string, BehaviorSubject<any | null>>();

  constructor(
    @Optional() @Inject('BASE_URL') baseUrl: string,
    @Inject(SIGNALR_HUB_URL) hubUrlOrPath: string
  ) {
    let hubUrl = hubUrlOrPath;

    if (this.isRelativeUrl(hubUrlOrPath)) {
      hubUrl = `${baseUrl}${hubUrlOrPath}`;
    }

    console.log(hubUrl);

    this._connection = new HubConnectionBuilder()
      .withAutomaticReconnect()
      .withUrl(hubUrl)
      .build();
  }

  public on<T>(method: string): Observable<T | null> {
    const handler = this._handlers.get(method);

    if (handler) {
      return <BehaviorSubject<T>>handler.asObservable();
    }

    const subject = new BehaviorSubject<T | null>(null);
    this._connection.on(method, (data: T) => subject.next(data));
    this._handlers.set(method, subject);

    const registerHandler = (s: Subscriber<unknown>) => {
      this._connection.on(method, (data: T) => subject.next(data));
      subject.subscribe(s);
    };

    return new Observable<T>((subscriber) => {
      this.invokeCallbackAfterConnection(() => registerHandler(subscriber));
      return () => this._connection.off(method);
    });
  }

  public async invoke<T>(method: string, data?: T): Promise<void> {
    return this.invokeCallbackAfterConnection(() =>
      this._connection.invoke(method, data)
    );
  }

  private async invokeCallbackAfterConnection(cb: () => void): Promise<void> {
    if (this._connection.state === HubConnectionState.Connected) {
      return cb();
    }

    if (this._connection.state === HubConnectionState.Disconnected) {
      return this._connection.start().then(() => cb());
    }

    setTimeout(() => this.invokeCallbackAfterConnection(cb), 100);
  }

  private isRelativeUrl(url: string): boolean {
    try {
      const uri = new URL(url);
      return uri.hostname !== '';
    } catch {
      return true;
    }
  }
}
