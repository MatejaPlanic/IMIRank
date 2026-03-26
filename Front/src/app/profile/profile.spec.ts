import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { vi } from 'vitest';
import { ProfilePage } from './profile';
import { Api } from '../services/api';
import { AuthService } from '../services/auth.service';

describe('ProfilePage', () => {
  let component: ProfilePage;
  let fixture: ComponentFixture<ProfilePage>;
  let apiMock: any;
  let authServiceMock: any;
  let routerMock: any;
  let routeMock: any;

  beforeEach(async () => {
    const apiSpy = {
      getProfile: vi.fn(),
      getUserById: vi.fn()
    };
    const authSpy = {
      logout: vi.fn()
    };
    const routerSpy = {
      navigate: vi.fn()
    };
    const routeSpy = {
      snapshot: {
        paramMap: {
          get: vi.fn().mockReturnValue(null)
        }
      }
    };

    await TestBed.configureTestingModule({
      imports: [ProfilePage],
      providers: [
        { provide: Api, useValue: apiSpy },
        { provide: AuthService, useValue: authSpy },
        { provide: Router, useValue: routerSpy },
        { provide: ActivatedRoute, useValue: routeSpy }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfilePage);
    component = fixture.componentInstance;
    apiMock = TestBed.inject(Api);
    authServiceMock = TestBed.inject(AuthService);
    routerMock = TestBed.inject(Router);
    routeMock = TestBed.inject(ActivatedRoute);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load profile on init when no id', () => {
    const profileData = { userName: 'testuser' };
    apiMock.getProfile.mockReturnValue(of(profileData));

    component.ngOnInit();

    expect(component.publicView).toBeFalsy();
    expect(apiMock.getProfile).toHaveBeenCalled();
    expect(component.profile).toEqual(profileData);
    expect(component.loading).toBeFalsy();
  });

  it('should load public profile on init when id present', () => {
    const userId = '123';
    const userData = { id: userId, userName: 'publicuser', role: 'User', profilePictureUrl: null, totalReviews: 5 };
    routeMock.snapshot.paramMap.get.mockReturnValue(userId);
    apiMock.getUserById.mockReturnValue(of(userData));

    component.ngOnInit();

    expect(component.publicView).toBeTruthy();
    expect(apiMock.getUserById).toHaveBeenCalledWith(userId);
    expect(component.profile?.userName).toBe('publicuser');
    expect(component.loading).toBeFalsy();
  });

  it('should handle 401 error in loadProfile', () => {
    const error = { status: 401 };
    apiMock.getProfile.mockReturnValue(throwError(() => error));

    component.loadProfile();

    expect(authServiceMock.logout).toHaveBeenCalled();
    expect(routerMock.navigate).toHaveBeenCalledWith(['/login']);
  });
});
