# Build stage
FROM node:20-alpine AS build

WORKDIR /app

# Which .env file to use — override with --build-arg ENV_FILE=.env.docker for local
ARG ENV_FILE=.env.production

# Backend API URL — injected by CI via --build-arg. Vite reads VITE_* env vars at build time.
# Overrides whatever is in the .env file.
ARG VITE_SERVER_URL
ENV VITE_SERVER_URL=${VITE_SERVER_URL}

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm ci

# Copy source code
COPY . .

# Copy the chosen environment file as .env so Vite picks it up at build time
COPY ${ENV_FILE} .env

# Build the app
RUN npm run build

# Production stage
FROM nginx:alpine

# Copy built assets from build stage
COPY --from=build /app/build /usr/share/nginx/html

# Copy nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose port
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
