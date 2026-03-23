import { Routes } from '@angular/router';
import { Register } from './register/register';
import { Login } from './login/login';
import { Home } from './home/home';
import { ReviewDetail } from './review-detail/review-detail';
import { ProfilePage } from './profile/profile';
import { GameReviews } from './game-reviews/game-reviews';
import { authGuard, notAuthGuard } from './guards/auth.guard';

export const routes: Routes = [
    {
        path: 'register',
        component: Register,
        canActivate: [notAuthGuard]
    },
    {
        path : 'login',
        component: Login,
        canActivate: [notAuthGuard]
    },
    {
        path:'home',
        component: Home,
        canActivate: [authGuard]
    },
    { 
        path: '', 
        redirectTo: 'home', 
        pathMatch: 'full' 
    },
    { 
        path: 'review/:id', 
        component: ReviewDetail,
        canActivate: [authGuard]
    },
    { 
        path: 'profile', 
        component: ProfilePage,
        canActivate: [authGuard]
    },
    { 
        path: 'profile/:id', 
        component: ProfilePage,
        canActivate: [authGuard]
    },
    {
        path: 'game/:id',
        component: GameReviews,
        canActivate: [authGuard]
    }
];
