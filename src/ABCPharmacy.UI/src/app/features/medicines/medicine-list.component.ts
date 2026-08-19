import { Component, OnInit, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Medicine } from '../../core/models/medicine.model';

@Component({
  selector: 'app-medicine-list',
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe],
  templateUrl: './medicine-list.component.html',
  styles: [`
    .expired-soon { background-color: #ffdddd; }
    .low-stock { background-color: #fff4cc; }
    table { width:100%; border-collapse: collapse; }
    th, td { padding:8px; border:1px solid #ddd; text-align:left; }
  `]
})
export class MedicineListComponent implements OnInit {
  medicines = signal<Medicine[]>([]);
  search = signal('');
  quantityToSell: Record<string, number> = {};

  constructor(private api: ApiService) {}

  ngOnInit(): void { this.load(); }

  load() {
    this.api.getMedicines(this.search()).subscribe(x => this.medicines.set(x));
  }

  isExpiryLessThan30(m: Medicine) {
    const d = new Date(m.expiryDate);
    const now = new Date();
    const diff = Math.ceil((d.getTime() - now.getTime()) / (1000*60*60*24));
    return diff >= 0 && diff <= 30;
  }

  sell(m: Medicine) {
    const qty = Number(this.quantityToSell[m.id] || 0);
    if (qty <= 0) { alert('Enter quantity > 0'); return; }
    if (qty > m.quantity) { alert('Not enough stock'); return; }
    this.api.recordSale({ medicineId: m.id, quantity: qty }).subscribe({
      next: () => { alert('Sale recorded'); this.quantityToSell[m.id] = 0; this.load(); },
      error: e => alert(e?.error?.error || e?.message || 'Sale error')
    });
  }
}
