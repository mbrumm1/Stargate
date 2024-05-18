import { Routes } from '@angular/router';
import { PeopleComponent } from './people/people.component';
import { AstronautsComponent } from './astronauts/astronauts.component';
import { RetiredComponent } from './retired/retired.component';
import { AddPersonComponent } from './add-person/add-person.component';
import { PersonComponent } from './person/person.component';
import { AddDutyComponent } from './add-duty/add-duty.component';

export const routes: Routes = [
  { path: "people", component: PeopleComponent, title: "People - Stargate" },
  { path: "astronauts", component: AstronautsComponent, title: "Astronauts - Stargate" },
  { path: "retired", component: RetiredComponent, title: "Retired - Stargate" },
  { path: "add-person", component: AddPersonComponent, title: "Add Person - Stargate" },
  { path: "person/:name", component: PersonComponent, title: "Person - Stargate" },
  { path: "add-duty/:name", component: AddDutyComponent, title: "Add Duty - Stargate" },
];
