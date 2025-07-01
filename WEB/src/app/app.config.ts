import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';


import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';


export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

import { routes } from './app.routes'; // Импортираме дефинираните маршрути от app.routes.ts

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes), // Предоставя маршрутизацията на приложението
    provideHttpClient(), // Предоставя HttpClient за HTTP заявки
    importProvidersFrom( // Зарежда ngx-translate модула
      TranslateModule.forRoot({
        defaultLanguage: 'bg', // Език по подразбиране
        loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient] // Зависимост за TranslateHttpLoader
        }
      })
    )
  ]
};
