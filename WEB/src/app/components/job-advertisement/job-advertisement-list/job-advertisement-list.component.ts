import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router'; // За routerLink
import { JobAdvertisementService } from '../../../services/job-advertisement.service';

@Component({
  selector: 'app-job-advertisement-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './job-advertisement-list.component.html',
  styleUrl: './job-advertisement-list.component.scss'
})
export class JobAdvertisementListComponent implements OnInit {
  jobAdvertisements: any[] = []; // Масив за съхранение на обявите

  constructor(private jobAdService: JobAdvertisementService) { }

  ngOnInit(): void {
    this.loadJobAdvertisements(); // Зарежда обявите при инициализация на компонента
  }

  loadJobAdvertisements(): void {
    this.jobAdService.getJobAdvertisements().subscribe({
      next: (data) => {
        this.jobAdvertisements = data;
        console.log('Job Advertisements loaded:', data);
      },
      error: (error) => {
        console.error('Failed to load job advertisements:', error);
        alert('Failed to load job advertisements. Please ensure you are logged in and the backend is running.');
      }
    });
  }

  deleteJobAdvertisement(id: number): void {
    if (confirm('Are you sure you want to delete this job advertisement?')) {
      this.jobAdService.deleteJobAdvertisement(id).subscribe({
        next: () => {
          console.log('Job advertisement deleted successfully.');
          this.loadJobAdvertisements(); // Презарежда списъка след изтриване
        },
        error: (error) => {
          console.error('Failed to delete job advertisement:', error);
          alert('Failed to delete job advertisement: ' + (error.error || error.message));
        }
      });
    }
  }
}
