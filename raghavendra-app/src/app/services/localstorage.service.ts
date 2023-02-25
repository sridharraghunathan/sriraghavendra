import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageService {
  constructor() {}

  setLocalStorage(key: string, value: Object) {
    localStorage.setItem(key, JSON.stringify(value));
    return;
  }

  getLocalStorageByKey(key: string) {
    return JSON.parse(localStorage.getItem(key));
  }

  clearLocalStorage() {
    localStorage.clear();
  }

  clearLocalStorageByKey(key: string) {
    localStorage.removeItem(key);
  }
}
