FROM node:lts
ENV PORT 80
EXPOSE 80

WORKDIR /app
COPY package.json .
RUN npm install
COPY . .
RUN npm install -g @angular/cli

CMD ["npm", "start"]
