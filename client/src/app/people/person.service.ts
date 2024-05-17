import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PersonAstronaut } from './person.model';
import { ApiResponse } from '../../api-response';

@Injectable({
  providedIn: 'root'
})
export class PersonService {

  constructor(private http: HttpClient) { }

  getPeople(): Observable<ApiResponse<PersonAstronaut[]>> {
    // TODO: don't hard-code api url
    return this.http.get<ApiResponse<PersonAstronaut[]>>("https://localhost:7204/Person");
  }
}
