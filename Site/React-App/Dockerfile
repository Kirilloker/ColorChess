# Этап сборки
FROM node:20.12-alpine as build-stage
WORKDIR /app
COPY package*.json ./
RUN npm install --network-timeout=1000000
COPY . .
RUN npm run build

# Этап запуска
FROM nginx:stable-alpine as production-stage
COPY --from=build-stage /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
