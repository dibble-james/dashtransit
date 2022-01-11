# DashTransit

Gain an insight into your MassTransit installation

[![CI Build](https://github.com/dibble-james/dashtransit/actions/workflows/ci.yml/badge.svg?branch=trunk)](https://github.com/dibble-james/dashtransit/actions/workflows/ci.yml)

![image](https://user-images.githubusercontent.com/11923585/149025625-29cadb55-cade-4b02-89cc-3095e605f93d.png)

![image](https://user-images.githubusercontent.com/11923585/149025720-f2c847ca-520a-41ac-b270-7f7a9a0095c1.png)

![image](https://user-images.githubusercontent.com/11923585/149025825-11539fe3-1a9b-45ad-ac0c-d5478e12d3e9.png)

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
| --------- |
| RabbitMQ  |

| Storage    |
| ---------- |
| SQL Server |

## Getting started

DashTransit is distributed as a Docker container. Both Linux and Windows containers are supported.

You can create the tables DashTransit requires by migrating using:

```
docker run --name dashtransit-migrations -e store__connection= ghcr.io/dibble-james/dashtransit migrate
```

Just provide connection strings for the storage and transport. The web app is exposed on port 80.

```
docker run -d -p 80:<desired port> --name dashtransit -e transport__connection= -e store_connection= ghcr.io/dibble-james/dashtransit
```
