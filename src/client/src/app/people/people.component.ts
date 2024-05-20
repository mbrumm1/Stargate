import { Component } from '@angular/core';
import { PersonService } from '../person.service';
import { PersonAstronaut } from '../models';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'bam-people',
  standalone: true,
  imports: [DatePipe, RouterLink, FormsModule],
  templateUrl: './people.component.html',
  styleUrl: './people.component.css'
})
export class PeopleComponent {
  people: PersonAstronaut[] = [];

  constructor(private personService: PersonService) {
  }

  ngOnInit() {
    this.personService.getPeople()
      .subscribe(data => {        
        this.people = data.people
      });
  }
}