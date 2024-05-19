import { Component } from '@angular/core';
import { PersonService } from '../person.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'bam-edit-person',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './edit-person.component.html',
  styleUrl: './edit-person.component.css'
})
export class EditPersonComponent {
  name: string = "";
  newName: string = "";
  showError: boolean = false;
  errorMessage: string = "";

  constructor(
    private personService: PersonService,
    private router: Router,
    private route: ActivatedRoute) {}

  ngOnInit() {
    this.name = this.route.snapshot.paramMap.get("name") ?? "";
  }

  editPerson() {
    this.personService.editPerson(this.name, this.newName)
      .subscribe({
        next: () => this.router.navigate(['/person', this.newName]),
        error: (err) => {
          console.log(err);
          this.showError = true;
          this.errorMessage = err;
        }
      });
  }
} 
