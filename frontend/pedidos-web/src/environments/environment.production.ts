// Build de produção (Docker): o Nginx serve o SPA e faz proxy de /api para a API,
// então usamos uma URL relativa de mesma origem (sem CORS no navegador).
export const environment = {
  apiBaseUrl: '/api',
};
