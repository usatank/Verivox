import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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
      .subscribe(data => {
        this.tariffs = data;
      });
    }
  }
}