import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { MedicineListComponent } from './medicine-list.component';
import { ApiService } from '../../core/services/api.service';
import { of } from 'rxjs';
import { CommonModule } from '@angular/common';

describe('MedicineListComponent', () => {
  let component: MedicineListComponent;
  let fixture: ComponentFixture<MedicineListComponent>;
  let apiSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getMedicines', 'recordSale']);

    await TestBed.configureTestingModule({
      imports: [CommonModule, FormsModule],
      declarations: [MedicineListComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(MedicineListComponent);
    component = fixture.componentInstance;
  });

  it('loads medicines and displays them', () => {
    const meds = [
      { id: '1', fullName: 'A', expiryDate: new Date(Date.now() + 1000 * 60 * 60 * 24 * 40).toISOString(), quantity: 20, price: 1.0 },
      { id: '2', fullName: 'B', expiryDate: new Date(Date.now() + 1000 * 60 * 60 * 24 * 10).toISOString(), quantity: 5, price: 2.0 }
    ];
    apiSpy.getMedicines.and.returnValue(of(meds));

    fixture.detectChanges(); // triggers ngOnInit via TestBed create
    component.load();

    expect(apiSpy.getMedicines).toHaveBeenCalled();
    // medicines are stored in the signal; access via medicines()
    expect(component.medicines()).toBeTruthy();
    expect(component.medicines().length).toBe(2);
    // check expiry less than 30 on second item
    expect(component.isExpiryLessThan30(component.medicines()[1])).toBeTrue();
  });
});
