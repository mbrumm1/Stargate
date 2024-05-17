import { Component } from '@angular/core';
import { PersonService } from './person.service';
import { PersonAstronaut } from './person.model';

@Component({
  selector: 'bam-people',
  standalone: true,
  imports: [],
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
        console.log(data);
        console.log("value: ", data.value);
        this.people = data.value
      });
  }
}