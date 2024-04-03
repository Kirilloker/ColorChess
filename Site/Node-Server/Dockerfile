# Основывается на официальном образе Node.js
FROM node:20.12-alpine

# Устанавливает рабочую директорию в контейнере
WORKDIR /usr/src/app

# Копирует файлы `package.json` и `package-lock.json` (если есть)
COPY package*.json ./

# Устанавливает зависимости проекта
RUN npm install --network-timeout=1000000

# Копирует файлы проекта в рабочую директорию контейнера
COPY . .

# Открывает порт, на котором будет работать сервер
EXPOSE 20203

# Запускает сервер
CMD ["node", "server.js"]
