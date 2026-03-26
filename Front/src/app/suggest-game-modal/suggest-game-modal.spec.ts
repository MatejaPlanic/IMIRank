import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuggestGameModal } from './suggest-game-modal';

describe('SuggestGameModal', () => {
  let component: SuggestGameModal;
  let fixture: ComponentFixture<SuggestGameModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SuggestGameModal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuggestGameModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
