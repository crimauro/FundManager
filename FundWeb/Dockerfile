# Use a Node.js base image for both build and serving
FROM node:23-bullseye-slim AS build

# Set the working directory
WORKDIR /app

# Copy configuration and dependency files
COPY package.json package-lock.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application files
COPY . .

# Build the Angular application
RUN npm run build

# Final stage with minimal footprint
FROM node:23-bullseye-slim

# Set working directory
WORKDIR /app

# Install http-server globally
RUN npm install -g http-server

# Copy only the built application from the build stage
COPY --from=build /app/dist/fundweb /app

# Expose port 80
EXPOSE 80

# Command to start http-server (serving from current directory)
CMD ["http-server", "browser","-p", "80", "--cors", "-c-1"]