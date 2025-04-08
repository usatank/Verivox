/**import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-electricity-page',
  imports: [CommonModule, FormsModule],
  templateUrl: './electricity-page.component.html',
  styleUrl: './electricity-page.component.css'
})

export class ElectricityPageComponent {
  kWh: number = 0;
  results: any[] = [];
  filter: string = "all";
  sort: string = "price-asc";

  constructor(private http: HttpClient) { }

  getComparisons() {
    this.http.get<any[]>(`http://localhost:5050/api/calculate/?kwh=${this.kWh}`)
      .subscribe(data => {
        this.results = data;
      });
  }

  filteredResults() {
    return this.results.filter(result => 
      this.filter === "all" || 
      (this.filter === "basic" && result.type === 1) || 
      (this.filter === "package" && result.type === 2)
    ).sort((a, b) => {
      if (this.sort === "price-asc") return a.annualCost - b.annualCost;
      if (this.sort === "price-desc") return b.annualCost - a.annualCost;
      if (this.sort === "name-asc") return a.provider.localeCompare(b.provider);
      if (this.sort === "name-desc") return b.provider.localeCompare(a.provider);
      return 0;
    });
  }
}

-------
*/
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
      filtered = filtered.filter(t => t.type === this.selectedType);
    }

    return this.sortOrder === 'asc' ? filtered : filtered.reverse();
  }

  validateConsumption() {
    this.isValidConsumption = this.consumption > 0;
  }

  compareTariffs() {
    this.validateConsumption();
    if (this.isValidConsumption) {
      //console.log(`Comparing for ${this.consumption} kWh`);
      this.http.get<any[]>(`http://localhost:5050/api/calculate/?kwh=${this.consumption}`)
      .subscribe(data => {
        this.tariffs = data;
      });
    }
  }
}