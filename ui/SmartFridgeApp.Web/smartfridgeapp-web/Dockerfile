# Build stage
FROM node:20-alpine AS build

WORKDIR /app

# Backend API URL — injected by CI via --build-arg. Vite reads VITE_* env vars at build time.
ARG VITE_SERVER_URL
ENV VITE_SERVER_URL=${VITE_SERVER_URL}

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm ci

# Copy source code
COPY . .

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
