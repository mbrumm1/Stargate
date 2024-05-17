import { Routes } from '@angular/router';
import { PeopleComponent } from './people/people.component';
import { AstronautsComponent } from './astronauts/astronauts.component';
import { RetiredComponent } from './retired/retired.component';

export const routes: Routes = [
  { path: "people", component: PeopleComponent, title: "People - Stargate" },
  { path: "astronauts", component: AstronautsComponent, title: "Astronauts - Stargate" },
  { path: "retired", component: RetiredComponent, title: "Retired - Stargate" }
];
