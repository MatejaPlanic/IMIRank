import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  {
    path: 'review/:id',
    renderMode: RenderMode.Server
  },
  {
    path: 'profile/:id',
    renderMode: RenderMode.Server
  },
  {
    path: 'game/:id',
    renderMode: RenderMode.Server
  },
  {
    path: '**',
    renderMode: RenderMode.Prerender
  }
];
