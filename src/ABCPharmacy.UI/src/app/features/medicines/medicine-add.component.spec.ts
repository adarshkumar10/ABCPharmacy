import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { MedicineAddComponent } from './medicine-add.component';
import { ApiService } from '../../core/services/api.service';
import { of, throwError } from 'rxjs';

describe('MedicineAddComponent', () => {
  let component: MedicineAddComponent;
  let fixture: ComponentFixture<MedicineAddComponent>;
  let apiSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['addMedicine']);

    await TestBed.configureTestingModule({
      imports: [FormsModule],
      declarations: [MedicineAddComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(MedicineAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('submit() calls ApiService.addMedicine and resets model on success', (done) => {
    apiSpy.addMedicine.and.returnValue(of({}));

    component.model.fullName = 'Test';
    component.model.expiryDate = new Date().toISOString().slice(0, 10);
    component.submit();

    setTimeout(() => {
      expect(apiSpy.addMedicine).toHaveBeenCalled();
      expect(component.model.fullName).toBe('');
      done();
    }, 0);
  });

  it('submit() shows error when API fails', (done) => {
    apiSpy.addMedicine.and.returnValue(throwError(() => ({ error: 'fail' })));
    spyOn(window, 'alert');

    component.model.fullName = 'Test';
    component.model.expiryDate = new Date().toISOString().slice(0, 10);
    component.submit();

    setTimeout(() => {
      expect(apiSpy.addMedicine).toHaveBeenCalled();
      expect(window.alert).toHaveBeenCalled();
      done();
    }, 0);
  });
});
