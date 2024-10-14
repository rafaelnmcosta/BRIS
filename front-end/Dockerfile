# Estágio 1: Build do React
FROM node:18.13.0 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . ./
RUN npm run build

# Estágio 2: Servindo com Nginx (Produção)
FROM nginx:alpine AS production
COPY --from=build /app/build /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

# Estágio 3: Desenvolvimento
FROM node:18.13.0 AS development
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . ./
EXPOSE 3000
CMD ["npm", "start"]
