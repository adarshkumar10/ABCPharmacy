import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';

@Component({
  selector: 'app-medicine-add',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './medicine-add.component.html'
})
export class MedicineAddComponent {
  @Output() added = new EventEmitter<void>();

  model = {
    fullName: '',
    brand: '',
    expiryDate: '',
    quantity: 0,
    price: 0.0,
    notes: ''
  };

  constructor(private api: ApiService) {}

  submit() {
    if (!this.model.fullName || !this.model.expiryDate) { alert('Fill required fields'); return; }
    this.api.addMedicine(this.model).subscribe({
      next: () => { alert('Medicine added'); this.model = { fullName: '', brand: '', expiryDate: '', quantity: 0, price: 0.0, notes: '' }; this.added.emit(); },
      error: e => alert(e?.error?.error || e?.message || 'Add error')
    });
  }
}
