import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'SonEcommerce',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44326/',
    redirectUri: baseUrl,
    clientId: 'SonEcommerce_App',
    responseType: 'code',
    scope: 'offline_access SonEcommerce',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44378',
      rootNamespace: 'SonEcommerce',
    },
  },
} as Environment;
