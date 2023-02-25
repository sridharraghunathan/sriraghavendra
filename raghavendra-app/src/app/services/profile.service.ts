import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@app-environments/environment';
import { IResult } from '@app/models/IUser';
import { IUserInfo } from '@app/models/IUserInfo';
 

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  API_URL = environment.apiUrl;
  constructor(private http: HttpClient) {}
  // Common Service used for the shared components like ProfileEdit 
  createContact(userInfo: IUserInfo, componentType: string) {
    return this.http.post<IResult>(this.API_URL + componentType, userInfo);
  }

  updateContact(id: string, userInfo: IUserInfo) {
    return this.http.put<IResult>(this.API_URL + 'contact/' + id, userInfo);
  }
}
