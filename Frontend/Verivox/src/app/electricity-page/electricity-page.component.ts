import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-electricity-page',
  imports: [CommonModule, FormsModule],
  templateUrl: './electricity-page.component.html',
  styleUrls: ['./electricity-page.component.css']
})
export class ElectricityPageComponent {
  consumption: number = 0;
  selectedType: string = 'All';
  sortOrder: string = 'asc';
  isValidConsumption: boolean = true;
  tariffs: any[] = [];
  errorMessage: string = "";

  constructor(private http: HttpClient) { }

  get filteredTariffs() {
    let filtered = this.tariffs.sort((a,b) => a.annualCost - b.annualCost);
    
    if (this.selectedType !== 'All') {
      var type = this.selectedType === "Basic" ? 1 : 2;
      filtered = filtered.filter(t => t.type === type);
    }

    return this.sortOrder === 'asc' ? filtered : filtered.reverse();
  }

  validateConsumption() {
    this.isValidConsumption = this.consumption > 0;
  }
 
    compareTariffs() {
      this.validateConsumption();
      if (this.isValidConsumption) {
        this.http.get<any[]>(`http://localhost:5050/api/calculate/?kwh=${this.consumption}`)
          .pipe(
            catchError(error => {
              console.error('Error fetching tariffs:', error);
              this.tariffs = [];
              this.errorMessage = 'Service unavailable. Please try again later.';
              return of(null); // Return 'null' to indicate an error occurred
            })
          )
          .subscribe(data => {
            if (data === null) {
              // Do nothing if an error already occurred (message is already set)
              return;
            }
            if (!data || data.length === 0) {
              this.tariffs = [];
              this.errorMessage = 'No tariffs found.';
            } else {
              this.tariffs = data;
              this.errorMessage = ''; // Clear error message if data is available
            }
          });
      }
    }
}