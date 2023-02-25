import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@app-environments/environment';
import { IEmail } from '@app/models/Email';
import { IResult, IUserQuery, IUserQueryEmail } from '@app/models/IUser';
import { IUserInfo } from '@app/models/IUserInfo';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  constructor(private http: HttpClient) {}
  API_URL = environment.apiUrl;

  getListByCategory(category: string, searchValue: string, searchController: string): Observable<IUserInfo[]> {
    let params = new HttpParams();
    params = params.append('category', category);
    params = params.append('searchValue', searchValue);
    return this.http.get<IUserInfo[]>(`${this.API_URL}${searchController}/category`, { params });
  }

  sendEmail(email: IEmail) {
    return this.http.post<IResult>(this.API_URL + 'aradhana/sendemail', email);
  }

  createLatestEvents(formData: any) {
    return this.http.post<IResult>(this.API_URL + 'aradhana/latest-update', formData);
  }

  uploadingPhoto(formData: any) {
    return this.http.post<IResult>(this.API_URL + 'contact/upload-photo', formData);
  }

  getUserClarification() {
    return this.http.get<IUserQuery[]>(this.API_URL + 'contact/clarification');
  }

  userClarfication(contact: IUserQueryEmail) {
    return this.http.post<IResult>(this.API_URL + 'contact/clarification', contact);
  }
}
