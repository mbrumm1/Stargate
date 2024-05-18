import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateAstronautDuty, GetAstronautDutiesResponse } from './models';

@Injectable({
  providedIn: 'root'
})
export class AstronautService {
  private url: string = "https://localhost:7204/AstronautDuty";

  constructor(private http: HttpClient) { }

  getAstronautDuties(name: string): Observable<GetAstronautDutiesResponse> {
    const url = `${this.url}/${name}`;
    return this.http.get<GetAstronautDutiesResponse>(url);
  }

  addDuty(duty: CreateAstronautDuty) {
    return this.http.post(this.url, duty);
  }
}
