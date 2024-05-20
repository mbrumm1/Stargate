import { Component } from '@angular/core';
import { AstronautDuty, CreateAstronautDuty } from '../models';
import { AstronautService } from '../astronaut.service';
import { FormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'bam-add-duty',
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe],
  templateUrl: './add-duty.component.html',
  styleUrl: './add-duty.component.css'
})
export class AddDutyComponent {
  duty: CreateAstronautDuty;
  showError: boolean = false;
  errorMessage: string = "";

  constructor(
    private astronautService: AstronautService, 
    private route: ActivatedRoute,
    private router: Router) { 
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    this.duty = {
      name: "",
      rank: "",
      dutyTitle: "",
      dutyStartDate: today
    };
  }

  ngOnInit() {
    this.duty.name = this.route.snapshot.paramMap.get("name") ?? "";
  }

  addDuty() {
    if (confirm(`Are you sure you want to add this duty for ${this.duty.name}?`))
    this.astronautService.addDuty(this.duty)
      .subscribe({
        next: () => this.router.navigate(["/person", this.duty.name]),
        error: (err) => {
          console.log(err);
          this.showError = true;
          this.errorMessage = err.error.message;
        }
      });
  }
}


