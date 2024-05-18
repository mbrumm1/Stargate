import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PersonService } from '../person.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'bam-add-person',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-person.component.html',
  styleUrl: './add-person.component.css'
})
export class AddPersonComponent {
  name: string = "";
  errorOccurred: boolean = false;
  errorMessage: string = "";

  constructor(private personService: PersonService, private router: Router) {
  }

  addPerson() {
    this.errorOccurred = false;
    if (confirm(`Are you sure you want to add a person named "${this.name}"?`)) {
      this.personService.addPerson(this.name)
        .subscribe({
          // TODO: nagivate to person details page instead
          next: (response) => {
            if (!response.success) {
              this.errorOccurred = true;              
              return;
            }
            this.router.navigate(["/people"])
          },
          error: (response) => { 
            this.errorOccurred = true;
            this.errorMessage = response.error.message;
          }
        });
    }
  }
}
