import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface IFontPreference {
  font: string;
  style: string;
  sizeInPx: number;
}

const DEFAULT_FONT_PREFERENCE: IFontPreference = {
  font: 'Inter',
  style: 'Regular',
  sizeInPx: 16,
};

const LOCAL_STORAGE_FONT_KEY = 'typography.font-preference';

@Injectable({
  providedIn: 'root',
})
export class FontPreferenceService {
  public get fontPreferences$() {
    return this.fontPreferences.asObservable();
  }
  private fontPreferences: BehaviorSubject<IFontPreference>;

  constructor() {
    const preferences = this.getFontPreference();
    this.fontPreferences = new BehaviorSubject(preferences);
  }

  public getFontPreference(): IFontPreference {
    const preference = localStorage.getItem(LOCAL_STORAGE_FONT_KEY);

    if (preference) {
      return <IFontPreference>JSON.parse(preference);
    }

    return DEFAULT_FONT_PREFERENCE;
  }

  public setFontPreference(fontPreference: IFontPreference) {
    const data = JSON.stringify(fontPreference);
    localStorage.setItem(LOCAL_STORAGE_FONT_KEY, data);
    this.fontPreferences.next(fontPreference);
  }
}
