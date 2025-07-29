# Raspberry Pi Deployment Guide

This guide will help you deploy your AccountingDemo application to a Raspberry Pi using Docker.

## Prerequisites

1. **Raspberry Pi** (3B+ or 4 recommended)
2. **Raspberry Pi OS** (64-bit recommended for better Docker support)
3. **Docker** installed on your Pi
4. **Docker Compose** installed on your Pi

## Setup Steps

### 1. Install Docker on Raspberry Pi

```bash
# Update system
sudo apt update && sudo apt upgrade -y

# Install Docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Add your user to docker group
sudo usermod -aG docker $USER

# Install Docker Compose
sudo apt install docker-compose -y

# Reboot to apply changes
sudo reboot
```

### 2. Set up SSH Key Authentication

Generate an SSH key pair for GitHub Actions:

```bash
# On your development machine
ssh-keygen -t rsa -b 4096 -C "github-actions" -f ~/.ssh/github_actions_key

# Copy public key to Pi
ssh-copy-id -i ~/.ssh/github_actions_key.pub pi@your-pi-ip

# Copy private key content (you'll need this for GitHub secrets)
cat ~/.ssh/github_actions_key
```

### 3. Configure GitHub Secrets

In your GitHub repository, go to Settings → Secrets and variables → Actions, and add:

- `PI_HOST`: Your Pi's IP address
- `PI_USERNAME`: Your Pi username (usually `pi`)
- `PI_SSH_KEY`: The private key content from step 2
- `PI_PORT`: SSH port (usually `22`)

### 4. Automatic Deployment

Once you push to your `Testing`, `main`, or `master` branch, the GitHub Actions workflow will:

1. ✅ Build and test your code
2. ✅ Create Docker images
3. ✅ Deploy to your Raspberry Pi automatically
4. ✅ Start the containers

### 5. Manual Deployment (Alternative)

If you prefer manual deployment:

```bash
# Create deployment directory
mkdir ~/accountingdemo
cd ~/accountingdemo
```

Create `docker-compose.prod.yml`:

```yaml
version: '3.8'

services:
  api:
    image: accountingdemo-api:latest
    # or build locally: build: .
    ports:
      - "5000:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      # Add your database connection string here if needed
      # - ConnectionStrings__DefaultConnection=your_database_connection_string
    restart: unless-stopped

  webapp:
    image: accountingdemo-webapp:latest
    # or build locally: build: .
    ports:
      - "8080:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
    depends_on:
      - api
    restart: unless-stopped

  # Optional: Add a database service
  # database:
  #   image: mcr.microsoft.com/mssql/server:2022-latest
  #   environment:
  #     - ACCEPT_EULA=Y
  #     - SA_PASSWORD=YourStrong@Passw0rd
  #   ports:
  #     - "1433:1433"
  #   volumes:
  #     - sqlserver_data:/var/opt/mssql
  #   restart: unless-stopped

# volumes:
#   sqlserver_data:
```

Deploy:
```bash
# Pull latest images (if using Docker Hub)
docker-compose -f docker-compose.prod.yml pull

# Or build locally
docker-compose -f docker-compose.prod.yml up -d --build
```

### 6. Access Your Application

- **Web App**: http://your-pi-ip:8080
- **API**: http://your-pi-ip:5000

## CI/CD Integration

### Automatic Deployment (Recommended)
The `deploy-to-pi.yml` workflow will automatically:
1. Build and test your code
2. Create Docker images
3. Transfer images to your Pi
4. Deploy and start containers

### Manual Deployment
1. Build images on GitHub Actions
2. Download artifacts
3. Transfer to Pi and load images

## Monitoring

```bash
# Check container status
docker-compose -f docker-compose.prod.yml ps

# View logs
docker-compose -f docker-compose.prod.yml logs -f

# Monitor resource usage
docker stats
```

## Troubleshooting

### Common Issues:
1. **Out of memory**: Reduce container memory limits
2. **Slow performance**: Use SSD storage, optimize images
3. **Network issues**: Check firewall settings
4. **SSH connection**: Verify SSH key authentication

### Useful Commands:
```bash
# Restart services
docker-compose -f docker-compose.prod.yml restart

# Update to latest version
docker-compose -f docker-compose.prod.yml pull
docker-compose -f docker-compose.prod.yml up -d

# Clean up old images
docker system prune -a

# Check GitHub Actions logs
# Go to your repo → Actions tab → Click on the workflow run
```

## Security Considerations

1. **Change default passwords**
2. **Use HTTPS in production**
3. **Set up firewall rules**
4. **Regular security updates**
5. **Backup your data**
6. **Use SSH keys instead of passwords**

## Next Steps

1. **Set up a reverse proxy** (nginx) for better routing
2. **Add SSL certificates** for HTTPS
3. **Set up automated backups**
4. **Configure monitoring** (Prometheus, Grafana)
5. **Set up log aggregation** 