import { Routes } from '@angular/router';
import { Register } from './register/register';
import { Login } from './login/login';
import { Home } from './home/home';
import { ReviewDetail } from './review-detail/review-detail';

export const routes: Routes = [
    {
        path: 'register',
        component: Register
    },
    {
        path : 'login',
        component: Login
    },
    {
        path:'home',
        component: Home
    },
    { 
        path: '', 
        redirectTo: 'home', 
        pathMatch: 'full' 
    },
    { 
        path: 'review/:id', 
        component: ReviewDetail 
    },
];
