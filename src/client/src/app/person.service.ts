import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddPersonResponse, EditPersonResponse, GetPeopleResponse, GetPersonResponse } from './models';

@Injectable({
  providedIn: 'root'
})
export class PersonService {
  // TODO: don't hard-code api url
  private url: string = "https://localhost:7204/Person";

  constructor(private http: HttpClient) { }

  getPeople(): Observable<GetPeopleResponse> {   
    return this.http.get<GetPeopleResponse>(this.url);
  }

  getPerson(name: string): Observable<GetPersonResponse> {
    const url = `${this.url}/${name}`;
    return this.http.get<GetPersonResponse>(url);
  }

  addPerson(name: string): Observable<AddPersonResponse> {
    return this.http.post<AddPersonResponse>(
      this.url, JSON.stringify(name), {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      });
  }

  editPerson(name: string, newName: string) {
    const url = `${this.url}/${name}`;
    return this.http.put<EditPersonResponse>(
      url, JSON.stringify(newName), {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      });
  }
}