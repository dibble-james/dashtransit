# DashTransit

Gain an insight into your MassTransit installation

[![CI Build](https://github.com/dibble-james/dashtransit/actions/workflows/ci.yml/badge.svg?branch=trunk)](https://github.com/dibble-james/dashtransit/actions/workflows/ci.yml)
## Features
- View your audit logs
- See related messages
- See the endpoints associated with a message
- Analyse faults
- Re-send failed messages

### Coming soon...
- Search audit logs
- Send new message
- Edit and re-send

## Supported Configurations
More to follow but for now we support:

| Transport |
| -- |
| RabbitMQ |

| Storage |
| -- |
| SQL Server |

## Getting started
DashTransit is distributed as a Docker container.

If you're using storage that requires a schema, run the migrations for the appropriate database engine.

Just provide connection strings for the storage and transport. The web app is exposed on port 80.

```
docker run -d -p 80:<desired port> -n dashtransit -e connectionstrings__transport= -e connectionstrings__storage= ghcr.io/dibble-james/dashtransit
```
