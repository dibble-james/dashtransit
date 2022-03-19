# DashTransit

Gain an insight into your MassTransit installation

[![CI Build](https://github.com/dibble-james/dashtransit/actions/workflows/ci.yml/badge.svg?branch=trunk)](https://github.com/dibble-james/dashtransit/actions/workflows/ci.yml)

![Dashboard](https://user-images.githubusercontent.com/11923585/159138108-cdff0fa5-6cc7-427f-bd2c-a23e17b1e816.png)
![Message](https://user-images.githubusercontent.com/11923585/159138157-5a9ab55d-342a-479c-98d8-cbc3f0f368fc.png)
![Editor](https://user-images.githubusercontent.com/11923585/159138195-8e2b442b-afe8-4c84-969f-67883595680f.png)
![Fault](https://user-images.githubusercontent.com/11923585/149025825-11539fe3-1a9b-45ad-ac0c-d5478e12d3e9.png)

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

| Storage    | Provider Variable |
| ---------- | ----------------- |
| SQL Server | sqlserver         |
| Postgres   | postgres          |

## Getting started

DashTransit is distributed as a Docker container. Both Linux and Windows containers are supported.

You can create the tables DashTransit requires by migrating using:

```
docker run --name dashtransit-migrations -e store__provider=<PROVIDER VARIABLE> -e store__connection= ghcr.io/dibble-james/dashtransit migrate
```

Just provide connection strings for the storage and transport. The web app is exposed on port 80.

```
docker run -d -p <desired port>:80 --name dashtransit -e transport__connection= -e store__provider=<PROVIDER VARIABLE> -e store__connection= ghcr.io/dibble-james/dashtransit
```
