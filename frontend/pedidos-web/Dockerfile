# ---------- Build ----------
FROM node:22-alpine AS build
WORKDIR /app

# Instala dependências a partir do lockfile (build reprodutível).
COPY package.json package-lock.json ./
RUN npm ci

# Copia o restante do código e gera o build de produção.
COPY . .
RUN npm run build

# ---------- Runtime (servidor estático) ----------
FROM nginx:1.27-alpine AS final

# Configuração do Nginx: serve o SPA e faz proxy de /api para o serviço da API.
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Artefatos estáticos gerados pelo Angular (@angular/build:application).
COPY --from=build /app/dist/pedidos-web/browser /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
