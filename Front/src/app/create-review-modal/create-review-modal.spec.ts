import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateReviewModal } from './create-review-modal';

describe('CreateReviewModal', () => {
  let component: CreateReviewModal;
  let fixture: ComponentFixture<CreateReviewModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateReviewModal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateReviewModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
