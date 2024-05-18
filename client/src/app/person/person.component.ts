import { Component } from '@angular/core';
import { AstronautDuty, PersonAstronaut } from '../models';
import { PersonService } from '../person.service';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AstronautService } from '../astronaut.service';

@Component({
  selector: 'bam-person',
  standalone: true,
  imports: [DatePipe, RouterLink],
  templateUrl: './person.component.html',
  styleUrl: './person.component.css'
})
export class PersonComponent {
  person: PersonAstronaut = {
    personId: 0,
    name: "",
    currentRank: "",
    currentDutyTitle: "",
    careerStartDate: new Date(),
    careerEndDate: new Date()
  };
  duties: AstronautDuty[] = [];

  constructor(
    private personService: PersonService,
    private astronautService: AstronautService,
    private route: ActivatedRoute) {
  }

  ngOnInit() {
    const name = this.route.snapshot.paramMap.get("name");
    if (name) {
      this.personService.getPerson(name)
        .subscribe(response => {
          this.person = response.person;
        });

      this.astronautService.getAstronautDuties(name)
        .subscribe(response => {
          this.duties = response.astronautDuties;
        });
    }
  }
}
