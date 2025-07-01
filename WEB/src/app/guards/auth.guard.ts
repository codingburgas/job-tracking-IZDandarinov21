import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService); 
  const router = inject(Router);         

  if (authService.isLoggedIn()) {
    return true; // Потребителят е логнат, разрешаваме достъп
  } else {
    // Потребителят не е логнат, пренасочваме го към страницата за вход
    router.navigate(['/login']);
    return false; // Отказваме достъп
  }
};
