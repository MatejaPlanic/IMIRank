import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';

import { ReviewDetail } from './review-detail';

describe('ReviewDetail', () => {
  let component: ReviewDetail;
  let fixture: ComponentFixture<ReviewDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReviewDetail],
      providers: [
        { provide: ActivatedRoute, useValue: { snapshot: { paramMap: { get: () => null } } } }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReviewDetail);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
