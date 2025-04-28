# Use a Node.js base image to build the application
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
RUN npm run build -- --output-path=dist/fundweb --configuration production

# Use an Nginx base image to serve the application
FROM nginx:alpine

# Copy the built files to the Nginx directory
COPY --from=build /app/dist/fundweb /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Command to start Nginx
CMD ["nginx", "-g", "daemon off;"]