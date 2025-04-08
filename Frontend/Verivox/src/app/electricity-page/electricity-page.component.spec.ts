import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ElectricityPageComponent } from './electricity-page.component';

describe('ElectricityPageComponent', () => {
  let component: ElectricityPageComponent;
  let fixture: ComponentFixture<ElectricityPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ElectricityPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ElectricityPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
