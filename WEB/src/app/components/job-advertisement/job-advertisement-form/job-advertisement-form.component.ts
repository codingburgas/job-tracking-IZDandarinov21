import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { JobAdvertisementService } from '../../../services/job-advertisement.service';

@Component({
  selector: 'app-job-advertisement-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './job-advertisement-form.component.html',
  styleUrl: './job-advertisement-form.component.scss'
})
export class JobAdvertisementFormComponent implements OnInit {
  jobAdForm!: FormGroup;
  isEditMode: boolean = false;
  jobAdId: number | null = null;

  constructor(
    private jobAdService: JobAdvertisementService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.jobAdForm = new FormGroup({
      title: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]), 
      companyName: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]), 
      location: new FormControl('', [Validators.required, Validators.maxLength(100)]), 
      description: new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(1000)]), 
      requirements: new FormControl('', [Validators.maxLength(1000)]),
      responsibilities: new FormControl('', [Validators.maxLength(1000)]),
      benefits: new FormControl('', [Validators.maxLength(1000)])
    });

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.isEditMode = true;
        this.jobAdId = +id;
        this.loadJobAdvertisement(this.jobAdId);
      }
    });
  }

  loadJobAdvertisement(id: number): void {
    this.jobAdService.getJobAdvertisementById(id).subscribe({
      next: (jobAd) => {
        this.jobAdForm.patchValue(jobAd);
      },
      error: (error) => {
        console.error('Failed to load job advertisement for editing:', error);
        alert('Failed to load job advertisement for editing: ' + (error.error || error.message));
        this.router.navigate(['/job-advertisements']);
      }
    });
  }

  onSubmit(): void {
    this.jobAdForm.markAllAsTouched();

    if (this.jobAdForm.valid) {
      const jobAdData = this.jobAdForm.value;

      if (this.isEditMode && this.jobAdId) {
        this.jobAdService.updateJobAdvertisement(this.jobAdId, jobAdData).subscribe({
          next: () => {
            alert('Job advertisement updated successfully!');
            this.router.navigate(['/job-advertisements']);
          },
          error: (error) => {
            console.error('Failed to update job advertisement:', error);
            alert('Failed to update job advertisement: ' + (error.error?.errors ? JSON.stringify(error.error.errors) : error.error || error.message));
          }
        });
      } else {
        this.jobAdService.addJobAdvertisement(jobAdData).subscribe({
          next: () => {
            alert('Job advertisement added successfully!');
            this.router.navigate(['/job-advertisements']);
          },
          error: (error) => {
            console.error('Failed to add job advertisement:', error);
          
            let errorMessage = 'Failed to add job advertisement.';
            if (error.error && error.error.errors) {
              
              errorMessage += '\nValidation Errors:\n';
              for (const key in error.error.errors) {
                if (error.error.errors.hasOwnProperty(key)) {
                  errorMessage += `${key}: ${error.error.errors[key].join(', ')}\n`;
                }
              }
            } else if (error.error && typeof error.error === 'string') {
              errorMessage += `\nError: ${error.error}`;
            } else if (error.message) {
              errorMessage += `\nError: ${error.message}`;
            }
            alert(errorMessage);
          }
        });
      }
    } else {
      alert('Please fill in all required fields correctly.');
    }
  }

  goBack(): void {
    this.router.navigate(['/job-advertisements']);
  }
}
